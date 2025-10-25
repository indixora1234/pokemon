using UnityEngine;
using Neocortex;

public class AI : MonoBehaviour
{
    private NeocortexSmartAgent agent;
    public CombatTracker combatTracker;
    private AudioSource audioSource;
    private bool useVoice = false;
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
