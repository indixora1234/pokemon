using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI speechText;
    [SerializeField] private Image background;

    [Header("Typing Settings")]
    [SerializeField] private int lettersPerSecond = 30;
    [SerializeField] private float holdSeconds = 2f;
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.75f);

    [Header("Target Settings")]
    [SerializeField] private Transform overworldTarget;
    [SerializeField] private Transform battleTarget;
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 2.5f, 0);

    [Header("Camera References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera battleCamera;

    private Canvas canvas;
    private RectTransform root;
    private Coroutine typingCoroutine;

    private AITraining aiTraining;
    public bool IsInBattle { get; private set; }

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        root = GetComponent<RectTransform>();

        if (mainCamera == null)
            mainCamera = GameObject.Find("Main Camera")?.GetComponent<Camera>();
        if (battleCamera == null)
            battleCamera = GameObject.Find("BattleCamera")?.GetComponent<Camera>();

        aiTraining = FindFirstObjectByType<AITraining>();

        if (background != null)
        {
            background.color = backgroundColor;
            background.gameObject.SetActive(false);
        }

        if (speechText != null)
        {
            speechText.text = "";
            speechText.color = textColor;
            speechText.gameObject.SetActive(false);
        }

        transform.localScale = Vector3.one;
    }

    // 🔥 Called when entering battle
    public void EnterBattle(Transform playerUnit)
    {
        IsInBattle = true;
        if (playerUnit != null) battleTarget = playerUnit;
        if (canvas != null && battleCamera != null)
            canvas.worldCamera = battleCamera;

        Debug.Log("💬 SpeechBubble switched to Battle Mode.");

        // Pause overworld chatter & analyze combat
        if (aiTraining != null)
        {
            aiTraining.PauseOverworldDialogue();
            Debug.Log("⚔️ Paused overworld chatter & analyzing combat.");
            aiTraining.AnalyzeCombatData();
        }
    }

    // 🌿 Called when leaving battle
    public void ExitBattle(Transform player)
    {
        IsInBattle = false;
        if (player != null) overworldTarget = player;
        if (canvas != null && mainCamera != null)
            canvas.worldCamera = mainCamera;

        Debug.Log("💬 SpeechBubble switched to Overworld Mode.");

        // Resume overworld dialogue
        if (aiTraining != null)
        {
            aiTraining.ResumeOverworldDialogue();
            ShowSpeech("Good battle! Let's keep training!");
        }
    }

    // 🗨️ Display speech text
    public void ShowSpeech(string message)
    {
        var target = IsInBattle ? battleTarget : overworldTarget;
        if (target == null)
        {
            Debug.LogWarning("❌ SpeechBubble: No target assigned.");
            return;
        }
        ShowSpeech(message, target);
    }

    public void ShowSpeech(string message, Transform target)
    {
        if (speechText == null || canvas == null) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeDialogRoutine(message, target));
    }

    private IEnumerator TypeDialogRoutine(string message, Transform target)
    {
        if (background != null) background.gameObject.SetActive(true);
        if (speechText != null) speechText.gameObject.SetActive(true);

        speechText.text = "";

        foreach (var letter in message.ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(holdSeconds);

        if (background != null) background.gameObject.SetActive(false);
        if (speechText != null) speechText.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        Transform target = IsInBattle ? battleTarget : overworldTarget;
        if (target == null || canvas == null) return;

        Camera cam = (IsInBattle ? battleCamera : mainCamera);
        if (cam == null) return;

        if (canvas.worldCamera != cam)
            canvas.worldCamera = cam;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position + worldOffset);
        if (screenPos.z < 0) return; // behind camera

        RectTransform canvasRect = (RectTransform)canvas.transform;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            cam,
            out Vector2 localPos))
        {
            root.anchoredPosition = localPos;
        }
    }
}
