using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Drag your containers here in the Inspector
    [SerializeField] GameObject startSceneObject;
    [SerializeField] GameObject gameWorldObject; // Your Grid
    [SerializeField] GameObject battleSystemObject; 

    [SerializeField] Camera mainGameCamera; 
    [SerializeField] Camera battleCamera;
    
    // Use Awake to set the starting view
    private void Awake()
    {
        // 1. Initial State: Only the Start Screen is visible and active
        if (startSceneObject != null)
            startSceneObject.SetActive(true); 
        
        if (gameWorldObject != null)
            gameWorldObject.SetActive(false); // Hide Grid/Player
            
        if (battleSystemObject != null)
            battleSystemObject.SetActive(false); // Hide Battle UI
    }

    // Public method called by the StartGameButton
    public void StartGame()
    {
        Debug.Log("Starting the main game world...");

        // 1. DISABLE the Start Screen elements
        if (startSceneObject != null)
            startSceneObject.SetActive(false);

        // 2. ENABLE the Main Game World (Grid, Player, etc.)
        if (gameWorldObject != null)
            gameWorldObject.SetActive(true);

        // 3. Re-enable overworld SpeechBubble
        SpeechBubble speechBubble = FindFirstObjectByType<SpeechBubble>();
        if (speechBubble != null)
        {
            // Find the player object
            var player = FindFirstObjectByType<PlayerController>();
            if (player != null)
            {
                // Switch back to overworld mode and set target
                speechBubble.ExitBattle(player.transform);
                Debug.Log("💬 SpeechBubble returned to overworld mode.");
            }
}
        
        // 3. Keep the BattleSystem DISABLED until combat starts
    }
    
    public void StartBattleSequence()
    {
        // DISABLE the Overworld (Grid)
        if (gameWorldObject != null)
            gameWorldObject.SetActive(false);
        // DISABLE the Main Camera
        if (mainGameCamera != null)
            mainGameCamera.enabled = false; 

        // ENABLE the Battle System UI and Logic
        if (battleSystemObject != null)
            battleSystemObject.SetActive(true);
            
        // ENABLE the Battle Camera
        if (battleCamera != null)
            battleCamera.enabled = true;
        
        // 5. Start the battle logic
        BattleSystem battleSystem = battleSystemObject.GetComponentInChildren<BattleSystem>();
        if (battleSystem != null)
        {
            StartCoroutine(battleSystem.SetupBattle());
        }
    }
}
