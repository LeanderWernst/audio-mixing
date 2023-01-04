using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource AudiSrcBass, AudiSrcHihat, AudiSrcSnare;
    public static AudioClip drum_bass, drum_hihat, drum_snare;
    public MixerOverhead mixerObj;

    // Start is called before the first frame update
    void Start()
    {
        drum_bass = Resources.Load<AudioClip>("drum_bass");
        drum_hihat = Resources.Load<AudioClip>("drum_hihat");
        drum_snare = Resources.Load<AudioClip>("drum_snare");
        AudiSrcBass.volume = 0;
        AudiSrcHihat.volume = 0;
        AudiSrcSnare.volume = 0;
    }

    public void SetFaderVolume(string channel, float value)
    {
        switch (channel)
        {
            case "Channel1":
                mixer.SetFloat("BassdrumVolume", mixerObj.channel1.fader.value);
                break;
            case "Channel2":
                mixer.SetFloat("SnareVolume", value);
                break;
            case "Channel3":
                mixer.SetFloat("HihatVolume", value);
                break;
            default:
                // code block
                break;
        }

    }

    public void SetKnobValue(string knob, string channel, float value)
    {
        switch(knob)
        {
            case "KnobPanControl":
                switch (channel)
                {
                    case "Channel1":
                        Debug.Log("check");
                        AudiSrcBass.panStereo = value;
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
        Debug.Log(mixerObj.channel1.fader.value);
    }
    public void setBassdrumVolum(float volume)
    {
        Debug.Log(mixerObj.channel1.fader.value);
    }
    // functions to controll bassdrum eq
    public void SetBassLowEqGain(float sliderValue)
    {
        mixer.SetFloat("BassDrumEqLow", sliderValue);
    }
    public void SetBassMidEqGain(float sliderValue)
    {
        mixer.SetFloat("BassDrumEqMid", sliderValue);
    }
    public void SetBassHighEqGain(float sliderValue)
    {
        mixer.SetFloat("BassDrumEqHigh", sliderValue);
    }
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
    public void SetSnareLowEqGain(float sliderValue)
    {
        mixer.SetFloat("SnareEqLow", sliderValue);
    }
    public void SetSnareMidEqGain(float sliderValue)
    {
        mixer.SetFloat("SnareEqMid", sliderValue);
    }
    public void SetSnareHighEqGain(float sliderValue)
    {
        mixer.SetFloat("SnareEqHigh", sliderValue);
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
    // functions to controll hihat eq
    public void SetHiHatGain(float gain)
    {
        AudiSrcHihat.volume = gain;
    }
    public void SetHihatLowEqGain(float sliderValue)
    {
        mixer.SetFloat("HihatEqLow", sliderValue);
    }
    public void SetHihatMidEqGain(float sliderValue)
    {
        mixer.SetFloat("HihatEqMid", sliderValue);
    }
    public void SetHihatHighEqGain(float sliderValue)
    {
        mixer.SetFloat("HihatEqHigh", sliderValue);
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
                AudiSrcBass.clip = drum_bass;
                AudiSrcHihat.Stop();
                AudiSrcSnare.Stop();
                AudiSrcBass.Play();
                    break;
            case "drum_hihat":
                AudiSrcHihat.clip = drum_hihat;
                AudiSrcBass.Stop();
                AudiSrcSnare.Stop();
                AudiSrcHihat.Play();
                break;
            case "drum_snare":
                AudiSrcSnare.clip = drum_snare;
                AudiSrcHihat.Stop();
                AudiSrcBass.Stop();
                AudiSrcSnare.Play();
                break;
            case "all":
                AudiSrcSnare.clip = drum_snare;
                AudiSrcBass.clip = drum_bass;
                AudiSrcHihat.clip = drum_hihat;
                AudiSrcSnare.Play();
                AudiSrcBass.Play();
                AudiSrcHihat.Play();
                break;
            case "stop":
                AudiSrcSnare.clip = drum_snare;
                AudiSrcBass.clip = drum_bass;
                AudiSrcHihat.clip = drum_hihat;
                AudiSrcSnare.Stop();
                AudiSrcBass.Stop();
                AudiSrcHihat.Stop();
                break;
        }
    }
    public void SetChannelLevel(string channel, float level)
    {
        mixer.SetFloat(channel, level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

