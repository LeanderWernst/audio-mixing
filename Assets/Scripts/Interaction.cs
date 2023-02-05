using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DemoAnimate : UnityEvent<float, float> { }

[System.Serializable]
public class DemoReset : UnityEvent { }

[System.Serializable]
public class DemoSetTarget : UnityEvent<float> { }

[System.Serializable]
public struct Interaction
{
    [Header("Training & Demo")]
    [Tooltip("If true, the interaction can be skipped with ENTER.")]
    public bool IsSkippable;
    [Tooltip("The target object to be clicked for this interaction.")]
    public GameObject TargetObject;
    [Tooltip("If this interaction has an exact target value, put it here.")]
    public float TargetValue;
    [Tooltip("If there is a range allowed to finish the step, set the minimum here. Min & Max must be the same, if interaction checks for exact value")]
    public float TargetValueMin;
    [Tooltip("If there is a range allowed to finish the step, set the maximum here. Min & Max must be the same, if interaction checks for exact value")]
    public float TargetValueMax;
    [TextArea(3, 10)]
    public string Headline;
    [TextArea(3, 10)]
    public string Instruction;
    public AudioClip InstrAudio;
    [TextArea(3, 10)]
    public string HelpMsg;
    [TextArea(3, 10)]
    public string HelpMsgBonus;
    [TextArea(3, 10)]
    public string ErrElement;
    [TextArea(3, 10)]
    public string ErrAboveMax;
    [TextArea(3, 10)]
    public string ErrBelowMin;
    //public bool HelpCounted { get; set; }

    [Header("Demo")]
    public bool UseInDemo;
    public CinemachineVirtualCamera vmCam;
    /*[Tooltip("'Fader', 'Knob', 'Button' or 'ChannelList'")]
    public String elementFamily;*/
    [Tooltip("An alternative instruction text, if original is too long for Demo Mode.")]
    [TextArea(3, 10)]
    public String altInstruction;
    [Tooltip("The string of the drum as defined in the AudioController.")]
    public String DrumToBePlayed;
    public float volumeBefore;
    public float volumeAfter;
    [Tooltip("If TargetObject is empty, put here multiple objects that need to be highlighted.")]
    public GameObject[] TargetGameObjects;
    public float[] TargetValues;
    [Tooltip("Duration in seconds for animating the object")]
    public float animationTime;
    public DemoAnimate Animate;
    public DemoReset Reset;
    public DemoSetTarget SetToTarget;

}
