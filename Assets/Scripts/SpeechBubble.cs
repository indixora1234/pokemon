using TMPro;
using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI textBox;

    [Header("Targets")]
    public Transform overworldTarget;  // The Player
    public Transform battleTarget;     // The PlayerUnit

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 2f, 0);
    public float textSpeed = 0.03f;
    public float displayTime = 2f;

    private Camera mainCamera;
    private Camera battleCamera;
    private Coroutine typingCoroutine;
    private bool isInBattle = false;

    void Start()
    {
        // Try to find cameras automatically
        mainCamera = GameObject.Find("Main Camera")?.GetComponent<Camera>();
        battleCamera = GameObject.Find("BattleCamera")?.GetComponent<Camera>();

        textBox.gameObject.SetActive(false);
    }

    public void SetMode(bool battleMode)
    {
        // true = battle mode, false = overworld mode
        isInBattle = battleMode;
    }

    public void ShowSpeech(string message)
    {
        // Pick the right target and camera based on mode
        Transform target = isInBattle ? battleTarget : overworldTarget;

        if (target == null)
        {
            Debug.LogWarning("SpeechBubble: No target set for current mode!");
            return;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message, target));
    }

    private IEnumerator TypeText(string message, Transform target)
    {
        textBox.text = "";
        textBox.gameObject.SetActive(true);

        foreach (char c in message)
        {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(displayTime);
        textBox.gameObject.SetActive(false);
    }

    void Update()
    {
        Transform target = isInBattle ? battleTarget : overworldTarget;
        Camera activeCam = isInBattle ? battleCamera : mainCamera;

        if (target != null && activeCam != null)
        {
            Vector3 screenPos = activeCam.WorldToScreenPoint(target.position + offset);
            textBox.rectTransform.position = screenPos;
        }
    }
}

