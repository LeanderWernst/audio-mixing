using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource AudiSrcBass, AudiSrcHihat, AudiSrcSnare;
    public static AudioClip drum_bass, drum_hihat, drum_snare;

    // Start is called before the first frame update
    void Start()
    {
        drum_bass = Resources.Load<AudioClip>("drum_bass");
        drum_hihat = Resources.Load<AudioClip>("drum_hihat");
        drum_snare = Resources.Load<AudioClip>("drum_snare");
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetEqFrq(float sliderValue)
    {
        mixer.SetFloat("EqFrq", sliderValue);
    }
    public void SetEqQ(float sliderValue)
    {
        mixer.SetFloat("EqQ", sliderValue);
    }

    public void SetEqGain(float sliderValue)
    {
        mixer.SetFloat("EqGain", sliderValue);
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

