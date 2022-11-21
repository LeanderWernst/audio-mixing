using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLevel : MonoBehaviour
{
    public float audioLevel;
    public float max;
    public float min;
    public int channelCount;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetLevel()
    {
        if (-0.1f < audioLevel && audioLevel < 0.1f ) return -0.54f; else return audioLevel;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        int i = 0; // 0 for left channel 1 for right channel
        audioLevel = data[i];
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
}


// werte zwischen 0.54 und - 0.54