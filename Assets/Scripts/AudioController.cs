using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] private AudioSource AudioSrcBass, AudioSrcSnare, AudioSrcHihat;
    [SerializeField] private AudioClip bassdrum, snare, hihat, bassdrumLoop, snareLoop, hihatLoop;
    public AudioLevel audioLevel;
    public SoundMeterBigLeft leftMeter;
    public SoundMeterBigRight rightMeter;
    public NewLEDMeter monoMeter;
    [SerializeField] private PlayButtonBehaviour pbbBassdrum;
    [SerializeField] private PlayButtonBehaviour pbbSnare;
    [SerializeField] private PlayButtonBehaviour pbbHihat;
    [SerializeField] private PlayButtonBehaviour pbbAll;

    private float oldFader1Value;
    private float oldFader2Value;
    private float oldFader3Value;
    private float oldFaderStereoInput1;
    private float knobGainMax = 2f;
    private float knobGainMin = 0.5f;

    public AudioMixerGroup audioMixerGroupStereo;

    private InteractionManager im;
    private void Awake()
    {
        //drum_bass = Resources.Load<AudioClip>("drum_bass");
        //drum_snare = Resources.Load<AudioClip>("drum_snare");
        //drum_hihat = Resources.Load<AudioClip>("drum_hihat");
        im = GameObject.FindGameObjectWithTag("InteractionManager").GetComponent<InteractionManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioSrcBass.volume = 0;
        AudioSrcSnare.volume = 0;
        AudioSrcHihat.volume = 0;
        AudioSrcSnare.clip = snare;
        AudioSrcBass.clip = bassdrum;
        AudioSrcHihat.clip = hihat;

        AudioMixerGroup[] audioMixerGroups = mixer.FindMatchingGroups("Stereo1");

        if (audioMixerGroups.Length > 0)
        {
            audioMixerGroupStereo = audioMixerGroups[0];
            Debug.Log("Found AudioMixerGroup: " + audioMixerGroupStereo.name);
        }
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
                mixer.SetFloat("Channel1Volume", value);
                break;
            case "Channel2":
                mixer.SetFloat("Channel2Volume", value);
                break;
            case "Channel3":
                mixer.SetFloat("Channel3Volume", value);
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
        if (channel == "Channel1" || channel == "Channel2" || channel == "Channel3" )
        {
            switch (knob)
            {
                case "KnobMicGain":
                    value = ConvertValuesToNewScale(value, 10, 60, 0.05f, 1);
                    switch (channel)
                    {
                        case "Channel1":
                            AudioSrcBass.volume = value;
                            break;
                        case "Channel2":
                            AudioSrcSnare.volume = value;
                            break;
                        case "Channel3":
                            AudioSrcHihat.volume = value;
                            break;
                    }
                    break;
                case "KnobTrebleGain":
                    value = ConvertValuesToNewScale(value, -15, 15, knobGainMin, knobGainMax);
                    mixer.SetFloat(channel + knob, value);
                    break;
                case "KnobTrebleFreq":
                    value = ConvertValuesToNewScale(value, 2, 20, 2000, 20000);
                    mixer.SetFloat(channel + knob, value);
                    break;
                case "KnobHiMidGain":
                    value = ConvertValuesToNewScale(value, -15, 15, knobGainMin, knobGainMax);
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
                    value = ConvertValuesToNewScale(value, -15, 15, knobGainMin, knobGainMax);
                    mixer.SetFloat(channel + knob, value);
                    break;
                case "KnobLoMidFreq":
                    value = ConvertValuesToNewScale(value, 0.1f, 2, 100, 2000);
                    mixer.SetFloat(channel + knob, value);
                    break;
                case "KnobLoMidWidth":
                    value = ConvertValuesToNewScale(value, 0.1f, 2, 0.2f, 2);
                    mixer.SetFloat(channel + knob, value);
                    break;
                case "KnobBassGain":
                    value = ConvertValuesToNewScale(value, -15, 15, knobGainMin, knobGainMax);
                    mixer.SetFloat(channel + knob, value);
                    break;
                case "KnobBassFreq":
                    value = ConvertValuesToNewScale(value, 0.02f, 0.2f, 20, 200);
                    mixer.SetFloat(channel + knob, value);
                    break;
                case "KnobPanControl":
                    switch (channel)
                    {
                        case "Channel1":
                            AudioSrcBass.panStereo = value;
                            break;
                        case "Channel2":
                            AudioSrcSnare.panStereo = value;
                            break;
                        case "Channel3":
                            AudioSrcHihat.panStereo = value;
                            break;
                    }
                    break;

                default:
                    //mixer.SetFloat(channel + knob, value);
                    break;
            }
        }
}

    public void SetButtonOn(string button, string channel)
    {
        switch (button)
        {
            case "ButtonMute":
                switch (channel)
                {
                    case "Channel1":
                        mixer.GetFloat("Channel1Volume",out oldFader1Value);
                        mixer.SetFloat("Channel1Volume", -80);
                        break;
                    case "Channel2":
                        mixer.GetFloat("Channel2Volume", out oldFader2Value);
                        mixer.SetFloat("Channel2Volume", -80);
                        break;
                    case "Channel3":
                        mixer.GetFloat("Channel3Volume", out oldFader3Value);
                        mixer.SetFloat("Channel3Volume", -80);
                        break;
                    case "StereoInput1":
                        mixer.GetFloat("StereoInput1Volume", out oldFaderStereoInput1);
                        mixer.SetFloat("StereoInput1Volume", -80);
                        break;
                }
                break;
            case "ButtonSolo":
                switch (channel)
                {
                    case "Channel1":
                        audioLevel.SetAudioSource(AudioSrcBass);
                        leftMeter.soloMode = true;
                        rightMeter.soloMode = true;
                        monoMeter.soloMode = true;
                        break;
                    case "Channel2":
                        audioLevel.SetAudioSource(AudioSrcSnare);
                        leftMeter.soloMode = true;
                        rightMeter.soloMode = true;
                        monoMeter.soloMode = true;
                        break;
                    case "Channel3":
                        audioLevel.SetAudioSource(AudioSrcHihat);
                        leftMeter.soloMode = true;
                        rightMeter.soloMode = true;
                        monoMeter.soloMode = true;
                        break;
                }
                break;
            case "Button80Hz":
                switch (channel)
                {
                    case "Channel1":
                            mixer.SetFloat("Channel1CutOff", 400);
                        break;
                    case "Channel2":
                            mixer.SetFloat("Channel2CutOff", 400);
                        break;
                    case "Channel3":
                            mixer.SetFloat("Channel3CutOff", 400);
                        break;
                }
                break;
        }
  
    }
    public void SetButtonOff(string button, string channel)
    {
        switch (button)
        {
            case "ButtonMute":
                switch (channel)
                {
                    case "Channel1":
                        mixer.SetFloat("Channel1Volume", oldFader1Value);
                        break;
                    case "Channel2":
                        mixer.SetFloat("Channel2Volume", oldFader2Value);
                        break;
                    case "Channel3":
                        mixer.SetFloat("Channel3Volume", oldFader3Value);
                        break;
                    case "StereoInput1":
                        mixer.SetFloat("StereoInput1Volume", oldFaderStereoInput1);
                        break;
                }
                break;

            case "ButtonSolo":
                {
                    leftMeter.soloMode = false;
                    rightMeter.soloMode = false;
                    monoMeter.soloMode = false;
                }
                break;
            case "Button80Hz":
                switch (channel)
                {
                    case "Channel1":
                        mixer.SetFloat("Channel1CutOff", 0);
                        break;
                    case "Channel2":
                        mixer.SetFloat("Channel2CutOff", 0);
                        break;
                    case "Channel3":
                        mixer.SetFloat("Channel3CutOff", 0);
                        break;
                }
                break;
        }

    }


    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "drum_bass":
                AudioSrcBass.clip = bassdrum;
                pbbBassdrum.isActive = !pbbBassdrum.isActive;
                if (pbbBassdrum.isActive)
                {
                    pbbHihat.isActive = false;
                    pbbSnare.isActive = false;
                    pbbAll.isActive = false;
                    AudioSrcHihat.Stop();
                    AudioSrcSnare.Stop();
                    AudioSrcBass.Play();
                    im.SetSoundWasPlayed("drum_bass");
                }
                else 
                    AudioSrcBass.Stop();
                
                break;
            case "drum_hihat":
                AudioSrcHihat.clip = hihat;
                pbbHihat.isActive = !pbbHihat.isActive;
                if (pbbHihat.isActive)
                {
                    pbbBassdrum.isActive = false;
                    pbbSnare.isActive = false;
                    pbbAll.isActive = false;
                    AudioSrcBass.Stop();
                    AudioSrcSnare.Stop();
                    AudioSrcHihat.Play();
                    im.SetSoundWasPlayed("drum_hihat");
                }
                else
                    AudioSrcHihat.Stop();
                break;
            case "drum_snare":
                AudioSrcSnare.clip = snare;
                pbbSnare.isActive = !pbbSnare.isActive;
                if (pbbSnare.isActive)
                {
                    pbbBassdrum.isActive = false;
                    pbbHihat.isActive = false;
                    pbbAll.isActive = false;
                    AudioSrcHihat.Stop();
                    AudioSrcBass.Stop();
                    AudioSrcSnare.Play();
                    im.SetSoundWasPlayed("drum_snare");
                }
                else
                    AudioSrcSnare.Stop();
                break;
            case "all":
                pbbAll.isActive = !pbbAll.isActive;
                AudioSrcSnare.clip = snareLoop;
                AudioSrcBass.clip = bassdrumLoop;
                AudioSrcHihat.clip = hihatLoop;
                if (pbbAll.isActive)
                {
                    pbbBassdrum.isActive = false;
                    pbbHihat.isActive = false;
                    pbbSnare.isActive = false;
                    AudioSrcSnare.Play();
                    AudioSrcBass.Play();
                    AudioSrcHihat.Play();
                    im.SetSoundWasPlayed("all");
                }
                else
                {
                    AudioSrcSnare.Stop();
                    AudioSrcBass.Stop();
                    AudioSrcHihat.Stop();
                }
                break;
        }
        pbbBassdrum.ChangeButtonColor();
        pbbAll.ChangeButtonColor();
        pbbSnare.ChangeButtonColor();
        pbbHihat.ChangeButtonColor();
    }

    public void PlaySoundInDemo(string clip)
    {
        switch (clip)
        {
            case "drum_bass":
                AudioSrcBass.clip = bassdrum;
                AudioSrcBass.Play();
                break;
            case "drum_hihat":
                AudioSrcHihat.clip = hihat;
                AudioSrcHihat.Play();
                break;
            case "drum_snare":
                AudioSrcSnare.clip = snare;
                AudioSrcSnare.Play();
                break;
            case "all":
                AudioSrcSnare.clip = snareLoop;
                AudioSrcBass.clip = bassdrumLoop;
                AudioSrcHihat.clip = hihatLoop;
                AudioSrcSnare.Play();
                AudioSrcBass.Play();
                AudioSrcHihat.Play();
                break;
        }
    }

    public void StopAllSounds()
    {
        AudioSrcSnare.Stop();
        AudioSrcBass.Stop();
        AudioSrcHihat.Stop();
    }

    private void ListenToKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlaySound("drum_bass");
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            PlaySound("drum_snare");
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            PlaySound("drum_hihat");
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            PlaySound("all");
    }
    public void SetChannelLevel(string channel, float level)
    {
        mixer.SetFloat(channel, level);
    }

    public bool IsDrumActive(string drum)
    {
        switch (drum)
        {
            case "drum_bass":
                return pbbBassdrum.isActive;
            case "drum_snare":
                return pbbSnare.isActive;
            case "drum_hihat":
                return pbbHihat.isActive;
            case "all":
                return pbbAll.isActive;
            default:
                return false;
        }
    }

    public void SetChannelToVolume(float volume, string channel)
    {
        switch (channel)
        {
            case "Channel1":
                mixer.SetFloat("Channel1Volume", volume);
                break;
            case "Channel2":
                mixer.SetFloat("Channel2Volume", volume);
                break;
            case "Channel3":
                mixer.SetFloat("Channel3Volume", volume);
                break;
        }
    }

    float ch1;
    float ch2;
    float ch3;
    // Update is called once per frame
    void Update()
    {
        ListenToKeyboard();

        /*mixer.GetFloat("Channel1Volume", out ch1);
        mixer.GetFloat("Channel2Volume", out ch2);
        mixer.GetFloat("Channel3Volume", out ch3);
        Debug.Log("Vol1: [" + ch1 + "]  Vol2: [" + ch2 + "]  Vol3: [" + ch3 + "]");*/
    }
}

