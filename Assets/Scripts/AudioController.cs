using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource AudiSrcBass, AudiSrcHihat, AudiSrcSnare;
    public static AudioClip drum_bass, drum_snare ,drum_hihat;

    // Start is called before the first frame update
    void Start()
    {
        drum_bass = Resources.Load<AudioClip>("drum_bass");
        drum_snare = Resources.Load<AudioClip>("drum_snare");
        drum_hihat = Resources.Load<AudioClip>("drum_hihat");
        AudiSrcBass.volume = 0;
        AudiSrcSnare.volume = 0;
        AudiSrcHihat.volume = 0;
    }
    private float ConvertValuesToNewScale(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        if (oldMin != oldMax)
            return (oldValue - oldMin) * (newMax - newMin) / (oldMax - oldMin) + newMin;
        else
            return 0;
    }
    public void SetFaderVolume(string channel, float value)
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
            default:
                // code block
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
            case "KnobLoMidGain":
                value = ConvertValuesToNewScale(value, -15, 15, 0.35f, 3);
                mixer.SetFloat(channel + knob, value);
                break;
            case "KnobBassGain":
                value = ConvertValuesToNewScale(value, -15, 15, 0.35f, 3);
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

