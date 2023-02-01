using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerMovementHandler : MonoBehaviour
{

    [SerializeField] private string faderTag = "Fader";
    [SerializeField] private string buttonTag = "Button";
    [SerializeField] private string knobTag = "Knob";
    private GameObject[] faders;
    private GameObject[] buttons;
    private GameObject[] knobs;
    private GameObject channelList;
    private void Awake()
    {
        faders = GameObject.FindGameObjectsWithTag(faderTag);
        buttons = GameObject.FindGameObjectsWithTag(buttonTag);
        knobs = GameObject.FindGameObjectsWithTag(knobTag);
        channelList = GameObject.FindGameObjectWithTag("ChannelList");

        MovementHandler();


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MovementHandler()
    {
        foreach (GameObject fader in faders) 
        {
            fader.AddComponent<ValueStorage>();
            //fader.AddComponent<Fader>();
        }
        foreach (GameObject button in buttons)
        {
            button.AddComponent<ValueStorage>();
            //button.AddComponent<Button>();
        }
        foreach (GameObject knob in knobs)
        {
            knob.AddComponent<ValueStorage>();
            //knob.AddComponent<Knob>();
        }
        channelList.AddComponent<ValueStorage>();
    }

    
}
