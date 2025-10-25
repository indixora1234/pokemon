using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public float textSpeed = 0.03f;

    private Coroutine typingCoroutine;

    // Call to show AI Text
    public void ShowText(string message)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message));
    }

    private System.Collections.IEnumerator TypeText(string message)
    {
        textBox.text = "";
        foreach (char c in message)
        {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    // Clear text after a while
    public void ClearAfterDelay(float delay)
    {
        CancelInvoke();
        Invoke(nameof(ClearText), delay);
    }

    private void ClearText()
    {
        textBox.text = "";
    }
}
