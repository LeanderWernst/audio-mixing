using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmLeaveButton : MonoBehaviour
{
    private GameObject applicationSettings;
    private ApplicationData applicationData;
    private UnityEngine.UI.Button button;

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

        button = gameObject.GetComponent<UnityEngine.UI.Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(delegate { applicationData.QuitToMenu(); });
    }
}
