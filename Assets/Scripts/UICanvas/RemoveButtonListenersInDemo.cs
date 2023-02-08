using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveButtonListenersInDemo : MonoBehaviour
{

    private GameObject applicationSettings;
    private ApplicationData applicationData;
    private void Awake()
    {
        applicationSettings = GameObject.FindGameObjectWithTag("ApplicationSettings");
        if (applicationSettings == null)
        {
            applicationSettings = new GameObject("ApplicationSettings");
            applicationSettings.tag = "ApplicationSettings";
            applicationSettings.AddComponent<ApplicationData>();
        }
        applicationData = applicationSettings.GetComponent<ApplicationData>();
    }
    void Start()
    {
        if (applicationData.demoMode)
        {
            UnityEngine.UI.Button[] buttons = GetComponentsInChildren<UnityEngine.UI.Button>();

            foreach (UnityEngine.UI.Button button in buttons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }
}

