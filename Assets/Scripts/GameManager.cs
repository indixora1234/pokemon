using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Drag your containers here in the Inspector
    [SerializeField] GameObject startSceneObject;
    [SerializeField] GameObject gameWorldObject; // Your Grid
    [SerializeField] GameObject battleSystemObject; 

    [SerializeField] private Audio audioManager; 
    [SerializeField] private AudioClip battleMusicClip; 

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

        audioManager = FindFirstObjectByType<Audio>();
        if (audioManager == null) Debug.LogError("AudioManager not found!");
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
        
    }
    
    public void StartBattleSequence()
    {
        float battleMusicStartTime = 40f;
        // DISABLE the Overworld (Grid)
        if (gameWorldObject != null)
            gameWorldObject.SetActive(false);
        if (battleSystemObject != null)
            battleSystemObject.SetActive(true);
        
        // 5. Start the battle logic
        BattleSystem battleSystem = battleSystemObject.GetComponentInChildren<BattleSystem>();
        if (battleSystem != null)
        {
            StartCoroutine(battleSystem.SetupBattle());
        }

        if (audioManager != null && battleMusicClip != null)
        {
            audioManager.PlayMusic(battleMusicClip, battleMusicStartTime);
        }
    }
}
