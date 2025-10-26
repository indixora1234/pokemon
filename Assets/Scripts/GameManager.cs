using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Drag your containers here in the Inspector
    [SerializeField] GameObject startSceneObject;
    [SerializeField] GameObject gameWorldObject; // Your Grid
    [SerializeField] GameObject battleSystemObject; // The BattleSystem parent object
    
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
    
    /* * EXAMPLE of how you would start a battle (Future Code)
    public void StartBattle()
    {
        gameWorldObject.SetActive(false); // Hide the Overworld
        battleSystemObject.SetActive(true); // Show the Battle UI
        
        // This is where you would swap the Main Camera for the BattleCamera
        MainCamera.enabled = false;
        BattleCamera.enabled = true;
    }
    */
}
