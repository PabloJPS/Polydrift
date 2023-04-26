using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    private static DontDestroyAudio instance = null;

    // Start is called before the first frame update
    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // Destroy the new instance
            Destroy(this.gameObject);
            return;
        }

        // Set the instance variable to this object
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }
}
