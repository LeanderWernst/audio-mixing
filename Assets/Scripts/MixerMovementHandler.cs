using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerMovementHandler : MonoBehaviour
{

    private GameObject[] faders;

    private void Awake()
    {
        FaderMovementHandler();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FaderMovementHandler()
    {
        faders = GameObject.FindGameObjectsWithTag("Fader");
        foreach (GameObject fader in faders) 
        {
            fader.AddComponent<MoveFader>();
        }
    }
}
