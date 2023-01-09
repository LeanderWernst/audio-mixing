using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource AudiSrcBass, AudiSrcHihat, AudiSrcSnare;
    public static AudioClip drum_bass, drum_snare ,drum_hihat;
    [SerializeField] private PlayButtonBehaviour pbbBassdrum;
    [SerializeField] private PlayButtonBehaviour pbbSnare;
    [SerializeField] private PlayButtonBehaviour pbbHihat;
    [SerializeField] private PlayButtonBehaviour pbbAll;

    private void Awake()
    {
        drum_bass = Resources.Load<AudioClip>("drum_bass");
        drum_snare = Resources.Load<AudioClip>("drum_snare");
        drum_hihat = Resources.Load<AudioClip>("drum_hihat");
    }

    // Start is called before the first frame update
    void Start()
    {
        AudiSrcBass.volume = 0;
        AudiSrcSnare.volume = 0;
        AudiSrcHihat.volume = 0;
        AudiSrcSnare.clip = drum_snare;
        AudiSrcBass.clip = drum_bass;
        AudiSrcHihat.clip = drum_hihat;
    }
    private float ConvertValuesToNewScale(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        if (oldMin != oldMax)
            return (oldValue - oldMin) * (newMax - newMin) / (oldMax - oldMin) + newMin;
        else
            return 0;
    }
    public void SetFaderVolume(string fader , string channel, float value)
    {
        switch (channel)
        {
            case "Channel1":
                mixer.SetFloat("BassdrumVolume", value);
                break;
            case "Channel2":
                mixer.SetFloat("SnareVolume", value);
                break;
            case "Channel3":
                mixer.SetFloat("HihatVolume", value);
                break;
            case "Master":
                switch (fader)
                {
                    case "FaderMasterL":
                        mixer.SetFloat("FaderMasterL", value);
                        break;
                }
                break;
            case "StereoInput1":
                mixer.SetFloat("StereoInput1Volume", value);
                break;
        }
    }
    public void SetKnobValue(string knob, string channel, float value)
    {
        switch(knob)
        {
            case "KnobMicGain":
                value = ConvertValuesToNewScale(value, 10, 60, 0.05f, 1);
                switch (channel)
                {
                    case "Channel1":
                        AudiSrcBass.volume = value;
                        break;
                    case "Channel2":
                        AudiSrcSnare.volume = value;
                        break;
                    case "Channel3":
                        AudiSrcHihat.volume = value;
                        break;
                }
                break;
            case "KnobTrebleGain":
                value = ConvertValuesToNewScale(value, -15, 15, 0.35f, 3);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobTrebleFreq":
                value = ConvertValuesToNewScale(value,2,20,2000,20000);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobHiMidGain":
                value = ConvertValuesToNewScale(value, -15, 15, 0.35f, 3);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobHiMidFreq":
                value = ConvertValuesToNewScale(value, 0.4f, 8, 400, 8000);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobHiMidWidth":
                value = ConvertValuesToNewScale(value, 0.1f, 2, 0.2f, 2);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobLoMidGain":
                value = ConvertValuesToNewScale(value, -15, 15, 0.35f, 3);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobLoMidFreq":
                value = ConvertValuesToNewScale(value, 0.1f,2, 100, 2000);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobLoMidWidth":
                value = ConvertValuesToNewScale(value, 0.1f, 2, 0.2f, 2);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobBassGain":
                value = ConvertValuesToNewScale(value, -15, 15, 0.35f, 3);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobBassFreq":
                value = ConvertValuesToNewScale(value, 0.02f,0.2f, 20, 200);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobPanControl":
                switch (channel)
                {
                    case "Channel1":
                        AudiSrcBass.panStereo = value;
                        break;
                    case "Channel2":
                        AudiSrcSnare.panStereo = value;
                        break;
                    case "Channel3":
                        AudiSrcHihat.panStereo = value;
                        break;
                }
                break;

            default:
                mixer.SetFloat(channel + knob, value);
                break;
    }
}
    public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("MasterVol", sliderValue);
    }
    public void SetBassdrumGain(float gain){
        AudiSrcBass.volume = gain;
    }
    public void SetBassdrumVolum(float volume)
    {
    }
    // functions to controll bassdrum eq

    bool bassCutOffStatus = false;
    public void SetBassCutOff()
    {
        bassCutOffStatus = !bassCutOffStatus;
        if (bassCutOffStatus == true)
            mixer.SetFloat("BassCutOff", 400);
        else
        {
            mixer.SetFloat("BassCutOff", 0);
        }
    }

    // functions to controll snaredrum eq
    public void SetSnaredrumGain(float gain)
    {
        AudiSrcSnare.volume = gain;
    }
    
    bool snareCutOffStatus = false;
    public void SetSnareCutOff()
    {
        snareCutOffStatus = !snareCutOffStatus;
        if (snareCutOffStatus == true)
            mixer.SetFloat("SnareCutOff", 200);
        else
        {
            mixer.SetFloat("SnareCutOff", 0);
        }
    }
    public void SetHiHatGain(float gain)
    {
        AudiSrcHihat.volume = gain;
    }
    bool hihatCutOffStatus = false;
    public void SetHihatCutOff()
    {
        hihatCutOffStatus = !hihatCutOffStatus;
        if (hihatCutOffStatus == true)
            mixer.SetFloat("HihatCutOff", 200);
        else
        {
            mixer.SetFloat("HihatCutOff", 0);
        }
    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "drum_bass":
                pbbBassdrum.isActive = !pbbBassdrum.isActive;
                if (pbbBassdrum.isActive)
                {
                    pbbHihat.isActive = false;
                    pbbSnare.isActive = false;
                    pbbAll.isActive = false;
                    AudiSrcHihat.Stop();
                    AudiSrcSnare.Stop();
                    AudiSrcBass.Play();
                }
                else 
                    AudiSrcBass.Stop();
                
                break;
            case "drum_hihat":
                pbbHihat.isActive = !pbbHihat.isActive;
                if (pbbHihat.isActive)
                {
                    pbbBassdrum.isActive = false;
                    pbbSnare.isActive = false;
                    pbbAll.isActive = false;
                    AudiSrcBass.Stop();
                    AudiSrcSnare.Stop();
                    AudiSrcHihat.Play();
                }
                else
                    AudiSrcHihat.Stop();
                break;
            case "drum_snare":
                pbbSnare.isActive = !pbbSnare.isActive;
                if (pbbSnare.isActive)
                {
                    pbbBassdrum.isActive = false;
                    pbbHihat.isActive = false;
                    pbbAll.isActive = false;
                    AudiSrcHihat.Stop();
                    AudiSrcBass.Stop();
                    AudiSrcSnare.Play();
                }
                else
                    AudiSrcSnare.Stop();
                break;
            case "all":
                pbbAll.isActive = !pbbAll.isActive;
                if (pbbAll.isActive)
                {
                    pbbBassdrum.isActive = false;
                    pbbHihat.isActive = false;
                    pbbSnare.isActive = false;
                    AudiSrcSnare.Play();
                    AudiSrcBass.Play();
                    AudiSrcHihat.Play();
                }
                else
                {
                    AudiSrcSnare.Stop();
                    AudiSrcBass.Stop();
                    AudiSrcHihat.Stop();
                }
                break;
        }
        pbbBassdrum.ChangeButtonColor();
        pbbAll.ChangeButtonColor();
        pbbSnare.ChangeButtonColor();
        pbbHihat.ChangeButtonColor();
    }
    private void ListenToKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.B))
            PlaySound("drum_bass");
        else if (Input.GetKeyDown(KeyCode.S))
            PlaySound("drum_snare");
        else if (Input.GetKeyDown(KeyCode.H))
            PlaySound("drum_hihat");
        else if (Input.GetKeyDown(KeyCode.Space))
            PlaySound("all");
    }
    public void SetChannelLevel(string channel, float level)
    {
        mixer.SetFloat(channel, level);
    }

    // Update is called once per frame
    void Update()
    {
        ListenToKeyboard();
    }
}

