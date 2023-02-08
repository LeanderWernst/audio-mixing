using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechToggleButton : MonoBehaviour
{

    private GameObject applicationSettings;
    private ApplicationData applicationData;

    [SerializeField] private InteractionManager interactionManager;
    [SerializeField] private Sprite spriteOn;
    [SerializeField] private Sprite spriteOff;

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

        if (!applicationData.demoMode)
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
        if (applicationData.speakInstructions)
        {
            image.sprite = spriteOn;
        }
        else
        {
            image.sprite = spriteOff;
        }
    }

    public void ToggleSpeechButton()
    {
        applicationData.speakInstructions = !applicationData.speakInstructions;
        SetSprite();
        if (!applicationData.speakInstructions)
        {
            interactionManager.audioSource.Stop();
        }
    }

    private void SetSprite()
    {
        if (applicationData.speakInstructions)
        {
            image.sprite = spriteOn;
        }
        else
        {
            image.sprite = spriteOff;
        }
    }
}
