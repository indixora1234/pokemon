using UnityEngine;

public class Audio : MonoBehaviour
{
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
    }
}
