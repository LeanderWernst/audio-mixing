using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRendererInDemo : MonoBehaviour
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
    void Start()
    {
        if (applicationData.demoMode)
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.enabled = false;

            // Disable child renderers
            Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in childRenderers)
            {
                childRenderer.enabled = false;
            }
        }
    }
}

