using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState {Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    public BattleUnit PlayerUnit => playerUnit;


    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox ;

    BattleState state;
    int currentAction;
    int currentMove;

    void Start()
    {
        var bubble = FindFirstObjectByType<SpeechBubble>();
        if (bubble != null)
            Debug.Log($"✅ Found SpeechBubble: {bubble.gameObject.name}");
        else
            Debug.LogWarning("⚠️ No SpeechBubble found in the scene!");

        StartCoroutine(SetupBattle());
    }


    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        // Dialogue when enemy appears
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");

        AITraining trainer = FindFirstObjectByType<AITraining>();
        if (trainer != null)
        {
            trainer.StopOverworldDialogue();
        }

        // ✅ Add the speech bubble here
        SpeechBubble speechBubble = FindFirstObjectByType<SpeechBubble>();
        if (speechBubble != null)
        {
            // tell it we’re in battle mode
            speechBubble.EnterBattle(playerUnit.transform);

            // display some battle intro dialogue above the player unit
            speechBubble.ShowSpeech("I'm ready to fight!", playerUnit.transform);
            Debug.Log("💬 SpeechBubble triggered from BattleSystem.");
        }
        else
        {
            Debug.LogWarning("⚠️ No SpeechBubble found in the scene!");
        }

        yield return new WaitForSeconds(1f);

        // continue normal battle flow
        PlayerAction();
    }


    void PlayerAction(){
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        Debug.Log("Moves available: " + playerUnit.Pokemon.Moves.Count);
        dialogBox.EnableActionSelector(true);
        
    }

    void PlayerMove(){
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove(){
        var move = playerUnit.Pokemon.Moves[currentMove];
    
        // Force the compiler to use the property by explicitly casting to object first
        string attackerName = (object)playerUnit.Pokemon.Base.Name as string;
        string moveName = (object)move.Base.Name as string;

        string message = $"{attackerName} used {moveName}";
        yield return dialogBox.TypeDialog(message);
    }

    private void Update(){
        if (state == BattleState.PlayerAction){
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove){
            HandleMoveSelection();
        }
    }

    void HandleActionSelection(){
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            if(currentAction < 1)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            if (currentAction > 0)
                --currentAction;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentAction == 0){
                //Fight
                PlayerMove();
            }
            else if (currentAction == 1){
                //Run
            }
        }
    }
    // void HandleMoveSelection(){
    //     // Use the count of actual moves to define the upper limit
    //     int maxMoves = playerUnit.Pokemon.Moves.Count - 1; 

    //     if (Input.GetKeyDown(KeyCode.RightArrow)){
    //         if(currentMove < maxMoves && currentMove % 2 == 0)
    //             currentMove++;
    //     }
    //     else if (Input.GetKeyDown(KeyCode.LeftArrow)){
    //         if (currentMove > 0 && currentMove % 2 != 0)
    //             currentMove--;
    //     }
    //     else if (Input.GetKeyDown(KeyCode.DownArrow)){
    //         // Only allow moving down if index is 0 or 1 AND there are moves in the next row
    //         if (currentMove < maxMoves - 1 && currentMove + 2 <= maxMoves)
    //             currentMove += 2;
    //     }
    //     else if (Input.GetKeyDown(KeyCode.UpArrow)){
    //         if (currentMove > 1)
    //             currentMove -= 2;
    //     }
        
    //     // Safety check: Clamp the index to prevent out-of-range errors
    //     currentMove = Mathf.Clamp(currentMove, 0, maxMoves);

    //     // Only update the selection if the Pokémon actually has moves
    //     if (playerUnit.Pokemon.Moves.Count > 0)
    //     {
    //         dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);
    //     }
    // }
    void HandleMoveSelection(){

        int maxMoves = playerUnit.Pokemon.Moves.Count - 1;
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            if(currentMove < playerUnit.Pokemon.Moves.Count - 1)
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (currentMove > 0)
                --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)){
            if (currentMove < playerUnit.Pokemon.Moves.Count - 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            if (currentMove > 1)
                currentMove -= 2;
        }
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Space)){
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }
}
