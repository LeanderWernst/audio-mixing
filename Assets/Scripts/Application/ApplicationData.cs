using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplicationData : MonoBehaviour
{
    [HideInInspector] public static ApplicationData instance; // Singleton
    [HideInInspector] public bool demoMode = false;
    [HideInInspector] public bool beginnerMode = false;
    [HideInInspector] public bool equalizerMode = true;
    [HideInInspector] public bool speakInstructions = true;

    [SerializeField] private AudioMixer audioMixer;
    [HideInInspector] public float masterVolume = 0.88888889f;
    [HideInInspector] public float systemVolume = 0.88888889f;

    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject exitScreen;
    [SerializeField] private TextMeshProUGUI exitScreenHeadline;
    [SerializeField] private GameObject exitScreenSubline;
    [HideInInspector] public GameObject hintPanel;
    [HideInInspector] public GameObject channelList;
    [HideInInspector] public SettingsPanel settingsPanel;
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateReferences();
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
        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "StartMenu")
        {
            exitScreenHeadline.SetText("Anwendung beenden");
            exitScreenSubline.SetActive(false);
        }
        else
        {
            exitScreenHeadline.SetText("Zurück zum Menü");
            exitScreenSubline.SetActive(true);
        }

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
        controlsScreen.SetActive(!controlsScreen.activeInHierarchy);
    }

    private void CheckControlsPanelInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controlsScreen.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            controlsScreen.SetActive(false);
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

    public void Quit()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "StartMenu")
        {
            QuitApplication();
        }
        else
        {
            QuitToMenu();
            if (exitScreen.activeInHierarchy)
            {
                exitScreen.SetActive(false);
            }
        }
    }
    private void QuitApplication()
    {
        // Compile depending on environment
        #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    private void QuitToMenu()
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

    public void LoadTraining(bool demoActivated)
    {
        demoMode = demoActivated;
        Scene trainingScene = SceneManager.GetSceneByName("Training");
        if (!trainingScene.IsValid())
        {
            SceneManager.LoadScene("Training", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.SetActiveScene(trainingScene);
        }
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
        // find volume sliders
        masterVolumeSlider = settingsPanel.masterSlider;
        systemVolumeSlider = settingsPanel.systemSlider;
        if (settingsScreen != null)
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
        if (hintPanel != null & channelList != null)
        { 
            hintPanel.SetActive(false);
            channelList.SetActive(false);
        }
    }
}
