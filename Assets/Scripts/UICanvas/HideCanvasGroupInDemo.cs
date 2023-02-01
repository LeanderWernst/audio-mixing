using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCanvasGroupInDemo : MonoBehaviour
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

    // Start is called before the first frame update
    private void Start()
    {
        if (applicationData.demoMode)
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
            }
        }
    }

}
