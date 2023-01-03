using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchLED : MonoBehaviour
{
    public bool status = true;
    public Renderer thisRend;
    public Color onColor;

    // Start is called before the first frame update
    void Start()
    {
        thisRend = GetComponent<Renderer>();
            
            }

    // Update is called once per frame
    void Update()
    {
        if (status == true)
        {
            thisRend.material.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            thisRend.material.SetColor("_EmissionColor", onColor );
        }
    }
}
