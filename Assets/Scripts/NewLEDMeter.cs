using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLEDMeter : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        /*
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            Debug.Log(g.name);
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        SwitchLed();
    }

    void SwitchLed()
 
    {
        if (audioLevel.GetVolume() > 18)  LED1.status = false;
        else LED1.status = true;

        if (audioLevel.GetVolume() > 12) LED2.status = false;
        else LED2.status = true;

        if (audioLevel.GetVolume() > 6) LED3.status = false;
        else LED3.status = true;

        if (audioLevel.GetVolume() > 0) LED4.status = false;
        else LED4.status = true;

        if (audioLevel.GetVolume() > -3) LED5.status = false; 
        else LED5.status = true;

        if (audioLevel.GetVolume() > -6) LED6.status = false; 
        else LED6.status = true;

        if (audioLevel.GetVolume() > -9) LED7.status = false;
        else LED7.status = true;

        if (audioLevel.GetVolume() > -12) LED8.status = false;
        else LED8.status = true;

        if (audioLevel.GetVolume() > -15) LED9.status = false;
        else LED9.status = true;

        if (audioLevel.GetVolume() > -18) LED10.status = false;
        else LED10.status = true;

        if (audioLevel.GetVolume() > -21) LED11.status = false;
        else LED11.status = true;

        if (audioLevel.GetVolume() > -24) LED12.status = false;
        else LED12.status = true;
    }
}
