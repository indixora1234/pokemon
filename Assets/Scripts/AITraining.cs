using UnityEngine;
using Neocortex;
using System.Collections;
using System.Collections.Generic;

public class AITraining : MonoBehaviour
{
    private NeocortexSmartAgent agent;
    public CombatTracker combatTracker;
    private AudioSource audioSource;
    private bool useVoice = false;

    [Header("UI Reference")]
    public SpeechBubble speechBubble; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Unity log system working!"); //can delete after
        agent = FindFirstObjectByType<NeocortexSmartAgent>();
        combatTracker = FindFirstObjectByType<CombatTracker>();
        audioSource = GetComponent<AudioSource>();

        if (agent == null)
        {
            Debug.LogError("NeocortexSmartAgent component not found in the scene.");
            return;
        }

        if (combatTracker == null)
        {
            Debug.LogError("CombatTracker component not found in the scene.");
            return;
        }

        agent.OnChatResponseReceived.AddListener((response) =>
        {
            Debug.Log("AI Response Received: " + response.message);

            if (speechBubble != null)
            {
                BattleSystem battleSystem = FindFirstObjectByType<BattleSystem>();

                if (battleSystem != null && battleSystem.PlayerUnit != null)
                {
                    // In battle
                    speechBubble.ShowSpeech(response.message, battleSystem.PlayerUnit.transform);
                }
                else
                {
                    // Overworld (fallback)
                    var player = FindFirstObjectByType<PlayerController>();
                    if (player != null)
                        speechBubble.ShowSpeech(response.message, player.transform);
                }
            }
            
        });

        agent.OnAudioResponseReceived.AddListener((audioClip) =>
        {
            Debug.Log("AI Audio Response Received.");
            if (audioSource != null && audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        });

        Debug.Log("AI and CombatTracker successfully initialized. Press T for Text,  V for Voice");
        StartCoroutine(OverworldDialogueLoop());
        AnalyzeCombatData();        
        // Start the overworld chat loop
        if (overworldLoop == null)
        {
            overworldLoop = StartCoroutine(OverworldDialogueLoop());
        }

    }

    void Update()
    {
        //T, V or space
        if (Input.GetKeyDown(KeyCode.T))
        {
            useVoice = false;
            Debug.Log("In Text Mode");
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            useVoice = true;
            Debug.Log("In Voice Mode");
        }
        

    }


// public void AnalyzeCombatData(bool useVoice = false)
// {
//     if (combatTracker == null || agent == null) return;
    
//     // Check if the battle system is active
//     BattleSystem battleSystem = FindFirstObjectByType<BattleSystem>();
//     if (battleSystem == null || battleSystem.PlayerUnit == null) 
//     {
//         Debug.LogWarning("Cannot analyze combat: Not currently in an active battle.");
//         return;
//     }

//     // 1. Fetch Move Lists and Turn Count
    
//     // Retrieve Player's moves (for Attack suggestions)
//     List<Move> playerMoves = battleSystem.playerUnit.Pokemon.Moves;
//     List<string> playerMoveNames = new List<string>();
//     foreach (var move in playerMoves)
//     {
//         playerMoveNames.Add(move.Base.Name); 
//     }
//     string playerMoveListString = string.Join(", ", playerMoveNames);

//     // Retrieve Enemy's moves (for Defensive context)
//     List<Move> enemyMoves = battleSystem.enemyUnit.Pokemon.Moves;
//     List<string> enemyMoveNames = new List<string>();
//     foreach (var move in enemyMoves)
//     {
//         enemyMoveNames.Add(move.Base.Name); 
//     }
//     string enemyMoveListString = string.Join(", ", enemyMoveNames);
    
//     // Get the turn count from the CombatTracker
//     int totalTurns = combatTracker.totalTurns;
    
    
//     // 2. Determine Turn-Specific Advice (Attack or Dodge)
//     string turnSpecificTip;
//     string playerMoveConstraint = "You MUST choose a Player Move from this list: [" + playerMoveListString + "]. ";


//     // Suggest Attack on Odd/First Turns (Player's Initiative)
//     // totalTurns == 0 is the start; odd numbers (1, 3, 5...) are subsequent player turns
//     if (totalTurns == 0 || totalTurns % 2 != 0) 
//     {
//         // 🛑 FORCE ATTACK SUGGESTION
//         turnSpecificTip = "Your advice MUST be: 'Use [Player Move Name]'. " + 
//                           playerMoveConstraint + 
//                           "DO NOT suggest a dodge direction.";
//     }
//     // Suggest Dodge on Even Turns (Enemy's Initiative/Player Defense)
//     else // Even numbers (2, 4, 6...)
//     {
//         // 🛑 FORCE DODGE SUGGESTION
//         string dodgeHistoryTip = (totalTurns == 2) // Check if this is the first dodge scenario
//             ? "Give a random dodge suggestion [Left, right, up, or down]."
//             : "Base your defensive tip on the opponent's historical attack direction data.";
        
//         turnSpecificTip = "Your advice MUST be: 'Dodge [LEFT/RIGHT/UP/DOWN]'. " + 
//                           "DO NOT suggest an attack move. " + 
//                           dodgeHistoryTip;
//     }

//     // 3. Final Prompt Assembly
//     string prompt = "You are a concise battle coach. Analyze the data and give ONE short, tactical tip for the next action. " +
//                     "The opponent can use these moves: [" + enemyMoveListString + "]. " +
//                     turnSpecificTip +
//                     "Keep your entire response under 20 words.\n\n" + combatData;
    
//     Debug.Log("Combat Analysis Report:\n" + combatData);
    
//     // 4. Send to AI
//     if (useVoice)
//     {
//         agent.TextToAudio(prompt);
//     }
//     else
//     {
//         agent.TextToText(prompt);
//     }
// }

    public void AnalyzeCombatData(bool useVoice = false)
    {
        if (combatTracker == null || agent == null) return;
        string combatData = combatTracker.GenerateCombatReport();

        string prompt = "You are a concise battle coach. Analyze the data and give ONE short tip for the next turn. " + "The first turn is attack options are (Growl, Electric,Tackle)" + "Second turn is dodge suggest a dodge direction (left,right,up,down)" + "When the turn is odd suggest a ATTACK move ONLY"+ "when turn is even suggest a DODGE move ONLY"+ "Be tactical, under 20 words.\n\n" + "after the playerUnit attacks" + combatData;
        Debug.Log("Combat Analysis Report:\n" + combatData);
        if (useVoice)
        {
            agent.TextToAudio(prompt);
        }
        else
        {
            agent.TextToText(prompt);
        }

        // Here you can implement logic to adjust AI behavior based on the report
        // For example, if the player dodges a lot, the AI could focus on predicting dodges
    }

private Coroutine overworldLoop;

    private IEnumerator OverworldDialogueLoop()
    {
        while (true)
        {
            // Wait between 10–20 seconds between each message
            yield return new WaitForSeconds(Random.Range(10f, 20f));

            if (agent == null) continue;

            // Only speak in overworld (not battle)
            BattleSystem battle = FindFirstObjectByType<BattleSystem>();
            if (battle != null && battle.isActiveAndEnabled)
                continue;

            // Friendly Pokémon-style prompt
            string prompt =
                "You are a cheerful Pokémon mentor. Say something short, fun, or motivational about Pokémon or battling. " +
                "Keep it under 20 words and natural.";

            Debug.Log("🌿 Sending overworld prompt to AI...");

            // Voice or text depending on current mode
            if (useVoice)
                agent.TextToAudio(prompt);
            else
                agent.TextToText(prompt);
        }
    }

    
    public void PauseOverworldDialogue()
    {
        StopCoroutine(OverworldDialogueLoop());
        Debug.Log("🛑 Overworld AI dialogue paused (battle started).");
    }

    public void ResumeOverworldDialogue()
    {
        StartCoroutine(OverworldDialogueLoop());
        Debug.Log("🌿 Overworld AI dialogue resumed (battle ended).");
    }

    public void StopOverworldDialogue()
    {
        if (overworldLoop != null)
        {
            StopCoroutine(overworldLoop);
            overworldLoop = null;
        }
    }




}
