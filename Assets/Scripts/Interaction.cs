using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Interaction
{
    public bool IsSkippable;
    public GameObject TargetObject;
    public float TargetValue;
    public float TargetValueMin;
    public float TargetValueMax;
    [TextArea(3, 10)]
    public string Headline;
    [TextArea(3, 10)]
    public string Instruction;
    public AudioClip InstrAudio;
    [TextArea(3, 10)]
    public string HelpMsg;
    public AudioClip HelpAudio;
    [TextArea(3, 10)]
    public string HelpMsgBonus;
    public AudioClip HelpBonusAudio;
    [TextArea(3, 10)]
    public string ErrElement;
    public AudioClip ErrorAudioElem;
    [TextArea(3, 10)]
    public string ErrAboveMax;
    public AudioClip ErrAudioMax;
    [TextArea(3, 10)]
    public string ErrBelowMin;
    public AudioClip ErrAudioMin;
    public UnityEvent OnExecution;
    public bool HelpCounted { get; set; }
}
