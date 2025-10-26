using UnityEngine;

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake()
    {
        // 1. Check if another AudioManager already exists
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioSourceTag");

        if (objs.Length > 1)
        {
            // If a duplicate exists, destroy this new one
            Destroy(this.gameObject);
        }
        else
        {
            // 2. Set this object to persist across scene loads
            DontDestroyOnLoad(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        
        // This ensures the initial music starts if PlayOnAwake is checked
        if (audioSource != null && audioSource.clip != null && audioSource.playOnAwake)
        {
            audioSource.Play();
        }
    }

    public void PlayMusic(AudioClip newClip, float startTime = 0f)
    {
        if (audioSource == null) return;

        // Stop the current music
        audioSource.Stop();

        // Assign the new clip and play it
        audioSource.clip = newClip;
        if (newClip != null && startTime > 0f && startTime < newClip.length)
        {
             audioSource.time = startTime;
        }
        else
        {
             audioSource.time = 0f; // Start from the beginning if time is invalid
        }
        audioSource.Play();
    }
}
