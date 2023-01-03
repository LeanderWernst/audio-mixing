using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLEDMeter : MonoBehaviour
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
        interval = 0.001f;
    }

    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        while (timeElapsed >= interval)
        {
            timeElapsed -= interval;
            SwitchLed();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void SwitchLed()
    {
        if (sound.GetLevelRight() > 1) LED1.status = false;
        else LED1.status = true;

        if (sound.GetLevelRight() > 0.4) LED2.status = false;
        else LED2.status = true;

        if (sound.GetLevelRight() > 0.2) LED3.status = false;
        else LED3.status = true;

        if (sound.GetLevelRight() > 0.036) LED4.status = false;
        else LED4.status = true;
        /*
        if (sound.GetLevelRight() > 1) LED5.status = false;
        else LED5.status = true;

        if (sound.GetLevelRight() > 0.8) LED6.status = false;
        else LED6.status = true;

        if (sound.GetLevelRight() > 0.5) LED7.status = false;
        else LED7.status = true;

        if (sound.GetLevelRight() > 0.2) LED8.status = false;
        else LED8.status = true;

        if (sound.GetLevelRight() > 0.15) LED9.status = false;
        else LED9.status = true;

        if (sound.GetLevelRight() > 0.1) LED10.status = false;
        else LED10.status = true;

        if (sound.GetLevelRight() > 0.05) LED11.status = false;
        else LED11.status = true;

        if (sound.GetLevelRight() > 0.036) LED12.status = false;
        else LED12.status = true;*/
    }

}
