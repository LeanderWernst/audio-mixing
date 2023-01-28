using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueStorage : MonoBehaviour
{

    private float storedValue;
    private string[] allowedCallers = new string[] {"Knob", "Fader", "Button", "ChannelList"};
    public float GetValue()
    {
        return storedValue;
    }

    public void SetValue(float value, GameObject sender)
    {
        if (Array.Exists(allowedCallers, caller => caller == sender.tag))
        {
            this.storedValue = value;
        }
    }
}
