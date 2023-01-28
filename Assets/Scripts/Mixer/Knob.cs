using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Knob : MonoBehaviour
{
	public bool isClicked = false;

	[SerializeField] private float minRotation = -105.0f;
	[SerializeField] private float maxRotation = 185.0f;
	[SerializeField] private float rotationSpeed = 250.0f;

    private float minValue;
    private float maxValue;

	private float angle;
    public float value;
    private ValueStorage valueStorage;
    private float movementTolerance = 4.5f; // Tolerance for discrepancy between target angle/value and selected angle/value (degrees)

    private KnobType knobType;
    private PositionValueRelation[] knobPvr;

    private AudioController audioController;
    public string channel;
    
    TextMeshProUGUI canvasValueText;

    private List<string> blockedChannels = new List<string> { "Channel1", "Channel2", "Channel3" };
    private InteractionManager im;

    private bool isMouseOverUI = false;

    private void Awake()
    {
        canvasValueText = GameObject.FindGameObjectWithTag("ValueText").GetComponent<TextMeshProUGUI>();
        valueStorage = gameObject.GetComponent<ValueStorage>();
        im = GameObject.FindGameObjectWithTag("InteractionManager").GetComponent<InteractionManager>();
    }
    private void Start()
    {
        angle = transform.localEulerAngles.z;
        angle = angle > maxRotation ? angle - 360 : angle;
        knobType = GetKnobType();
        knobPvr = KnobPvr.Relation(knobType);
        value = GetNonLinearKnobValue(knobPvr);
        audioController = GameObject.Find("PanelKeys").GetComponent<AudioController>();
        
        // Climb parents up to get channel name
        var parent = transform;
        while(!parent.CompareTag("Channel"))
        {
            parent = parent.parent;
        }
        channel = parent.name;

        // Initial "move" to get initial value from position
        TurnKnob(0, initialMove: true);

        // Read min max values
        minValue = knobPvr[0].values[0];
        maxValue = knobPvr[knobPvr.Length-1].values[1];
        Debug.Log(name + ": " + minValue + " | " + maxValue);
    }

    private void Update()
    {
        if (isClicked && (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0)) 
        {
            TurnKnob(Input.mouseScrollDelta.y / 2);
        }
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { isMouseOverUI = true; }
        else { isMouseOverUI = false; }
    }

    void OnMouseDrag()
	{
        // abort if mouse is over UI
        if (isMouseOverUI) { return; }

        TurnKnob(Input.GetAxis("Mouse Y"));
    }

    private void TurnKnob(float inputForce, bool initialMove = false)
    {
        Debug.Log("Force: " + inputForce);
        // Turn Knob only when it's the current target object or not needed for future interactions
        if (im.GetCurrentInteraction().TargetObject == gameObject || !blockedChannels.Contains(channel) || initialMove)
        {
            angle += inputForce * rotationSpeed * Time.deltaTime;
            angle = Mathf.Clamp(angle, minRotation, maxRotation);
            angle = angle > maxRotation ? angle - 360 : angle;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
            value = GetNonLinearKnobValue(knobPvr);
            ChangeValueText();
            valueStorage.SetValue(value, gameObject);
            audioController.SetKnobValue(transform.name, channel, value);
        }
        else
        {
            ChangeValueText();
        }


        // If Knob is current target object, check if the objects angle (value) 
        // is close to the target angle (value). If so, move the GO to the angle 
        // and call the CheckInteractionOrder() of Interaction Manager.
        // This way it's not neccessary to hit the target value exactly.
        if (im.GetCurrentInteraction().TargetObject == gameObject // GO is target GO
            && Mathf.Abs(GetNonLinearKnobAngle(im.GetCurrentInteraction().TargetValue) - transform.localEulerAngles.z) <= movementTolerance // Target Angle is in close range
            && im.GetCurrentInteraction().TargetValueMax == im.GetCurrentInteraction().TargetValueMin // Target is depending on an exact value not a range
            && Mathf.Abs(Input.GetAxis("Mouse Y")) <= 0.10f && Mathf.Abs(Input.mouseScrollDelta.y) <= 1f) // don't react on too forcefull input  
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, GetNonLinearKnobAngle(im.GetCurrentInteraction().TargetValue));
            value = GetNonLinearKnobValue(knobPvr);
            ChangeValueText();
            valueStorage.SetValue(value, gameObject);
            audioController.SetKnobValue(transform.name, channel, value);
            // Stop all running coroutines to prevent multiple coroutine calling
            StopAllCoroutines();
            StartCoroutine(WaitForMouseAndScrollThenCheckTargetValue()); // this will wait for input to finish, then check value
        }
    }

    private void ChangeValueText() 
    {
        switch (knobType)
        {
            case KnobType.MicGain:
            case KnobType.EqGain:
            case KnobType.DSEqControl:
                canvasValueText.text = value.ToString("F2") + " dB";
                break;
            case KnobType.TrebleFreq:
            case KnobType.HiMidFreq:
            case KnobType.LoMidFreq:
            case KnobType.BassFreq:
                if (value >= 1)
                    canvasValueText.text = value.ToString("F2") + " kHz";
                else
                    canvasValueText.text = (value * 1000).ToString("F2") + " Hz";
                break;
            case KnobType.EqWidth:
            case KnobType.MinusOnePlusOne:
                canvasValueText.text = value.ToString("F2");
                break;
            case KnobType.InfTo6DbControl:
            case KnobType.InfTo10DbControl:
            case KnobType.InfTo20DbControl:
                if (value == -80)
                    canvasValueText.text = "-" + "\u221E" + " dB";
                else
                    canvasValueText.text = value.ToString("F2") + " dB";
                break;
        }
    }

    float GetNonLinearKnobValue(PositionValueRelation[] pvr)
    {
        var angle = transform.localEulerAngles.z;
        angle = angle > maxRotation ? angle - 360 : angle;
        foreach (var relation in knobPvr)
        {
            if (angle >= relation.positions[0] && angle <= relation.positions[1])
            {
                return GetKnobValue(
                    relation.positions[0], relation.positions[1],
                    relation.values[0], relation.values[1]
                    );
            }
        }
        return 0f;
    }

    // Returns calculated knob value base on given min-max-scale
    // Get values between 0-100: min = 0, max = 100
    float GetKnobValue(float minRotation, float maxRotation, float scaleMin, float scaleMax)
    {
        var angle = transform.localEulerAngles.z;
        angle = angle > maxRotation ? angle - 360 : angle;
        if (minRotation != maxRotation)
            return (angle - minRotation) * (scaleMax - scaleMin) / (maxRotation - minRotation) + scaleMin;
        else
            return 0;
    }

    /* Returns gameObject angle corresponding to a value */
    float GetNonLinearKnobAngle(float correspondingValue)
    {
        foreach (var relation in knobPvr)
        {
            if (correspondingValue >= relation.values[0] && correspondingValue <= relation.values[1])
            {
                var knobAngle = GetKnobAngle(correspondingValue, relation.values[0], relation.values[1], relation.positions[0], relation.positions[1]);
                if (knobAngle < 0)
                    return 360 + knobAngle;
                else
                    return knobAngle;
            }
        }
        return 0f;
    }

    float GetKnobAngle(float value, float scaleMin, float scaleMax, float minRotation, float maxRotation)
    {
        if (scaleMin != scaleMax)
            return (value - scaleMin) * (maxRotation - minRotation) / (scaleMax - scaleMin) + minRotation;
        else
            return 0;
    }

  

    KnobType GetKnobType()
    {
        KnobType knobType;
        switch (transform.name)
        {
            case "KnobMicGain":
            case "KnobMicGainL":
            case "KnobMicGainR":
            case "KnobTalkMicGain":
                knobType = KnobType.MicGain;
                break;
            case "KnobTrebleGain":
            case "KnobHiMidGain":
            case "KnobLoMidGain":
            case "KnobBassGain":
                knobType = KnobType.EqGain;
                break;
            case "KnobTrebleFreq":
                knobType = KnobType.TrebleFreq;
                break;
            case "KnobHiMidFreq":
                knobType = KnobType.HiMidFreq;
                break;
            case "KnobLoMidFreq":
                knobType = KnobType.LoMidFreq;
                break;
            case "KnobBassFreq":
                knobType = KnobType.BassFreq;
                break;
            case "KnobHiMidWidth":
            case "KnobLoMidWidth":
                knobType = KnobType.EqWidth;
                break;
            case var name when new Regex(@"^(KnobMonitorControl).*").IsMatch(name):
                knobType = KnobType.InfTo6DbControl;
                break;
            case var name when new Regex(@"^(KnobAuxControl).*").IsMatch(name):
                knobType = KnobType.InfTo6DbControl;
                break;
            case var name when new Regex(@"^(KnobPanControl).*").IsMatch(name):
            case "KnobDSBalControl":
            case "KnobBalControlMaster":
                knobType = KnobType.MinusOnePlusOne;
                break;
            case "KnobDSTrebleControl":
            case "KnobDSHiMidControl":
            case "KnobDSLoMidControl":
            case "KnobDSBassControl":
                knobType = KnobType.DSEqControl;
                break;
            case var name when new Regex(@"^((KnobMatrix)\D+.*)|((KnobMonitorControl).*)").IsMatch(name):
                knobType = KnobType.InfTo6DbControl;
                break;
            case "Knob1kHzControl":
            case "KnobPlaybackToMasters":
            case "KnobLocalControl":
            case "KnobPhonesControl":
            case var name when new Regex(@"^(Knob)(Monitor|Matrix|Aux)\d{1}.*").IsMatch(name):
                knobType = KnobType.InfTo10DbControl;
                break;
            case "KnobStereoLineGain":
            case "KnobReturn1":
            case "KnobReturn2":
                knobType = KnobType.InfTo20DbControl;
                break;
            default:
                knobType = KnobType.MicGain;
                break;
        }
        return knobType;
    }

    IEnumerator WaitForMouseAndScrollThenCheckTargetValue()
    {
        while (Input.GetMouseButton(0))
        {
            yield return null;
        }
        while (Input.mouseScrollDelta.y > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        
        im.CheckInteractionOrder(gameObject);
    }
}