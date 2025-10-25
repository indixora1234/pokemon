using UnityEngine;
using Neocortex;

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
        AnalyzeCombatData();        
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
    
    public void AnalyzeCombatData(bool useVoice = false)
    {
        if (combatTracker == null || agent == null) return;
        string combatData = combatTracker.GenerateCombatReport();

        string prompt = "You are a concise battle coach. Analyze the data and give ONE short tip for the next turn. " + "Be tactical, under 20 words.\n\n" + combatData;
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

}
