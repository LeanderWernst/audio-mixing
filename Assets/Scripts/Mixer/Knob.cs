using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
 
public class Knob : MonoBehaviour
{
	public bool isClicked = false;

	[SerializeField] private float minRotation = -105.0f;
	[SerializeField] private float maxRotation = 185.0f;
	[SerializeField] private float rotationSpeed = 250.0f;

	private float angle;
    private KnobType knobType;
    private PositionValueRelation[] knobPvr;
    private AudioController audioController;
    public string channel;
    public float value;
    private ValueStorage valueStorage;
    TextMeshProUGUI canvasValueText;

    private void Awake()
    {
        canvasValueText = GameObject.FindGameObjectWithTag("ValueText").GetComponent<TextMeshProUGUI>();
        valueStorage = gameObject.GetComponent<ValueStorage>();
    }
    private void Start()
    {
        angle = transform.localEulerAngles.z;
        angle = angle > maxRotation ? angle - 360 : angle;
        knobType = GetKnobType();
        knobPvr = KnobPvr.Relation(knobType);
        value = GetNonLinearFaderValue(knobPvr);
        audioController = GameObject.Find("PanelKeys").GetComponent<AudioController>();
        var parent = transform;
        while(!parent.CompareTag("Channel"))
        {
            parent = parent.parent;
        }
        channel = parent.name;
        TurnKnob(0); // Initial "move" to get initial value from position
    }

    private void Update()
    {
        if (isClicked && (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0)) 
        {
            TurnKnob(Input.mouseScrollDelta.y / 2);
        }
    }

    void OnMouseDrag()
	{
        TurnKnob(Input.GetAxis("Mouse Y"));
    }

    private void TurnKnob(float inputForce)
    {
        angle += inputForce * rotationSpeed * Time.deltaTime;
        angle = Mathf.Clamp(angle, minRotation, maxRotation);
        angle = angle > maxRotation ? angle - 360 : angle;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
        value = GetNonLinearFaderValue(knobPvr);
        ChangeValueText();
        valueStorage.SetValue(value, gameObject);
        audioController.SetKnobValue(transform.name, channel, value);
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

    float GetNonLinearFaderValue(PositionValueRelation[] pvr)
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
}