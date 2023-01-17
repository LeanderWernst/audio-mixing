using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioLevel : MonoBehaviour
{
    public float audioLevelLeft;
    public float audioLevelRight;
    private int counter;
    public AudioListener audioListern;
    public AudioMixer mixer;
    //public AudioOutputCapture capture;

    // Start is called before the first frame update
    void Start()
    {
        samples = new float[qSamples]; 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetLevelLeft()
    {
        return (audioLevelLeft);
    }
    public float GetLevelRight()
    {
        return (audioLevelRight);
    }


    [SerializeField] AudioSource audioSource;
    int qSamples = 1024;  // array size
    float refValue = 0.1f; // RMS value for 0 dB
    float rmsValue;   // sound level - RMS
    float dbValue;    // sound level - dB
    private float[] samples; // audio samples
    private float[] samplesLR; // audio samples LR

    public float GetVolume()
    {

        audioSource.GetOutputData(samples, 0); // fill array with samples
        float sum = 0;
        for (var i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
        dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
        return dbValue;
    }

    float rmsValueLeft;   // sound level - RMS
    float dbValueLeft;    // sound level - dB
    float rmsValueRight;   // sound level - RMS
    float dbValueRight;    // sound level - dB

    private void OnAudioFilterRead(float[] data, int channels)
    {

            samplesLR = data;

            List<float> samplesLeft = new List<float>();
            List<float> samplesRight = new List<float>();

            for (var x = 0; x < samplesLR.Length; x++)
            {
                if (x % 2 == 0)
                {
                    samplesLeft.Add(samplesLR[x]);
                }
                else
                {
                    samplesRight.Add(samplesLR[x]);
                }
            }

            float sumLeft = 0;
            for (var j = 0; j < qSamples; j++)
            {
                sumLeft += samplesLeft[j] * samplesLeft[j]; // sum squared samples
            }
            rmsValueLeft = Mathf.Sqrt(sumLeft / qSamples); // rms = square root of average
            dbValueLeft = 20 * Mathf.Log10(rmsValueLeft / refValue); // calculate dB
            if (dbValueLeft < -160) dbValueLeft = -160; // clamp it to -160dB min
            audioLevelLeft = dbValueLeft;

            float sumRight = 0;
            for (var j = 0; j < qSamples; j++)
            {
                sumRight += samplesRight[j] * samplesRight[j]; // sum squared samples
            }
            rmsValueRight = Mathf.Sqrt(sumRight / qSamples); // rms = square root of average
            dbValueRight = 20 * Mathf.Log10(rmsValueRight / refValue); // calculate dB
            if (dbValueRight < -160) dbValueRight = -160; // clamp it to -160dB min
            audioLevelRight = dbValueRight;
        

    }
}
