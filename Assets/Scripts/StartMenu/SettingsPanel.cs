using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{

    private GameObject applicationSettings;
    private ApplicationData applicationData;
    
    [HideInInspector] public Slider masterSlider;
    [HideInInspector] public Slider systemSlider;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        masterSlider.value = applicationData.masterVolume;
        systemSlider.value = applicationData.systemVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
