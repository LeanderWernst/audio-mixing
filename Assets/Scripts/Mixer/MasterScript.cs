using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    string channelName;

    public Button btnMon1;
    public Button btnMon2;
    public Button btnMatrix1;
    public Button btnMatrix2;
    public Button btnOverrideAux12;
    public Button btnOverrideGroup12;
    public Button btnOverrideAux34;
    public Button btnOverrideGroup34;
    public Button btnOverrideMon12;
    public Button btnOverrideMatrix12;
    public Button btnOverrideMastersLR;
    public Button btnStereoRetMute1;
    public Button btnStereoRetSolo1;
    public Button btnStereoRetMute2;
    public Button btnStereoRetSolo2;
    public Button btnStereoRetGroup12;
    public Button btnStereoRetMaster1;
    public Button btnStereoRetGroup34;
    public Button btnStereoRetMaster2;
    public Button btnMon1Pre;
    public Button btnMon1Mute;
    public Button btnMon1Solo;
    public Button btnMon2Pre;
    public Button btnMon2Mute;
    public Button btnMon2Solo;
    public Button btn1kHzGenOn;
    public Button btnMatrixSplitStereo;
    public Button btnMatrix1Mute;
    public Button btnMatrix1Solo;
    public Button btnMatrix2Mute;
    public Button btnMatrix2Solo;
    public Button btnTalkMicOn;
    public Button btnTalkMicAux;
    public Button btnTalkMicGroup;
    public Button btnTalkMicMatrix;
    public Button btnTalkMicMaster;
    public Button btnTalkMicMon1;
    public Button btnTalkMicMon2;
    public Button btnPlayBackToMastersMute;
    public Button btnPlayBackToMastersSolo;
    public Button btnAux1Mute;
    public Button btnAux1Solo;
    public Button btnAux1Pre;
    public Button btnGroup1Mono;
    public Button btnGroup1Stereo;
    public Button btnGroup1Mute;
    public Button btnGroup1Solo;
    public Button btnAux2Mute;
    public Button btnAux2Solo;
    public Button btnAux2Pre;
    public Button btnGroup2Mono;
    public Button btnGroup2Stereo;
    public Button btnGroup2Mute;
    public Button btnGroup2Solo;
    public Button btnAux3Mute;
    public Button btnAux3Solo;
    public Button btnAux3Pre;
    public Button btnGroup3Mono;
    public Button btnGroup3Stereo;
    public Button btnGroup3Mute;
    public Button btnGroup3Solo;
    public Button btnAux4Mute;
    public Button btnAux4Solo;
    public Button btnAux4Pre;
    public Button btnGroup4Mono;
    public Button btnGroup4Stereo;
    public Button btnGroup4Mute;
    public Button btnGroup4Solo;
    public Button btnLocalMonLocalMute;
    public Button btnLocalMonPhonesMute;
    public Button btnLocalMonPfl;
    public Button btnMastersAllStereoToMasters;
    public Button btnMastersStereoToMono;
    public Button btnMastersStereoMute;
    public Button btnMastersStereoSolo;
    public Button btnMastersMonoMute;
    public Button btnMastersMonoSolo;

    public Knob knobStereoReturn1;
    public Knob knobStereoReturn2;
    public Knob knobStereoReturn1Mon1;
    public Knob knobStereoReturn1Mon2;
    public Knob knobStereoReturn2Mon1;
    public Knob knobStereoReturn2Mon2;
    public Knob knobMatrixGroup1L;
    public Knob knobMatrixGroup2L;
    public Knob knobMatrixGroup3L;
    public Knob knobMatrixGroup4L;
    public Knob knobMatrixGroupMonoL;
    public Knob knobMatrixGroupStereoL;
    public Knob knobMatrix1;
    public Knob knobMatrixGroup1R;
    public Knob knobMatrixGroup2R;
    public Knob knobMatrixGroup3R;
    public Knob knobMatrixGroup4R;
    public Knob knobMatrixGroupMonoR;
    public Knob knobMatrixGroupStereoR;
    public Knob knobMatrix2;
    public Knob knob1kHzControl;
    public Knob knobTalkMicGain;
    public Knob knobPlaybackToMasters;
    public Knob knobAux1Gain;
    public Knob knobAux1Pan;
    public Knob knobAux2Gain;
    public Knob knobAux2Pan;
    public Knob knobAux3Gain;
    public Knob knobAux3Pan;
    public Knob knobAux4Gain;
    public Knob knobAux4Pan;
    public Knob knobLocalMonLocal;
    public Knob knobLocalMonPhones;
    public Knob knobMastersBal;

    public Fader faderGroup1;
    public Fader faderGroup2;
    public Fader faderGroup3;
    public Fader faderGroup4;
    public Fader faderMasterL;
    public Fader faderMasterR;

    private void Awake()
    {
        channelName = gameObject.name;

        btnMon1 = GameObject.Find(channelName + "/ButtonMonitor1").GetComponent<Button>();
        btnMon2 = GameObject.Find(channelName + "/ButtonMonitor2").GetComponent<Button>();
        btnMatrix1 = GameObject.Find(channelName + "/ButtonMatrix1").GetComponent<Button>();
        btnMatrix2 = GameObject.Find(channelName + "/ButtonMatrix2").GetComponent<Button>();
        btnOverrideAux12 = GameObject.Find(channelName + "/LEDFirewireOutSel/ButtonFirewireOutSelAux12").GetComponent<Button>();
        btnOverrideGroup12 = GameObject.Find(channelName + "/LEDFirewireOutSel/ButtonFirewireOutSelGroup12").GetComponent<Button>();
        btnOverrideAux34 = GameObject.Find(channelName + "/LEDFirewireOutSel/ButtonFirewireOutSelAux34").GetComponent<Button>();
        btnOverrideGroup34 = GameObject.Find(channelName + "/LEDFirewireOutSel/ButtonFirewireOutSelGroup34").GetComponent<Button>();
        btnOverrideMon12 = GameObject.Find(channelName + "/LEDFirewireOutSel/ButtonFirewireOutSelMon12").GetComponent<Button>();
        btnOverrideMatrix12 = GameObject.Find(channelName + "/LEDFirewireOutSel/ButtonFirewireOutSelMatr12").GetComponent<Button>();
        btnOverrideMastersLR = GameObject.Find(channelName + "/LEDFirewireOutSel/ButtonFirewireOutSelMasterLR").GetComponent<Button>();
        btnStereoRetMute1 = GameObject.Find(channelName + "/LEDStereoReturn1Mute/ButtonStereoReturn1Mute").GetComponent<Button>();
        btnStereoRetSolo1 = GameObject.Find(channelName + "/LEDStereoReturn1Solo/ButtonStereoReturn1Solo").GetComponent<Button>();
        btnStereoRetMute2 = GameObject.Find(channelName + "/LEDStereoReturn2Mute/ButtonStereoReturn2Mute").GetComponent<Button>();
        btnStereoRetSolo2 = GameObject.Find(channelName + "/LEDStereoReturn2Solo/ButtonStereoReturn2Solo").GetComponent<Button>();
        btnStereoRetGroup12 = GameObject.Find(channelName + "/ButtonStereoReturnG1-2").GetComponent<Button>();
        btnStereoRetMaster1 = GameObject.Find(channelName + "/ButtonStereoReturn1").GetComponent<Button>();
        btnStereoRetGroup34 = GameObject.Find(channelName + "/ButtonStereoReturnG3-4").GetComponent<Button>();
        btnStereoRetMaster2 = GameObject.Find(channelName + "/ButtonStereoReturn2").GetComponent<Button>();
        btnMon1Pre = GameObject.Find(channelName + "/ButtonPre1").GetComponent<Button>();
        btnMon1Mute = GameObject.Find(channelName + "/LEDMonMute/ButtonMon1Mute").GetComponent<Button>();
        btnMon1Solo = GameObject.Find(channelName + "/LEDMonSolo/ButtonMon1Solo").GetComponent<Button>();
        btnMon2Pre = GameObject.Find(channelName + "/ButtonPre2").GetComponent<Button>();
        btnMon2Mute = GameObject.Find(channelName + "/LEDMonMute/ButtonMon2Mute").GetComponent<Button>();
        btnMon2Solo = GameObject.Find(channelName + "/LEDMonSolo/ButtonMon2Solo").GetComponent<Button>();
        btn1kHzGenOn = GameObject.Find(channelName + "/Button1kHzOn").GetComponent<Button>();
        btnMatrixSplitStereo = GameObject.Find(channelName + "/ButtonMatrixSplitStereoMaster").GetComponent<Button>();
        btnMatrix1Mute = GameObject.Find(channelName + "/LEDMatrixMute/ButtonMatrix1Mute").GetComponent<Button>();
        btnMatrix1Solo = GameObject.Find(channelName + "/LEDMatrixSolo/ButtonMatrix1Solo").GetComponent<Button>();
        btnMatrix2Mute = GameObject.Find(channelName + "/LEDMatrixMute/ButtonMatrix2Mute").GetComponent<Button>();
        btnMatrix2Solo = GameObject.Find(channelName + "/LEDMatrixSolo/ButtonMatrix2Solo").GetComponent<Button>();
        btnTalkMicOn = GameObject.Find(channelName + "/LEDTalkMicOn/ButtonTalkMicOn").GetComponent<Button>();
        btnTalkMicAux = GameObject.Find(channelName + "/ButtonTalkMicAux").GetComponent<Button>();
        btnTalkMicGroup = GameObject.Find(channelName + "/ButtonTalkMicGroup").GetComponent<Button>();
        btnTalkMicMatrix = GameObject.Find(channelName + "/ButtonTalkMicMatrix").GetComponent<Button>();
        btnTalkMicMaster = GameObject.Find(channelName + "/ButtonTalkMicMaster").GetComponent<Button>();
        btnTalkMicMon1 = GameObject.Find(channelName + "/ButtonTalkMicMon1").GetComponent<Button>();
        btnTalkMicMon2 = GameObject.Find(channelName + "/ButtonTalkMicMon2").GetComponent<Button>();
        btnPlayBackToMastersMute = GameObject.Find(channelName + "/LEDPlaybackMute/ButtonPlaybackMute").GetComponent<Button>();
        btnPlayBackToMastersSolo = GameObject.Find(channelName + "/LEDPlaybackSolo/ButtonPlaybackSolo").GetComponent<Button>();
        btnAux1Mute = GameObject.Find(channelName + "/LEDAuxMute/Button").GetComponent<Button>();
        btnAux1Solo = GameObject.Find(channelName + "/LEDAuxSolo/Button").GetComponent<Button>();
        btnAux1Pre = GameObject.Find(channelName + "/ButtonAuxPre1").GetComponent<Button>();
        btnGroup1Mono = GameObject.Find(channelName + "/ButtonMonoAux1").GetComponent<Button>();
        btnGroup1Stereo = GameObject.Find(channelName + "ButtonStereoAux1").GetComponent<Button>();
        btnGroup1Mute = GameObject.Find(channelName + "/LEDMute/ButtonMuteAux1").GetComponent<Button>();
        btnGroup1Solo = GameObject.Find(channelName + "/LEDSolo/ButtonSoloAux1").GetComponent<Button>();
        btnAux2Mute = GameObject.Find(channelName + "/LEDAuxMute/Button").GetComponent<Button>();
        btnAux2Solo = GameObject.Find(channelName + "/LEDAuxSolo/Button").GetComponent<Button>();
        btnAux2Pre = GameObject.Find(channelName + "/ButtonAuxPre2").GetComponent<Button>();
        btnGroup2Mono = GameObject.Find(channelName + "ButtonMonoAux2").GetComponent<Button>();
        btnGroup2Stereo = GameObject.Find(channelName + "ButtonStereoAux2").GetComponent<Button>();
        btnGroup2Mute = GameObject.Find(channelName + "/LEDMute/ButtonMuteAux2").GetComponent<Button>();
        btnGroup2Solo = GameObject.Find(channelName + "/LEDSolo/ButtonSoloAux2").GetComponent<Button>();
        btnAux3Mute = GameObject.Find(channelName + "/LEDAuxMute/Button").GetComponent<Button>();
        btnAux3Solo = GameObject.Find(channelName + "/LEDAuxSolo/Button").GetComponent<Button>();
        btnAux3Pre = GameObject.Find(channelName + "/ButtonAuxPre3").GetComponent<Button>();
        btnGroup3Mono = GameObject.Find(channelName + "ButtonMonoAux3").GetComponent<Button>();
        btnGroup3Stereo = GameObject.Find(channelName + "ButtonStereoAux3").GetComponent<Button>();
        btnGroup3Mute = GameObject.Find(channelName + "/LEDMute/ButtonMuteAux3").GetComponent<Button>();
        btnGroup3Solo = GameObject.Find(channelName + "/LEDSolo/ButtonSoloAux3").GetComponent<Button>();
        btnAux4Mute = GameObject.Find(channelName + "/LEDAuxMute/Button").GetComponent<Button>();
        btnAux4Solo = GameObject.Find(channelName + "/LEDAuxSolo/Button").GetComponent<Button>();
        btnAux4Pre = GameObject.Find(channelName + "/ButtonAuxPre4").GetComponent<Button>();
        btnGroup4Mono = GameObject.Find(channelName + "ButtonMonoAux4").GetComponent<Button>();
        btnGroup4Stereo = GameObject.Find(channelName + "ButtonStereoAux4").GetComponent<Button>();
        btnGroup4Mute = GameObject.Find(channelName + "/LEDMute/ButtonMuteAux4").GetComponent<Button>();
        btnGroup4Solo = GameObject.Find(channelName + "/LEDSolo/ButtonSoloAux4").GetComponent<Button>();
        btnLocalMonLocalMute = GameObject.Find(channelName + "/LEDLocalMute/ButtonLocalMute").GetComponent<Button>();
        btnLocalMonPhonesMute = GameObject.Find(channelName + "/LEDPhonesMute/ButtonPhonesMute").GetComponent<Button>();
        btnLocalMonPfl = GameObject.Find(channelName + "/LEDPfl/ButtonPfl").GetComponent<Button>();
        btnMastersAllStereoToMasters = GameObject.Find(channelName + "/LEDAllStereoToMaster/ButtonAllStereoToMaster").GetComponent<Button>();
        btnMastersStereoToMono = GameObject.Find(channelName + "/ButtonMasterStereoMono").GetComponent<Button>();
        btnMastersStereoMute = GameObject.Find(channelName + "/LEDMute/ButtonMasterStereoMute").GetComponent<Button>();
        btnMastersStereoSolo = GameObject.Find(channelName + "/LEDSolo/ButtonMasterStereoSolo").GetComponent<Button>();
        btnMastersMonoMute = GameObject.Find(channelName + "/LEDMute/ButtonMasterMonoMute").GetComponent<Button>();
        btnMastersMonoSolo = GameObject.Find(channelName + "/LEDSolo/ButtonMasterMonoSolo").GetComponent<Button>();

        knobStereoReturn1 = GameObject.Find(channelName + "/KnobReturn1").GetComponent<Knob>();
        knobStereoReturn2 = GameObject.Find(channelName + "/KnobReturn2").GetComponent<Knob>();
        knobStereoReturn1Mon1 = GameObject.Find(channelName + "/KnobMonitorControl1Group1-2").GetComponent<Knob>();
        knobStereoReturn1Mon2 = GameObject.Find(channelName + "/KnobMonitorControl2Group1-2").GetComponent<Knob>();
        knobStereoReturn2Mon1 = GameObject.Find(channelName + "/KnobMonitorControl1Group3-4").GetComponent<Knob>();
        knobStereoReturn2Mon2 = GameObject.Find(channelName + "/KnobMonitorControl2Group3-4").GetComponent<Knob>();
        knobMatrixGroup1L = GameObject.Find(channelName + "/KnobMatrixGroup1-1").GetComponent<Knob>();
        knobMatrixGroup2L = GameObject.Find(channelName + "/KnobMatrixGroup2-1").GetComponent<Knob>();
        knobMatrixGroup3L = GameObject.Find(channelName + "/KnobMatrixGroup3-1").GetComponent<Knob>();
        knobMatrixGroup4L = GameObject.Find(channelName + "/KnobMatrixGroup4-1").GetComponent<Knob>();
        knobMatrixGroupMonoL = GameObject.Find(channelName + "/KnobMatrixMono1").GetComponent<Knob>();
        knobMatrixGroupStereoL = GameObject.Find(channelName + "/KnobMatrixStereo1").GetComponent<Knob>();
        knobMatrix1 = GameObject.Find(channelName + "/KnobMatrix1Control").GetComponent<Knob>();
        knobMatrixGroup1R = GameObject.Find(channelName + "/KnobMatrixGroup1-2").GetComponent<Knob>();
        knobMatrixGroup2R = GameObject.Find(channelName + "/KnobMatrixGroup2-2").GetComponent<Knob>();
        knobMatrixGroup3R = GameObject.Find(channelName + "/KnobMatrixGroup3-2").GetComponent<Knob>();
        knobMatrixGroup4R = GameObject.Find(channelName + "/KnobMatrixGroup4-2").GetComponent<Knob>();
        knobMatrixGroupMonoR = GameObject.Find(channelName + "/KnobMatrixMono2").GetComponent<Knob>();
        knobMatrixGroupStereoR = GameObject.Find(channelName + "/KnobMatrixStereo2").GetComponent<Knob>();
        knobMatrix2 = GameObject.Find(channelName + "/KnobMatrix2Control").GetComponent<Knob>();
        knob1kHzControl = GameObject.Find(channelName + "/Knob1kHzControl").GetComponent<Knob>();
        knobTalkMicGain = GameObject.Find(channelName + "/KnobTalkMicGain").GetComponent<Knob>();
        knobPlaybackToMasters = GameObject.Find(channelName + "/KnobPlaybackToMasters").GetComponent<Knob>();
        knobAux1Gain = GameObject.Find(channelName + "/KnobAux1").GetComponent<Knob>();
        knobAux1Pan = GameObject.Find(channelName + "/KnobPanControlAux1").GetComponent<Knob>();
        knobAux2Gain = GameObject.Find(channelName + "/KnobAux2").GetComponent<Knob>();
        knobAux2Pan = GameObject.Find(channelName + "/KnobPanControlAux2").GetComponent<Knob>();
        knobAux3Gain = GameObject.Find(channelName + "/KnobAux3").GetComponent<Knob>();
        knobAux3Pan = GameObject.Find(channelName + "/KnobPanControlAux3").GetComponent<Knob>();
        knobAux4Gain = GameObject.Find(channelName + "/KnobAux4").GetComponent<Knob>();
        knobAux4Pan = GameObject.Find(channelName + "/KnobPanControlAux4").GetComponent<Knob>();
        knobLocalMonLocal = GameObject.Find(channelName + "/KnobLocalControl").GetComponent<Knob>();
        knobLocalMonPhones = GameObject.Find(channelName + "/KnobPhonesControl").GetComponent<Knob>();
        knobMastersBal = GameObject.Find(channelName + "/KnobBalControlMaster").GetComponent<Knob>();

        faderGroup1 = GameObject.Find(channelName + "/Fader1").GetComponent<Fader>();
        faderGroup2 = GameObject.Find(channelName + "/Fader2").GetComponent<Fader>();
        faderGroup3 = GameObject.Find(channelName + "/Fader3").GetComponent<Fader>();
        faderGroup4 = GameObject.Find(channelName + "/Fader4").GetComponent<Fader>();
        faderMasterL = GameObject.Find(channelName + "/FaderMasterL").GetComponent<Fader>();
        faderMasterR = GameObject.Find(channelName + "/FaderMasterR").GetComponent<Fader>();
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
