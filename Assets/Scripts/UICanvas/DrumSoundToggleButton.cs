using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrumSoundToggleButton : MonoBehaviour
{

    private GameObject applicationSettings;
    private ApplicationData applicationData;

    [SerializeField] private InteractionManager interactionManager;
    [SerializeField] private Sprite spriteOn;
    [SerializeField] private Sprite spriteOff;

    private bool drumsOn = true;

    private Image image;


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

        image = gameObject.GetComponent<Image>();

        if (applicationData.demoMode)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    public void ToggleDrumsButton()
    {
        drumsOn = !drumsOn;
        SetSprite();
        if (!drumsOn)
        {
            interactionManager.audioController.mixer.SetFloat("FaderMasterL", -80);
        }
        else
        {
            interactionManager.audioController.mixer.SetFloat("FaderMasterL", applicationData.masterVolume);
        }
    }

    private void SetSprite()
    {
        if (drumsOn)
        {
            image.sprite = spriteOn;
        }
        else
        {
            image.sprite = spriteOff;
        }
    }

    public void SetDrumSoundsOn()
    {
        if (gameObject.activeInHierarchy && !applicationData.settingsPanel.gameObject.activeInHierarchy)
        {
            drumsOn = true;
            SetSprite();
            interactionManager.audioController.mixer.SetFloat("FaderMasterL", applicationData.masterVolume);
        }
    }
}
