using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLevel : MonoBehaviour
{
    public float audioLevelLeft;
    public float audioLevelRight;
    public float max;
    public float min;
    public int channelCount;


    // Start is called before the first frame update
    void Start()
    {
        samples = new float[qSamples]; // TEST
    }

    // Update is called once per frame
    void Update()
    {
        GetVolume(); // TEST
    }

    public float GetLevelLeft()
    {
        return Mathf.Abs(audioLevelLeft);
    }
    public float GetLevelRight()
    {
        return Mathf.Abs(audioLevelRight);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        int i = 0; // 0 for left channel 1 for right channel
        audioLevelLeft = data[i];
        audioLevelRight = data[1];
        channelCount = channels;
        if (data[i] > max)
        {
            max = data[i];
        }
        if( data[i] < min)
        {
            min = data[i];
        }
    }


    // TEST AREA
    [SerializeField] AudioSource audioSource;
    int qSamples = 1024;  // array size
    float refValue = 0.1f; // RMS value for 0 dB
    float rmsValue;   // sound level - RMS
    float dbValue;    // sound level - dB
    private float[] samples; // audio samples

    void GetVolume()
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
        Debug.Log(dbValue + " dB");
    }

}


// werte zwischen 0.54 und - 0.54
// Nur ein Fund ohne Hintergrund: "array of floats ranging from [-1.0f;1.0f]"
// => https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnAudioFilterRead.html