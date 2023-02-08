using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SceneStartup : MonoBehaviour
{

    private GameObject applicationSettings;
    private ApplicationData applicationData;

    private GameObject hintPanel;
    private GameObject channelList;

    [SerializeField] private AudioMixer mixer;

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


        hintPanel = GameObject.FindGameObjectWithTag("HelpPanel");
        channelList = GameObject.FindGameObjectWithTag("ChannelListOverlay");

        // Update references in applicationData
        applicationData.hintPanel = hintPanel;
        applicationData.channelList = channelList;
        applicationData.UpdateReferences();
        applicationData.HidePanels();
    }
    // Start is called before the first frame update
    void Start()
    {
        applicationData.ChangeMasterVolume(applicationData.masterVolume);
        applicationData.ChangeSystemVolume(applicationData.systemVolume);

        // Set Channel volumes to -80 to correspond to fader start positions
        mixer.SetFloat("Channel1Volume", -80);
        mixer.SetFloat("Channel2Volume", -80);
        mixer.SetFloat("Channel3Volume", -80);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
