using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelScript : MonoBehaviour
{
    string channelName;

    public Button btnPower;
    public Button btnPad;
    public Button btnMic;
    public Button btn80Hz;
    public Button btnInput;
    public Button btnPrePostEq;
    public Button btnInsert;
    public Button btnMuteEq;
    public Button btnPreEq;
    public Button btnGroup1;
    public Button btnGroup2;
    public Button btnGroup3;
    public Button btnGroup4;
    public Button btnGroupMono;
    public Button btnGroupStero;
    public Button btnPanToGroups;
    public Button btnMute;
    public Button btnSolo;

    public Knob knobMicGain;
    public Knob knobTrebleGain;
    public Knob knobTrebleFreq;
    public Knob knobHiMidGain;
    public Knob knobHiMidFreq;
    public Knob knobLoMidGain;
    public Knob knobLoMidFreq;
    public Knob knobBassGain;
    public Knob knobBassFreq;
    public Knob knobHiMidWidth;
    public Knob knobLoMidWidth;
    public Knob knobMon1;
    public Knob knobMon2;
    public Knob knobAux1;
    public Knob knobAux2;
    public Knob knobAux3;
    public Knob knobAux4;
    public Knob knobPan;

    public Fader fader;

    private void Awake()
    {
        channelName = gameObject.name;

        btnPower = GameObject.Find(channelName+ "/LEDPower/ButtonPower").GetComponent<Button>();
        btnPad = GameObject.Find(channelName + "/ButtonPad").GetComponent<Button>();
        btnMic = GameObject.Find(channelName + "/ButtonMic").GetComponent<Button>();
        btn80Hz = GameObject.Find(channelName + "/Button80Hz").GetComponent<Button>();
        btnInput = GameObject.Find(channelName + "/LEDInput/ButtonFirewireInput").GetComponent<Button>();
        btnPrePostEq = GameObject.Find(channelName + "/ButtonPrePostEq").GetComponent<Button>();
        btnInsert = GameObject.Find(channelName + "/LEDInsert/ButtonInsert").GetComponent<Button>();
        btnMuteEq = GameObject.Find(channelName + "/LEDEq/ButtonEq").GetComponent<Button>();
        btnPreEq = GameObject.Find(channelName + "/ButtonPreEq").GetComponent<Button>();
        btnGroup1 = GameObject.Find(channelName + "/ButtonGroup1").GetComponent<Button>();
        btnGroup2 = GameObject.Find(channelName + "/ButtonGroup2").GetComponent<Button>();
        btnGroup3 = GameObject.Find(channelName + "/ButtonGroup3").GetComponent<Button>();
        btnGroup4 = GameObject.Find(channelName + "/ButtonGroup4").GetComponent<Button>();
        btnGroupMono = GameObject.Find(channelName + "/ButtonMono").GetComponent<Button>();
        btnGroupStero = GameObject.Find(channelName + "/ButtonStereo").GetComponent<Button>();
        btnPanToGroups = GameObject.Find(channelName + "/ButtonPanToGroups").GetComponent<Button>();
        btnMute = GameObject.Find(channelName + "/LEDMute/ButtonMute").GetComponent<Button>();
        btnSolo = GameObject.Find(channelName + "/LEDSolo/ButtonSolo").GetComponent<Button>();

        knobMicGain = GameObject.Find(channelName + "/KnobMicGain").GetComponent<Knob>();
        knobTrebleGain = GameObject.Find(channelName + "/KnobTrebleGainFreq/KnobTrebleGain").GetComponent<Knob>();
        knobTrebleFreq = GameObject.Find(channelName + "/KnobTrebleGainFreq/KnobTrebleFreq").GetComponent<Knob>();
        knobHiMidGain = GameObject.Find(channelName + "/KnobHiMidGainFreq/KnobHiMidGain").GetComponent<Knob>();
        knobHiMidFreq = GameObject.Find(channelName + "/KnobHiMidGainFreq/KnobHiMidFreq").GetComponent<Knob>();
        knobLoMidGain = GameObject.Find(channelName + "/KnobLoMidGainFreq/KnobLoMidGain").GetComponent<Knob>();
        knobLoMidFreq = GameObject.Find(channelName + "/KnobLoMidGainFreq/KnobLoMidFreq").GetComponent<Knob>();
        knobBassGain = GameObject.Find(channelName + "/KnobBassGainFreq/KnobBassGain").GetComponent<Knob>();
        knobBassFreq = GameObject.Find(channelName + "/KnobBassGainFreq/KnobBassFreq").GetComponent<Knob>();
        knobHiMidWidth = GameObject.Find(channelName + "/KnobHiMidWidth").GetComponent<Knob>();
        knobLoMidWidth = GameObject.Find(channelName + "/KnobLoMidWidth").GetComponent<Knob>();
        knobMon1 = GameObject.Find(channelName + "/KnobMonitorControl1").GetComponent<Knob>();
        knobMon2 = GameObject.Find(channelName + "/KnobMonitorControl2").GetComponent<Knob>();
        knobAux1 = GameObject.Find(channelName + "/KnobAuxControl1").GetComponent<Knob>();
        knobAux2 = GameObject.Find(channelName + "/KnobAuxControl2").GetComponent<Knob>();
        knobAux3 = GameObject.Find(channelName + "/KnobAuxControl3").GetComponent<Knob>();
        knobAux4 = GameObject.Find(channelName + "/KnobAuxControl4").GetComponent<Knob>();
        knobPan = GameObject.Find(channelName + "/KnobPanControl").GetComponent<Knob>();

        fader = GameObject.Find(channelName + "/Fader").GetComponent<Fader>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
