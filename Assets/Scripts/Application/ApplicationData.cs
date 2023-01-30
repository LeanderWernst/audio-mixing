using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ApplicationData : MonoBehaviour
{
    public static ApplicationData instance; // Singleton
    public bool beginnerMode = false;
    public bool equalizerMode = true;
    public bool speakInstructions = true;

    private GameObject exitScreen;

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

        exitScreen = GameObject.FindGameObjectWithTag("ExitPanel");
        exitScreen.SetActive(false);
    }

    public void SetBeginnerOn(bool state)
    {
        beginnerMode = state;
        Debug.Log("BeginnerMode: " + beginnerMode);
    }

    public void SetEqualizerOn(bool state)
    {
        equalizerMode = state;
        Debug.Log("EqualizerMode: " + equalizerMode);
    }

    public void SetSpeechOn(bool state)
    {
        speakInstructions = state;
        Debug.Log("Speech: " + speakInstructions);
    }

    public void ToggleExitScreen()
    {
        exitScreen.SetActive(!exitScreen.activeInHierarchy);
    }
    public void QuitApplication()
    {
        // Compile depending on environment
        #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
