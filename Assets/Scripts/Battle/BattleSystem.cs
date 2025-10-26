using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState {Start, ActionSelection, MoveSelection, PerformMove, Busy, BattleOver, PlayerDodge}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    public BattleUnit PlayerUnit => playerUnit;
    [SerializeField] BattleDialogBox dialogBox ;

    [field: SerializeField] public DodgeMenu dodgeMenu { get; private set; }
    Direction attackDirection;

    BattleState state;
    int currentAction;
    int currentMove;

    public enum Direction {Up, Down, Left, Right, None}

    public IEnumerator SetupBattle(){
        playerUnit.Setup();
        enemyUnit.Setup();
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");

        yield return new WaitForSeconds(1f);

        ActionSelection();
    }

    void BattleOver(bool won){
        state = BattleState.BattleOver;
        StartCoroutine(OnBattleOver());
    }

    IEnumerator OnBattleOver(){
        yield return new WaitForSeconds(1f);
        if (playerUnit.Pokemon.HP <= 0){
            yield return dialogBox.TypeDialog($"Battle Over. {enemyUnit.Pokemon.Base.Name} won.");
        }
        else{
            yield return dialogBox.TypeDialog($"Battle Over. {playerUnit.Pokemon.Base.Name} won.");
        }
    }

    void ActionSelection(){
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        Debug.Log("Moves available: " + playerUnit.Pokemon.Moves.Count);
        dialogBox.EnableActionSelector(true);
        
    }

    void MoveSelection(){
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PlayerMove(){
        state = BattleState.PerformMove;
        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return RunMove(playerUnit, enemyUnit, move);
    }

    IEnumerator EnemyMove(){
        state = BattleState.PerformMove;
        var move = enemyUnit.Pokemon.GetRandomMove();
        attackDirection = GetRandomDirection();
        yield return RunMove(enemyUnit, playerUnit, move);
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move){
        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(0.5f);

        bool isMiss = false;
        bool isFainted = false;

        //Dodge Mechanic
        if (targetUnit == playerUnit)
        {
            yield return PlayerDodgePhase();
            Direction playerDodge = GetDirectionFromInt(dodgeMenu.ChosenDirection);

            isMiss = CheckForDodge(attackDirection, playerDodge, move.Base.Accuracy);
        }

        if (isMiss){
            yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} dodged the attack!");
            //Skip damage calculation
        }
        else {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name}'s attack hit!"); 
            yield return new WaitForSeconds(0.5f); // Short pause before damage
            isFainted = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
            targetUnit.Hud.UpdateHP();

            if (isFainted){
                yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} Fainted");
                BattleOver(true);
                yield break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        if (!isFainted && state == BattleState.PerformMove)
        {
            // If it was the player's move, it's now the enemy's turn
            if (sourceUnit == playerUnit)
            {
                StartCoroutine(EnemyMove());
            }
            // If it was the enemy's move, it's now the player's turn to choose
            else
            {
                ActionSelection();
            }
        }
    }

    IEnumerator PlayerDodgePhase(){
        state = BattleState.PlayerDodge;
        yield return dialogBox.TypeDialog("Quick! Choose a dodge direction!");

        // Reset and show the menu
        dodgeMenu.ResetState(); 
        
        // Loop while the player has NOT confirmed a choice
        yield return new WaitUntil(() => dodgeMenu.IsConfirmed); 

        // Return to the main battle flow
        state = BattleState.PerformMove;
    }

    // Helper to determine if the dodge was successful (true=miss, false=hit)
    bool CheckForDodge(Direction attackDir, Direction dodgeDir, int accuracy)
    {
        // 1. Check for 100% Miss (Opposite Direction)
        if (attackDir == Direction.Right && dodgeDir == Direction.Left) return true;
        if (attackDir == Direction.Left && dodgeDir == Direction.Right) return true;
        if (attackDir == Direction.Up && dodgeDir == Direction.Down) return true;
        if (attackDir == Direction.Down && dodgeDir == Direction.Up) return true;

        // 2. Check for Neutral Hit/Miss (Standard Accuracy)
        // If directions are not opposite, check against the move's accuracy.
        // The higher the accuracy, the less likely the player is to miss.
        
        // Generate a random number (0 to 100)
        int randomRoll = Random.Range(1, 101); 
        
        // If the random roll is greater than the move's accuracy, it's a miss (dodge succeeds).
        if (randomRoll > accuracy)
            return true; 
        
        // Otherwise, the move hits.
        return false;
    }

    // Converts the integer input from the UI (0, 1, 2, 3) back to an Enum
    Direction GetDirectionFromInt(int choice)
    {
        if (choice == 0) return Direction.Up;
        if (choice == 1) return Direction.Down;
        if (choice == 2) return Direction.Left;
        if (choice == 3) return Direction.Right;
        return Direction.None; // Should only be used if cancel/error occurs
    }

    Direction GetRandomDirection()
    {
        int randomInt = Random.Range(0, 4); 
        return (Direction)randomInt;
    }   

    private void Update(){
        if (state == BattleState.ActionSelection){
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection){
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
                MoveSelection();
            }
            else if (currentAction == 1){
                //Run
            }
        }
    }
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
            StartCoroutine(PlayerMove());
        }
    }

    
}
