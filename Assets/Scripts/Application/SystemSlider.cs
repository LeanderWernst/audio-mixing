using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemSlider : MonoBehaviour
{
    private GameObject applicationSettings;
    private ApplicationData applicationData;
    private Slider slider;

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

        slider = gameObject.GetComponent<Slider>();
    }
    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { applicationData.ChangeSystemVolume(slider.value); });
    }
}
