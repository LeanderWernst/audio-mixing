using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationData : MonoBehaviour
{
    public static ApplicationData instance; // Singleton
    public bool equalizerMode;
    public bool beginnerMode;
    public bool speakInstructions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
