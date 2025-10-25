using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond ;
    [SerializeField] Color highlightedColor ;
    [SerializeField] Color normalColor;
    [SerializeField] Text dialogText ;
    [SerializeField] GameObject actionSelector ;
    [SerializeField] GameObject moveSelector ;
    [SerializeField] GameObject moveDetails ;

    [SerializeField] List<Text> actionTexts ;
    [SerializeField] List<Image> actionImages;
    [SerializeField] List<Text> moveTexts ;

    [SerializeField] Text ppText ;
    [SerializeField] Text typeText ;

    private List<Color> originalColors;

    private void Awake(){
        //Initialize the list
        originalColors = new List<Color>();
        // Store the initial color of each image component
        foreach (var image in actionImages)
        {
            if (image != null)
            {
                originalColors.Add(image.color);
            }
            else
            {
                // Add a default color if the slot is empty to prevent crashes
                originalColors.Add(Color.white); 
            }
        }
    }    

    public void SetDialog(string dialog){
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog){
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
    }

    public void EnableDialogText(bool enabled){
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled){
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled){
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
        foreach (var moveText in moveTexts)
        {
            if (moveText != null) 
                moveText.enabled = enabled;
        }

    }
    public void UpdateActionSelection(int selectedAction){
        // Check to ensure initialization happened
        if (originalColors == null || originalColors.Count != actionImages.Count)
        {
            // Safety check in case Awake hasn't run or lists are mismatched
            Awake(); 
        }
        
        // Use Math.Min for safety in case list sizes are unequal
        int actionCount = Mathf.Min(actionTexts.Count, actionImages.Count);

        for (int i = 0; i < actionCount; ++i) 
        {
            // Ensure we don't crash and the original color exists
            if (actionImages[i] == null || originalColors.Count <= i) continue;

            if (i == selectedAction)
            {
                // Highlighted State
                actionTexts[i].color = highlightedColor;
                actionImages[i].color = highlightedColor; 
            }
            else
            {
                // Unselected State: Use the stored original color
                actionTexts[i].color = Color.black; 
                actionImages[i].color = originalColors[i]; 
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move){
        for (int i = 0; i < moveTexts.Count; ++i){
            if (i == selectedMove){
                moveTexts[i].color = highlightedColor;
            }
            else {
                moveTexts[i].color = Color.black;
            }

            ppText.text = $"PP {move.PP}/{move.Base.PP}";
            typeText.text = move.Base.Type.ToString();
        }
    }

    public void SetMoveNames(List<Move> moves){
        for (int i = 0; i < moveTexts.Count; ++i){
            moveTexts[i].text = "";
            moveTexts[i].color = Color.black;
            if (i < moves.Count){
                moveTexts[i].text = moves[i].Base.Name;
                Debug.Log("Move added.");
            }
            else{
                moveTexts[i].text = "-";
            }
        }

    }
}
