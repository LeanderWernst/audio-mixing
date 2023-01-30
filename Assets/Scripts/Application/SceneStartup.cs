using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartup : MonoBehaviour
{

    private GameObject applicationSettings;
    private ApplicationData applicationData;

    private void Awake()
    {
        // Read settings from gameobject if present.
        // For debugging purposes, if scene was not started from menu, create settings object
        applicationSettings = GameObject.FindGameObjectWithTag("ApplicationSettings");
        if (applicationSettings == null)
        {
            applicationSettings = new GameObject("ApplicationSettings");
            applicationSettings.tag = "ApplicationSettings";
            applicationSettings.AddComponent<ApplicationData>();
        }
        applicationData = applicationSettings.GetComponent<ApplicationData>();
        
        // Update references
        applicationData.UpdateReferences();
        applicationData.HidePanels();
    }
    // Start is called before the first frame update
    void Start()
    {
        applicationData.ChangeMasterVolume(applicationData.masterVolume);
        applicationData.ChangeSystemVolume(applicationData.systemVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
