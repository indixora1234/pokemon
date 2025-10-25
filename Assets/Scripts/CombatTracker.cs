using UnityEngine;
using System.Text;

public class CombatTracker : MonoBehaviour
{
    // Player Data
    private int playerFireCount = 0;
    private int playerWaterCount = 0;
    private int playerElectricCount = 0;
    private int playerGrassCount = 0;
    private int playerIceCount = 0;
    private int playerFightingCount = 0;
    private int playerPoisonCount = 0;
    private int playerGroundCount = 0;
    private int playerFlyingCount = 0;
    private int playerPsychicCount = 0;
    private int playerBugCount = 0;
    private int playerRockCount = 0;

    //Player Dodge Directions
    private int playerDodgeTotal, playerDodgeLeft, playerDodgeRight, playerDodgeUp, playerDodgeDown;

    //player Attack Directions
    private int playerRightAttacks, playerLeftAttacks, playerUpperAttacks, playerLowerAttacks;

    //opponent Data
    private int opponentFireCount = 0;
    private int opponentWaterCount = 0;
    private int opponentElectricCount = 0;
    private int opponentGrassCount = 0;
    private int opponentIceCount = 0;
    private int opponentFightingCount = 0;
    private int opponentPoisonCount = 0;
    private int opponentGroundCount = 0;
    private int opponentFlyingCount = 0;
    private int opponentPsychicCount = 0;
    private int opponentBugCount = 0;
    private int opponentRockCount = 0;

    //opponent Dodge Directions
    private int opponentDodgeTotal, opponentDodgeLeft, opponentDodgeRight, opponentDodgeUp, opponentDodgeDown;
    
    //opponent Attack Directions
    private int opponentRightAttacks, opponentLeftAttacks, opponentUpperAttacks, opponentLowerAttacks;

    private int totalTurns = 0;

//Player Move Tracking
    public void RecordPlayerMoves(string action)
    {
        switch (action)
        {
            //Atacks
            case "Fire":
                playerFireCount++;
                break;
            case "Water":
                playerWaterCount++;
                break;
            case "Electric":
                playerElectricCount++;
                break;
            case "Grass":
                playerGrassCount++;
                break;
            case "Ice":
                playerIceCount++;
                break;
            case "Fighting":
                playerFightingCount++;
                break;
            case "Poison":
                playerPoisonCount++;
                break;
            case "Ground":
                playerGroundCount++;
                break;
            case "Flying":
                playerFlyingCount++;
                break;
            case "Psychic":
                playerPsychicCount++;
                break;
            case "Bug":
                playerBugCount++;
                break;
            case "Rock":
                playerRockCount++;
                break;
            case "Dodge":
                playerDodgeTotal++;
                break;

            //Attack Directions
            case "AttackRight":
                playerRightAttacks++;
                break;
            case "AttackLeft":
                playerLeftAttacks++;
                break;
            case "AttackUp":
                playerUpperAttacks++;
                break;
            case "AttackDown":
                playerLowerAttacks++;
                break;

            //Dodges
            case "DodgeLeft":
                playerDodgeLeft++;
                break;
            case "DodgeRight":
                playerDodgeRight++;
                break;
            case "DodgeUp":
                playerDodgeUp++;
                break;
            case "DodgeDown":
                playerDodgeDown++;
                break;

            default:
                Debug.LogWarning("Unknown move type: " + action);
                break;
        }
        totalTurns++;
    }

//Opponent Attack Tracking
    public void RecordOpponentAttacks(string action)
    {
        switch (action)
        {
            //Attack Methods
            case "Fire":
                opponentFireCount++;
                break;
            case "Water":
                opponentWaterCount++;
                break;
            case "Electric":
                opponentElectricCount++;
                break;
            case "Grass":
                opponentGrassCount++;
                break;
            case "Ice": 
                opponentIceCount++;
                break;
            case "Fighting":
                opponentFightingCount++;
                break;
            case "Poison":
                opponentPoisonCount++;
                break;
            case "Ground":
                opponentGroundCount++;
                break;
            case "Flying":
                opponentFlyingCount++;
                break;
            case "Psychic":
                opponentPsychicCount++;
                break;
            case "Bug":
                opponentBugCount++;
                break;
            case "Rock":
                opponentRockCount++;
                break;

            //Attack Directions
            case "Right":
                opponentRightAttacks++;
                break;
            case "Left":
                opponentLeftAttacks++;
                break;
            case "Upper":
                opponentUpperAttacks++;
                break;
            case "Lower":
                opponentLowerAttacks++;
                break;

            //Dodges
            case "DodgeLeft":
                opponentDodgeLeft++;
                break;
            case "DodgeRight":
                opponentDodgeRight++;
                break;
            case "DodgeUp":
                opponentDodgeUp++;
                break;
            case "DodgeDown":
                opponentDodgeDown++;
                break;

            default:
                Debug.LogWarning("Unknown attack direction: " + action);
                break;
        }
    }
    
    public string GenerateCombatReport()
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine("Combat Report:");
        report.AppendLine($"Total Turns: {totalTurns}");
        report.AppendLine("");

        // Player attacks
        report.AppendLine("Player Attack Moves:");
        report.AppendLine($"  Fire: {playerFireCount}, Water: {playerWaterCount}, Electric: {playerElectricCount}, Grass: {playerGrassCount}");
        report.AppendLine($"  Ice: {playerIceCount}, Fighting: {playerFightingCount}, Poison: {playerPoisonCount}, Ground: {playerGroundCount}");
        report.AppendLine($"  Flying: {playerFlyingCount}, Psychic: {playerPsychicCount}, Bug: {playerBugCount}, Rock: {playerRockCount}");
        report.AppendLine($"Attack Directions → Left: {playerLeftAttacks}, Right: {playerRightAttacks}, Up: {playerUpperAttacks}, Down: {playerLowerAttacks}");
        report.AppendLine();

        // Player dodges
        report.AppendLine("Player Dodges:");
        report.AppendLine($"  Total: {playerDodgeTotal} → Left: {playerDodgeLeft}, Right: {playerDodgeRight}, Up: {playerDodgeUp}, Down: {playerDodgeDown}");
        report.AppendLine();

        // Opponent attacks
        report.AppendLine("Opponent Attack Moves:");
        report.AppendLine($"  Fire: {opponentFireCount}, Water: {opponentWaterCount}, Electric: {opponentElectricCount}, Grass: {opponentGrassCount}");
        report.AppendLine($"  Ice: {opponentIceCount}, Fighting: {opponentFightingCount}, Poison: {opponentPoisonCount}, Ground: {opponentGroundCount}");
        report.AppendLine($"  Flying: {opponentFlyingCount}, Psychic: {opponentPsychicCount}, Bug: {opponentBugCount}, Rock: {opponentRockCount}");
        report.AppendLine($"Attack Directions → Left: {opponentLeftAttacks}, Right: {opponentRightAttacks}, Up: {opponentUpperAttacks}, Down: {opponentLowerAttacks}");
        report.AppendLine();

        // Opponent dodges
        report.AppendLine("Opponent Dodges:");
        report.AppendLine($"  Total: {opponentDodgeTotal} → Left: {opponentDodgeLeft}, Right: {opponentDodgeRight}, Up: {opponentDodgeUp}, Down: {opponentDodgeDown}");


        return report.ToString();
    }
public void ResetTracker()
    {
        // Player
        playerFireCount = playerWaterCount = playerElectricCount = playerGrassCount = playerIceCount = 0;
        playerFightingCount = playerPoisonCount = playerGroundCount = playerFlyingCount = playerPsychicCount = 0;
        playerBugCount = playerRockCount = 0;
        playerRightAttacks =  playerLeftAttacks =  playerUpperAttacks =  playerLowerAttacks = 0;
        playerDodgeTotal = playerDodgeLeft = playerDodgeRight = playerDodgeUp = playerDodgeDown = 0;

        // Opponent
        opponentFireCount = opponentWaterCount = opponentElectricCount = opponentGrassCount = opponentIceCount = 0;
        opponentFightingCount = opponentPoisonCount = opponentGroundCount = opponentFlyingCount = opponentPsychicCount = 0;
        opponentBugCount = opponentRockCount = 0;
        opponentRightAttacks = opponentLeftAttacks = opponentUpperAttacks = opponentLowerAttacks = 0;
        opponentDodgeTotal = opponentDodgeLeft = opponentDodgeRight = opponentDodgeUp = opponentDodgeDown = 0;

        totalTurns = 0;
    }
}
