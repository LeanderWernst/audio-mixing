using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Fader : MonoBehaviour
{
    private GameObject applicationSettings;
    private ApplicationData applicationData;

    [SerializeField] private float upperPosBoundary = 0.0236f;
    [SerializeField] private float lowerPosBoundary = 0.06782f;
    [SerializeField] private float sensitivityY = 0.05f;

    private Vector3 initialPosition;

    private PositionValueRelation[] faderPvr = FaderPvr.relation;
    public float value;
    private ValueStorage valueStorage;
    private float verticalMovement;
    public bool isClicked = false;
    public AudioController audioController;
    TextMeshProUGUI canvasValueText;
    public string channel;

    private bool isMouseOverUI = false;

    private List<string> blockedChannels = new List<string> { "Channel1", "Channel2", "Channel3" };
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
    }

    void Start()
    {
        valueStorage = gameObject.GetComponent<ValueStorage>();
        audioController = GameObject.Find("PanelKeys").GetComponent<AudioController>();
        var parent = transform;
        while (!parent.CompareTag("Channel"))
        {
            parent = parent.parent;
        }
        channel = parent.name;
        SlideFader(0, initialMove: true); // Initial "move" to get initial value from position

        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update () 
    {
        if (isClicked && (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0))
        {
            SlideFader(Input.mouseScrollDelta.y / 2);
        }
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { isMouseOverUI = true; }
        else { isMouseOverUI = false; }
    }

    private void OnMouseDrag()
    {
        // abort if mouse is over UI
        if (isMouseOverUI) { return; }

        SlideFader(Input.GetAxis("Mouse Y"));
    }

    float GetNonLinearFaderValue(PositionValueRelation[] pvr)
    {
        var pos = transform.localPosition.x;
        foreach (var relation in faderPvr)
        {
            if (pos >= relation.positions[0] && pos <= relation.positions[1])
            {
                return GetFaderValue(
                    relation.positions[0], relation.positions[1], 
                    relation.values[0], relation.values[1]
                    );
            }
        }
        return 0f;
    }

    /**
     * Function calculates a value between 'min' and 'max'
     * based on the game objects position. This works if the
     * relation between scale and position is linear.
     * Otherwise function 'GetNonLinearFaderValue' ist needed.
     */
    float GetFaderValue(float upperBound, float lowerBound, float scaleMax, float scaleMin)
    {
        var pos = transform.localPosition.x;
        return (pos - lowerBound) * (scaleMax - scaleMin) / (upperBound - lowerBound) + scaleMin;

    }

    public float GetNonLinearFaderPosition(float value)
    {
        foreach (var relation in faderPvr)
        {
            if (value <= relation.values[0] && value >= relation.values[1])
            {
                return GetFaderPosition(
                    relation.positions[0], relation.positions[1],
                    relation.values[0], relation.values[1],
                    value
                    );
            }
        }
        return 0f;
    }

    public float GetFaderPosition(float upperBoundPos, float lowerBoundPos, float scaleMax, float scaleMin, float value)
    {
        return (value - scaleMin) * (upperBoundPos - lowerBoundPos) / (scaleMax - scaleMin) + lowerBoundPos;
    }

    private void SlideFader(float inputForce, bool initialMove = false)
    {
        // Slide Fader only when it's the current target object or not needed for future interactions
        if (im.GetCurrentInteraction().TargetObject == gameObject
            || im.ObjectIsInTargetObjects(gameObject) 
            || !blockedChannels.Contains(channel) 
            || initialMove
            || im.FinalMixingIsActive())
        {
            verticalMovement = inputForce * sensitivityY * Time.deltaTime;
            float posX = transform.localPosition.x - verticalMovement;
            float clampedPosX = Mathf.Clamp(posX, upperPosBoundary, lowerPosBoundary);
            transform.localPosition = new Vector3(clampedPosX, transform.localPosition.y, transform.localPosition.z);
            value = GetNonLinearFaderValue(faderPvr);
            ChangeValueText();
            valueStorage.SetValue(value, gameObject);
            audioController.SetFaderVolume(transform.name, channel, value);

            if (!initialMove) // prevent audiovolume from being changed when value not yet set correctly
            { 
                ChangeVolumeInSettings();
            }
        }
        else
        {
            ChangeValueText();
        }
    }

    private void ChangeValueText()
    {
        if (value == -80)
            canvasValueText.text = "-" + "\u221E" + " dB";
        else
            canvasValueText.text = value.ToString("F2") + " dB";
    }

    private void ChangeVolumeInSettings()
    {
        if (channel == "Master")
        {
            Debug.Log("MasterVol: " + value);
            applicationData.masterVolume = (value + 80) / 90;
            applicationData.settingsPanel.masterSlider.value = applicationData.masterVolume;
        }
        else if (channel == "StereoInput1")
        {
            Debug.Log("SpeechVol: " + value);
            applicationData.systemVolume = (value + 80) / 90;
            applicationData.settingsPanel.systemSlider.value = applicationData.systemVolume;
        }
    }


    //////////////////////////////////////////////////
    // FUNCTIONS FOR DEMO MODE

    // To be called from interaction in demo mode
    public void AnimateFader(float targetValue, float animationTime)
    { 
        var targetPos = new Vector3(GetNonLinearFaderPosition(targetValue), transform.localPosition.y, transform.localPosition.z);
        SlideFader(targetPos, animationTime);
    }
    
    // To be called when going backwards
    public void SetToInitialPosition()
    {
        StopAllCoroutines();
        transform.localPosition = initialPosition;
        value = GetNonLinearFaderValue(faderPvr);
        valueStorage.SetValue(value, gameObject);
        audioController.SetFaderVolume(transform.name, channel, value);
        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    // To be called when going forwards
    public void SetToTargetPosition(float targetValue)
    {
        StopAllCoroutines();
        transform.localPosition = new Vector3(GetNonLinearFaderPosition(targetValue), transform.localPosition.y, transform.localPosition.z);
        value = GetNonLinearFaderValue(faderPvr);
        valueStorage.SetValue(value, gameObject);
        audioController.SetFaderVolume(transform.name, channel, value);
        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }
    
    // To be called when watching at step
    private void SlideFader(Vector3 targetPos, float timeToReachInSeconds)
    {
        StartCoroutine(AnimateToTargetPosition(targetPos, timeToReachInSeconds));
    }

    private IEnumerator AnimateToTargetPosition(Vector3 targetPos, float timeToReachInSeconds)
    {
        float elapsedTime = 0f;

        // start from initial position & state
        transform.localPosition = initialPosition;
        value = GetNonLinearFaderValue(faderPvr);
        valueStorage.SetValue(value, gameObject);
        ChangeValueText();
        audioController.SetFaderVolume(transform.name, channel, value);
        
        Vector3 startPos = transform.localPosition;

        yield return new WaitForSeconds(0.5f);

        // Turn on emission
        gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

        yield return new WaitForSeconds(0.5f);

        while (elapsedTime < timeToReachInSeconds)
        {
            float t = elapsedTime / timeToReachInSeconds;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);

            value = GetNonLinearFaderValue(faderPvr);
            ChangeValueText();
            valueStorage.SetValue(value, gameObject);
            audioController.SetFaderVolume(transform.name, channel, value);
            ChangeVolumeInSettings();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPos;

        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        //yield return new WaitForSeconds(0.5f);

        SlideFader(targetPos, timeToReachInSeconds);
    }

    /////////////////////
    // To randomly move in mixing step (demo)
    public void AnimateFaderRandomInOptimum(float _ignored, float animationTime)
    {
        var targetPos = new Vector3(GetNonLinearFaderPosition(Random.Range(-8, 0)), transform.localPosition.y, transform.localPosition.z);
        SlideFaderRandom(targetPos, animationTime);
    }

    public void AnimateFaderRandom(float _ignored, float animationTime)
    {
        var targetPos = new Vector3(GetNonLinearFaderPosition(Random.Range(-6, 2)), transform.localPosition.y, transform.localPosition.z);
        SlideFaderRandom(targetPos, animationTime);
    }

    private void SlideFaderRandom(Vector3 targetPos, float timeToReachInSeconds)
    {
        StartCoroutine(AnimateToTargetPositionRandom(targetPos, timeToReachInSeconds));
    }

    private IEnumerator AnimateToTargetPositionRandom(Vector3 targetPos, float timeToReachInSeconds)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.localPosition;

        // Turn on emission
        gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

        yield return new WaitForSeconds(0.5f);

        while (elapsedTime < timeToReachInSeconds)
        {
            float t = elapsedTime / timeToReachInSeconds;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);

            value = GetNonLinearFaderValue(faderPvr);
            ChangeValueText();
            valueStorage.SetValue(value, gameObject);
            audioController.SetFaderVolume(transform.name, channel, value);
            ChangeVolumeInSettings();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPos;

        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        //yield return new WaitForSeconds(0.5f);

        transform.localPosition = startPos;
        value = GetNonLinearFaderValue(faderPvr);
        valueStorage.SetValue(value, gameObject);
        ChangeValueText();
        audioController.SetFaderVolume(transform.name, channel, value);

        yield return new WaitForSeconds(0.5f);

        SlideFaderRandom(targetPos, timeToReachInSeconds);
    }

}