using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeechBubble : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI textBox;
    public Image bubbleImage; // Make sure to assign SpeechBubbleBackground here

    [Header("Targets")]
    public Transform overworldTarget;  // Player in overworld
    public Transform battleTarget;     // PlayerUnit during battle

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
        mainCamera = GameObject.Find("Main Camera")?.GetComponent<Camera>();
        battleCamera = GameObject.Find("BattleCamera")?.GetComponent<Camera>();

        // Ensure both text and bubble start hidden properly
        if (bubbleImage != null)
        {
            bubbleImage.gameObject.SetActive(false);
            Debug.Log("🧩 Start(): Bubble hidden initially, reference OK: " + bubbleImage.name);
        }

        if (textBox != null)
        {
            textBox.gameObject.SetActive(false);
            Debug.Log("🧩 Start(): TextBox hidden initially, reference OK: " + textBox.name);
        }
    }


    void Awake()
    {
        if (bubbleImage == null)
            Debug.LogWarning("❌ bubbleImage is NULL in Awake!");
        else
            Debug.Log("✅ bubbleImage assigned: " + bubbleImage.name);
    }


    /// <summary>
    /// Switch between overworld and battle mode.
    /// </summary>
    public void SetMode(bool battleMode)
    {
        isInBattle = battleMode;

        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = battleMode ? battleCamera : mainCamera;
            Debug.Log($"🎥 SpeechBubbleCanvas now using {(battleMode ? battleCamera.name : mainCamera.name)}");

            if (battleMode)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = battleCamera;
                Debug.Log("⚔️ Switched SpeechBubbleCanvas to BattleCamera mode.");
            }
            else
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = mainCamera;
                Debug.Log("🌍 Switched SpeechBubbleCanvas to MainCamera mode.");
            }
        }
    }


    /// <summary>
    /// Show speech bubble automatically targeting the active mode's character.
    /// </summary>
    public void ShowSpeech(string message)
    {
        gameObject.SetActive(true); // ensure parent is reactivated
        Debug.Log("💬 ShowSpeech (auto-target): " + message);
        Transform target = isInBattle ? battleTarget : overworldTarget;
        if (target == null || textBox == null)
        {
            Debug.LogWarning("SpeechBubble: Missing target or text box.");
            return;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message, target));
    }

    /// <summary>
    /// Show speech bubble at a specific world target (used by AITraining.cs).
    /// </summary>
    public void ShowSpeech(string message, Transform target)
    {
        gameObject.SetActive(true); // ensure parent is reactivated

        Debug.Log("💬 ShowSpeech (manual target): " + message);

        if (target == null)
        {
            Debug.LogWarning("❌ Target is null!");
            return;
        }

        if (textBox == null)
        {
            Debug.LogWarning("❌ textBox is null!");
            return;
        }

        Debug.Log("✅ Passed all null checks, starting TypeText coroutine...");
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message, target));
    }


   private IEnumerator TypeText(string message, Transform target)
    {
        Debug.Log("🌀 Entered TypeText coroutine...");

        // make sure both UI elements are visible
        if (bubbleImage != null)
        {
            bubbleImage.gameObject.SetActive(true);
            Debug.Log("🗨️ Speech bubble enabled: " + bubbleImage.name);
        }
        else
        {
            Debug.LogWarning("⚠️ bubbleImage is null inside TypeText!");
        }

        if (textBox != null)
        {
            textBox.text = "";
            textBox.gameObject.SetActive(true);
        }

        // ✅ Do not wrap yield statements in try/catch
        foreach (char c in message)
        {
            if (textBox != null)
                textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(displayTime);

        if (textBox != null)
            textBox.gameObject.SetActive(false);
        if (bubbleImage != null)
            bubbleImage.gameObject.SetActive(false);

        Debug.Log("💤 Speech bubble hidden after delay.");
    }





    void Update()
    {
        Transform target = isInBattle ? battleTarget : overworldTarget;
        Camera activeCam = isInBattle ? battleCamera : mainCamera;

        if (target == null || activeCam == null || textBox == null || bubbleImage == null)
            return;

        // Convert world position to screen point
        Vector3 screenPos = activeCam.WorldToScreenPoint(target.position + offset);

        // Convert screen point to local position within the canvas
        RectTransform canvasRect = bubbleImage.canvas.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, activeCam, out Vector2 localPos))
        {
            // Apply the position to both text and bubble
            textBox.rectTransform.anchoredPosition = localPos;
            bubbleImage.rectTransform.anchoredPosition = localPos;
        }
    }

}





