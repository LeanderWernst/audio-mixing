using System.Text.RegularExpressions;
using UnityEngine;
 
public class MoveKnob : MonoBehaviour
{
	public bool isClicked = false;

	[SerializeField] private float minRotation = -105.0f;
	[SerializeField] private float maxRotation = 185.0f;
	[SerializeField] private float rotationSpeed = 250.0f;

	private float angle;
    private KnobType knobType;
    private PositionValueRelation[] knobPvr;

    private void Start()
    {
        angle = transform.localEulerAngles.z;
        knobType = GetKnobType();
        knobPvr = KnobPvr.Relation(knobType);
    }

    private void Update()
    {
        if (isClicked) 
        {
            angle += Input.mouseScrollDelta.y * rotationSpeed*2 * Time.deltaTime;
            angle = Mathf.Clamp(angle, minRotation, maxRotation);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
        }
    }

    void OnMouseDrag()
	{
        angle += Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        angle = Mathf.Clamp(angle, minRotation, maxRotation);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
        Debug.Log(GetNonLinearFaderValue(knobPvr));
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
            case "KnobHidMidWidth":
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