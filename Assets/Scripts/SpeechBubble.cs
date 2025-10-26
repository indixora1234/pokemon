using TMPro;
using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform root;          // SpeechBubbleUI (this)
    [SerializeField] private TextMeshProUGUI textBox;     // child TMP text

    [Header("Targets")]
    [SerializeField] private Transform overworldTarget;   // Player
    [SerializeField] private Transform battleTarget;      // PlayerUnit

    [Header("Settings")]
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 2f, 0);
    [SerializeField] private float typeSpeed = 0.03f;
    [SerializeField] private float holdSeconds = 2f;

    public bool IsInBattle { get; private set; }

    Canvas canvas;
    Camera mainCam;
    Camera battleCam;
    Coroutine typing;

    void Awake()
    {
        // cache refs
        canvas   = GetComponentInParent<Canvas>();
        mainCam  = GameObject.Find("Main Camera")   ?.GetComponent<Camera>();
        battleCam= GameObject.Find("BattleCamera")  ?.GetComponent<Camera>();

        if (!root) root = (RectTransform)transform;
        if (textBox) textBox.gameObject.SetActive(false);
        gameObject.SetActive(true); // keep parent active; we’ll just hide the text
    }

    // ---- Public API ----------------------------------------------------------

    public void EnterBattle(Transform playerUnit)
    {
        IsInBattle = true;
        if (playerUnit) battleTarget = playerUnit;
    }

    public void ExitBattle(Transform player)
    {
        IsInBattle = false;
        if (player) overworldTarget = player;
    }

    /// Show above current mode’s target
    public void ShowSpeech(string message)
    {
        var t = IsInBattle ? battleTarget : overworldTarget;
        if (!t || !textBox) return;

        if (typing != null) StopCoroutine(typing);
        typing = StartCoroutine(TypeRoutine(message, t));
    }

    /// Show above a specific target (optional overload)
    public void ShowSpeech(string message, Transform target)
    {
        if (!target || !textBox) return;

        if (typing != null) StopCoroutine(typing);
        typing = StartCoroutine(TypeRoutine(message, target));
    }

    // ---- Internals -----------------------------------------------------------

    IEnumerator TypeRoutine(string message, Transform target)
    {
        textBox.text = "";
        textBox.gameObject.SetActive(true);

        foreach (char c in message)
        {
            textBox.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        yield return new WaitForSeconds(holdSeconds);
        textBox.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        // pick the active cam & target
        var target = IsInBattle ? battleTarget : overworldTarget;
        if (!target || !canvas || !root) return;

        var cam = IsInBattle && battleCam ? battleCam : mainCam;

        // 🔥 auto-update the canvas camera if needed
        if (canvas.worldCamera != cam)
            canvas.worldCamera = cam;

        Vector3 screen = cam ? cam.WorldToScreenPoint(target.position + worldOffset)
                            : (Vector3)RectTransformUtility.WorldToScreenPoint(null, target.position + worldOffset);

        // convert screen → canvas local
        RectTransform canvasRect = (RectTransform)canvas.transform;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screen,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
                out Vector2 local))
        {
            root.anchoredPosition = local;
        }
    }

}
