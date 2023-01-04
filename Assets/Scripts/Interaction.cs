using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Interaction
{
    public GameObject go;
    public string headline;
    public string instruction;
    public string helpMsg;
    public string errorMsg;
    UnityEvent OnExecution;
}
