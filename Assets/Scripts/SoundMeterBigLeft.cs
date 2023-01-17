using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMeterBigLeft : MonoBehaviour
{
    public SwitchLED LED1;
    public SwitchLED LED2;
    public SwitchLED LED3;
    public SwitchLED LED4;
    public SwitchLED LED5;
    public SwitchLED LED6;
    public SwitchLED LED7;
    public SwitchLED LED8;
    public SwitchLED LED9;
    public SwitchLED LED10;
    public SwitchLED LED11;
    public SwitchLED LED12;

    public AudioLevel audioLevel;
    float timeElapsed;
    float interval;

    // Start is called before the first frame update
    void Start()
    {
        interval = 0.001f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        while (timeElapsed >= interval)
        {
            timeElapsed -= interval;
            SwitchLedLeft();
        }
    }
    void SwitchLedLeft()
    {
        float level = audioLevel.GetLevelLeft();
        if (level > 18) LED1.status = false;
        else LED1.status = true;

        if (level > 12) LED2.status = false;
        else LED2.status = true;

        if (level > 6) LED3.status = false;
        else LED3.status = true;

        if (level > 0) LED4.status = false;
        else LED4.status = true;

        if (level > -3) LED5.status = false;
        else LED5.status = true;

        if (level > -6) LED6.status = false;
        else LED6.status = true;

        if (level > -9) LED7.status = false;
        else LED7.status = true;

        if (level > -12) LED8.status = false;
        else LED8.status = true;

        if (level > -15) LED9.status = false;
        else LED9.status = true;

        if (level > -18) LED10.status = false;
        else LED10.status = true;

        if (level > -21) LED11.status = false;
        else LED11.status = true;

        if (level > -24) LED12.status = false;
        else LED12.status = true;
    }

}
