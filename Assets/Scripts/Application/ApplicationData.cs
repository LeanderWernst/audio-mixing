using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ApplicationData : MonoBehaviour
{
    public static ApplicationData instance; // Singleton
    [HideInInspector] public bool beginnerMode = false;
    [HideInInspector] public bool equalizerMode = true;
    [HideInInspector] public bool speakInstructions = true;

    [SerializeField] private AudioMixer audioMixer;
    public float masterVolume = 0.88888889f;
    public float systemVolume = 0.88888889f;

    private GameObject controlsPanel;
    private GameObject exitScreen;
    private GameObject settingsScreen;
    private GameObject hintPanel;
    private GameObject channelList;
    public SettingsPanel settingsPanel;
    private Slider masterVolumeSlider;
    private Slider systemVolumeSlider;
    [HideInInspector] public GameObject masterFader;
    [HideInInspector] public GameObject systemFader;

    private void Awake()
    {
        // Create singleton instance, if not existent
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // use screen resolution of running system as application resolution
        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreen);

        // Find necessary objects
        UpdateReferences();

        // Hide deactivated panels
        HidePanels();
    }

    private void Start()
    {

    }

    private void Update()
    {
        CheckInput();
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
        if (settingsScreen.activeInHierarchy)
        {
            settingsScreen.SetActive(false);
        }
    }

    public void ToggleSettingsScreen()
    {
        settingsScreen.SetActive(!settingsScreen.activeInHierarchy);
        if (exitScreen.activeInHierarchy)
        {
            exitScreen.SetActive(false);
        }
    }

    public void ToggleControlsPanel()
    {
        controlsPanel.SetActive(!controlsPanel.activeInHierarchy);
    }

    private void CheckControlsPanelInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controlsPanel.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            controlsPanel.SetActive(false);
        }
    }

    private void CheckExitPanelInput()
    {
        var active = exitScreen.activeInHierarchy;
        // close exit panel on esc
        if (Input.GetKeyDown(KeyCode.Escape) && active)
        {
            ToggleExitScreen();
        }
        // open exit panel on esc when no other panel is active
        else if (Input.GetKeyDown(KeyCode.Escape) && !active && !settingsScreen.activeInHierarchy)
        {
            Debug.Log("HintPanel: " + (hintPanel != null) + " ChannelList: " + (channelList != null));
            if (hintPanel != null & channelList != null && (hintPanel.activeInHierarchy || channelList.activeInHierarchy))
            {
                return;
            }
            ToggleExitScreen();
        }
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

    public void QuitToMenu()
    {
        SceneLoader.LoadScene("StartMenu");
    }

    private void CheckInput()
    {
        CheckExitPanelInput();

        // close seetings screen on esc
        if (Input.GetKeyDown(KeyCode.Escape) && settingsScreen.activeInHierarchy)
        {
            ToggleSettingsScreen();
        }

        // Check STRG for controls panel
        CheckControlsPanelInput();
    }

    public void ChangeMasterVolume(float value) 
    {
        masterVolume = value;
        var valueConverted = (value * 90) - 80;
        audioMixer.SetFloat("FaderMasterL", valueConverted);
        masterFader.transform.localPosition = new Vector3(
                FaderMoveStartMenu.GetNonLinearFaderPosition(valueConverted),
                masterFader.transform.localPosition.y,
                masterFader.transform.localPosition.z
            );
    }

    public void ChangeSystemVolume(float value)
    {
        systemVolume = value;
        var valueConverted = (value * 90) - 80;
        audioMixer.SetFloat("StereoInput1Volume", valueConverted);
        systemFader.transform.localPosition = new Vector3(
                FaderMoveStartMenu.GetNonLinearFaderPosition(valueConverted),
                systemFader.transform.localPosition.y,
                systemFader.transform.localPosition.z
            );
    }

    public void UpdateReferences()
    {
        // find controls panel
        controlsPanel = GameObject.FindGameObjectWithTag("ControlsPanel");

        // find exit panel
        exitScreen = GameObject.FindGameObjectWithTag("ExitPanel");

        // find settings panel
        settingsScreen = GameObject.FindGameObjectWithTag("SettingsPanel");

        hintPanel = GameObject.FindGameObjectWithTag("HelpPanel");
        channelList = GameObject.FindGameObjectWithTag("ChannelListOverlay");

        // find volume sliders
        masterVolumeSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        systemVolumeSlider = GameObject.Find("SystemSlider").GetComponent<Slider>();
        settingsPanel = settingsScreen.GetComponent<SettingsPanel>();
        settingsPanel.masterSlider = masterVolumeSlider;
        settingsPanel.systemSlider = systemVolumeSlider;

        // master and system fader
        masterFader = GameObject.Find("FaderMasterL");
        SpeechFader[] components = FindObjectsOfType<SpeechFader>();
        foreach (SpeechFader component in components)
        {
            if (component.gameObject.tag == "Fader")
            {
                systemFader = component.gameObject;
                break;
            }
        }
    }

    public void HidePanels()
    {
        exitScreen.SetActive(false);
        settingsScreen.SetActive(false);
        controlsPanel.SetActive(false);
        if (hintPanel != null & channelList != null)
        { 
            hintPanel.SetActive(false);
            channelList.SetActive(false);
        }
    }
}
