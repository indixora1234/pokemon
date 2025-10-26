using UnityEngine;
using UnityEngine.UI; // For Button components

public class DodgeMenu : MonoBehaviour
{
    // The direction the player chose (0=Up, 1=Down, 2=Left, 3=Right)
    public int ChosenDirection { get; private set; } = -1; 
    public bool IsConfirmed { get; private set; } = false;

    [SerializeField] Button upButton;
    [SerializeField] Button downButton;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button confirmButton;

    public void OnDirectionSelected(int direction)
    {
        // Direction value comes from the Button's OnClick() event (0, 1, 2, 3)
        ChosenDirection = direction;
        // Logic to highlight the selected button goes here (Optional)
    }

    public void OnConfirm()
    {
        IsConfirmed = true;
        // Optionally disable input here
        gameObject.SetActive(false); // Hide the menu after confirmation
    }

    public void OnCancel()
    {
        ChosenDirection = -1;
        IsConfirmed = true; // Still confirm, but with no choice
        gameObject.SetActive(false);
    }

    // Call this before showing the menu
    public void ResetState()
    {
        ChosenDirection = -1;
        IsConfirmed = false;
        gameObject.SetActive(true);
    }
}
