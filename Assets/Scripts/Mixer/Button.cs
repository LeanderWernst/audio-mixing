using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    private GameObject applicationSettings;
    private ApplicationData applicationData;

    public bool isOn = false;
    private ValueStorage valueStorage;
    public bool isClicked = false;
    private bool isMoving = false;
    private bool hasLED = false;
    [SerializeField] private string LEDTag = "Light";

    private Vector3 initialPosition;
    private bool currentDemoTargetState;

    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float smoothTime = 0.1F;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    TextMeshProUGUI canvasValueText;

    private AudioController audioController;
    public string channel;

    private List<string> blockedChannels = new List<string>{"Channel1", "Channel2", "Channel3"};
    private InteractionManager im;

    private void Awake()
    {
        canvasValueText = GameObject.FindGameObjectWithTag("ValueText").GetComponent<TextMeshProUGUI>();
        im = GameObject.FindGameObjectWithTag("InteractionManager").GetComponent<InteractionManager>();

        applicationSettings = GameObject.FindGameObjectWithTag("ApplicationSettings");
        if (applicationSettings == null)
        {
            applicationSettings = new GameObject("ApplicationSettings");
            applicationSettings.tag = "ApplicationSettings";
            applicationSettings.AddComponent<ApplicationData>();
        }
        applicationData = applicationSettings.GetComponent<ApplicationData>();

        audioController = GameObject.Find("PanelKeys").GetComponent<AudioController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        valueStorage = gameObject.GetComponent<ValueStorage>();
        startPosition = transform.position;
        endPosition = transform.TransformPoint(new Vector3(0, 0, -0.00057567f));
        // startPosition = transform.localPosition;
        // endPosition = new Vector3(startPosition.x, startPosition.y, -0.00057567f);
        hasLED = transform.parent != null && transform.parent.tag == LEDTag;
        if (hasLED)
            transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        
        var parent = transform;
        while (!parent.CompareTag("Channel"))
        {
            parent = parent.parent;
        }
        channel = parent.name;

        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isMoving)
        {
            if (isOn)
                transform.position = Vector3.SmoothDamp(transform.position, endPosition, ref velocity, smoothTime);
            else
                transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, smoothTime);
        }
        if (transform.position == endPosition || transform.position == startPosition)
            isMoving = false;
    }

    private void OnMouseDown()
    {
        // Move only when it is the target object of an interaction
        // or when it is a gameObject that is not in channels 1-3
        if (im.GetCurrentInteraction().TargetObject == gameObject 
            || im.ObjectIsInTargetObjects(gameObject) 
            || !blockedChannels.Contains(channel) 
            || im.FinalMixingIsActive() 
            && !EventSystem.current.IsPointerOverGameObject())
        {
            isOn = !isOn;
            isMoving = true;
            canvasValueText.text = isOn ? "on" : "off";
            valueStorage.SetValue(isOn ? 1f : 0f, gameObject);
            if (hasLED && isOn)
            {
                transform.parent.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            }
            else if (hasLED && !isOn)
            {
                transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            }
            if (isOn)
            {
                audioController.SetButtonOn(transform.name, channel);
            }
            else
            {
                audioController.SetButtonOff(transform.name, channel);
            }
        }
        else 
        {
            canvasValueText.text = isOn ? "on" : "off";
        }
    }

    ///////////////////////////
    /// Functions for demo mode
    /// 

    public void AnimateButton(float targetValue, float timeToReachInSeconds)
    {
        float startValue = isOn ? 1f : 0f;
        StartCoroutine(AnimateToTargetValue(startValue, targetValue, timeToReachInSeconds));
    }

    // To be called when going backwards
    public void SetToInitialPosition()
    {
        StopAllCoroutines();
        if (!currentDemoTargetState)
        {
            transform.position = endPosition;
            isOn = true;
        }
        else
        {
            transform.localPosition = initialPosition;
            isOn = false;
        }

        canvasValueText.text = isOn ? "on" : "off";
        valueStorage.SetValue(isOn ? 1f : 0f, gameObject);

        if (!currentDemoTargetState)
        {
            audioController.SetButtonOn(transform.name, channel);
            if (hasLED) { transform.parent.GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); }
        }
        else
        {
            audioController.SetButtonOff(transform.name, channel);
            if (hasLED) { transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); }
        }
        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        // Workaround to ensure the correct volume (defined in Inspector of InteractionManager)
        if (im.GetCurrentInteraction().volumeBefore != 0)
        {
            audioController.SetChannelToVolume(im.GetCurrentInteraction().volumeBefore, channel);
        }
    }

    // To be called when going forwards
    public void SetToTargetPosition(float targetValue)
    {
        StopAllCoroutines();
        if (targetValue == 1f)
        {
            transform.position = endPosition;
            isOn = true;
        }
        else
        {
            transform.localPosition = initialPosition;
            isOn = false;
        }
        
        canvasValueText.text = isOn ? "on" : "off";
        valueStorage.SetValue(isOn ? 1f : 0f, gameObject);

        if (targetValue == 1f) 
        { 
            audioController.SetButtonOn(transform.name, channel);
            if (hasLED) { transform.parent.GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); }
        }
        else { 
            audioController.SetButtonOff(transform.name, channel);
            if (hasLED) { transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); }
        }
        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        // Workaround to ensure the correct volume (defined in Inspector of InteractionManager)
        if (im.GetCurrentInteraction().volumeAfter != 0)
        {
            audioController.SetChannelToVolume(im.GetCurrentInteraction().volumeAfter, channel);
        }
    }

    private IEnumerator AnimateToTargetValue(float startValue, float targetValue, float timeToReachInSeconds)
    {
        float elapsedTime = 0f;
        currentDemoTargetState = targetValue == 1 ? true : false;

        // set to initial position & state
        if (currentDemoTargetState)
        {
            transform.localPosition = initialPosition;
        }
        else
        {
            transform.position = endPosition;
        }
        isOn = currentDemoTargetState == true ? false : true;
        canvasValueText.text = isOn ? "on" : "off";
        valueStorage.SetValue(isOn ? 1f : 0f, gameObject);

        if (currentDemoTargetState)
        {
            audioController.SetButtonOff(transform.name, channel);
            if (hasLED) { transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); }
        }
        if (!currentDemoTargetState)
        {
            audioController.SetButtonOn(transform.name, channel);
            if (hasLED) { transform.parent.GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); }
        }

        if (im.GetCurrentInteraction().volumeAfter != 0)
        {
            audioController.SetChannelToVolume(im.GetCurrentInteraction().volumeBefore, channel);
        }

        yield return new WaitForSeconds(0.5f);

        // Turn on emission
        gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

        yield return new WaitForSeconds(0.5f);

        while (elapsedTime < timeToReachInSeconds)
        {
            float t = elapsedTime / timeToReachInSeconds;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            // float currentValue = Mathf.Lerp(startValue, targetValue, t);
            if (targetValue == 1.0f)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                if (t >= 0.5f)
                {
                    if (hasLED)
                    {
                        transform.parent.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                    }
                    audioController.SetButtonOn(transform.name, channel);
                    canvasValueText.text = "on";
                }
            }
            else
            {
                transform.position = Vector3.Lerp(endPosition, startPosition, t);
                if (t >= 0.5f)
                {
                    if (hasLED)
                    {
                        transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                    }
                    audioController.SetButtonOff(transform.name, channel);
                    canvasValueText.text = "off";
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isOn = targetValue == 1.0f ? true : false;
        canvasValueText.text = isOn ? "on" : "off";

        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        //yield return new WaitForSeconds(0.5f);

        AnimateButton(targetValue, timeToReachInSeconds);
    }

}
