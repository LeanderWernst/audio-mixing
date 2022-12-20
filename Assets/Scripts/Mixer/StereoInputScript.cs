using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StereoInputScript : MonoBehaviour
{
    string channelName;

    Button btnPower;
    Button btnPad;
    Button btnLeft;
    Button btn80Hz;
    Button btnInput;
    Button btnPrePostEq;
    Button btnChannelMasters;
    Button btnStereoLineSolo;
    Button btnMuteEq;
    Button btnPreEq;
    Button btnGroup1;
    Button btnGroup2;
    Button btnGroup3;
    Button btnGroup4;
    Button btnGroupMono;
    Button btnGroupStero;
    Button btnMonoSum;
    Button btnMute;
    Button btnSolo;

    Knob knobMicGainL;
    Knob knobMicGainR;
    Knob knobDSTrebleControl;
    Knob knobDSHiMidControl;
    Knob knobDSLoMidControl;
    Knob knobDSBassControl;
    Knob knobMon1;
    Knob knobMon2;
    Knob knobAux1;
    Knob knobAux2;
    Knob knobAux3;
    Knob knobAux4;
    Knob knobBal;

    Fader fader;

    private void Awake()
    {
        channelName = gameObject.name;

        btnPower = GameObject.Find(channelName + "/LEDPower/ButtonPower").GetComponent<Button>();
        btnPad = GameObject.Find(channelName + "/ButtonPad").GetComponent<Button>();
        btnLeft = GameObject.Find(channelName + "/ButtonLeft").GetComponent<Button>();
        btn80Hz = GameObject.Find(channelName + "/Button80Hz").GetComponent<Button>();
        btnInput = GameObject.Find(channelName + "/LEDInput/ButtonFirewireInput").GetComponent<Button>();
        btnPrePostEq = GameObject.Find(channelName + "/LEDFirewireOut/ButtonPrePostEq").GetComponent<Button>();
        btnChannelMasters = GameObject.Find(channelName + "/ButtonChannelMasters").GetComponent<Button>();
        btnStereoLineSolo = GameObject.Find(channelName + "/LEDStereoLineSolo/Button").GetComponent<Button>();
        btnMuteEq = GameObject.Find(channelName + "/LEDEq/ButtonEq").GetComponent<Button>();
        btnPreEq = GameObject.Find(channelName + "/ButtonPreEq").GetComponent<Button>();
        btnGroup1 = GameObject.Find(channelName + "/ButtonGroup1").GetComponent<Button>();
        btnGroup2 = GameObject.Find(channelName + "/ButtonGroup2").GetComponent<Button>();
        btnGroup3 = GameObject.Find(channelName + "/ButtonGroup3").GetComponent<Button>();
        btnGroup4 = GameObject.Find(channelName + "/ButtonGroup4").GetComponent<Button>();
        btnGroupMono = GameObject.Find(channelName + "/ButtonMono").GetComponent<Button>();
        btnGroupStero = GameObject.Find(channelName + "/ButtonStereo").GetComponent<Button>();
        btnMonoSum = GameObject.Find(channelName + "/ButtonMonoSum").GetComponent<Button>();
        btnMute = GameObject.Find(channelName + "/LEDMute/ButtonMute").GetComponent<Button>();
        btnSolo = GameObject.Find(channelName + "/LEDSolo/ButtonSolo").GetComponent<Button>();

        knobMicGainL = GameObject.Find(channelName + "/KnobMicGainLR/KnobMicGainL").GetComponent<Knob>();
        knobMicGainR = GameObject.Find(channelName + "/KnobMicGainLR/KnobMicGainR").GetComponent<Knob>();
        knobDSTrebleControl = GameObject.Find(channelName + "/KnobDSTrebleControl").GetComponent<Knob>();
        knobDSHiMidControl = GameObject.Find(channelName + "/KnobDSHiMidControl").GetComponent<Knob>();
        knobDSLoMidControl = GameObject.Find(channelName + "/KnobDSLoMidControl").GetComponent<Knob>();
        knobDSBassControl = GameObject.Find(channelName + "/KnobDSBassControl").GetComponent<Knob>();
        knobMon1 = GameObject.Find(channelName + "/KnobMonitorControl1").GetComponent<Knob>();
        knobMon2 = GameObject.Find(channelName + "/KnobMonitorControl2").GetComponent<Knob>();
        knobAux1 = GameObject.Find(channelName + "/KnobAuxControl1").GetComponent<Knob>();
        knobAux2 = GameObject.Find(channelName + "/KnobAuxControl2").GetComponent<Knob>();
        knobAux3 = GameObject.Find(channelName + "/KnobAuxControl3").GetComponent<Knob>();
        knobAux4 = GameObject.Find(channelName + "/KnobAuxControl4").GetComponent<Knob>();
        knobBal = GameObject.Find(channelName + "/KnobDSBalControl").GetComponent<Knob>();

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
