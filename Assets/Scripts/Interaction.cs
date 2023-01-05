using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Interaction
{
    public GameObject TargetObject;
    public float TargetValue;
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
    public string ErrorMsg;
    public UnityEvent OnExecution;
    public bool HelpCounted { get; set; }
}
