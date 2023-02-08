using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLEDMeter : MonoBehaviour
{
    public SwitchLED LED1;
    public SwitchLED LED2;
    public SwitchLED LED3;
    public SwitchLED LED4;

    public AudioLevel audioLevel;
    float timeElapsed;
    float interval;
    // Start is called before the first frame update
    void Start()
    {
        interval = 0.001f;
    }

    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        while (timeElapsed >= interval)
        {
            timeElapsed -= interval;
            if (audioLevel != null)
            SwitchLed();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void SwitchLed()
    {
        if (audioLevel.GetVolume() > 18) LED1.status = false;
        else LED1.status = true;

        if (audioLevel.GetVolume() > 12) LED2.status = false;
        else LED2.status = true;

        if (audioLevel.GetVolume() > 0) LED3.status = false;
        else LED3.status = true;

        if (audioLevel.GetVolume() > -18) LED4.status = false;
        else LED4.status = true;
    }

}
