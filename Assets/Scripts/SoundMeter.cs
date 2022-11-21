using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMeter : MonoBehaviour
{

    public SwitchLED LED1;
    public SwitchLED LED2;
    public SwitchLED LED3;
    public SwitchLED LED4;
    public AudioLevel sound;
    float timeElapsed;
    float interval;

    // Start is called before the first frame update
    void Start()
    {
        interval = 0.025f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        while (timeElapsed >= interval)
        {
            timeElapsed -= interval;
            SwitchLed();
        }

    }
    void SwitchLed()
    {
        if (sound.GetLevel() > 0.4)
        {
            LED1.status = false;
        }
        else
        {
            LED1.status = true;
        }
        if (sound.GetLevel() > 0.0)
        {
            LED2.status = false;
        }
        else
        {
            LED2.status = true;
        }
        if (sound.GetLevel() > -0.2)
        {
            LED3.status = false;
        }
        else
        {
            LED3.status = true;
        }
        if (sound.GetLevel() > -0.4)
        {
            LED4.status = false;
        }
        else
        {
            LED4.status = true;
        }
    }

}
