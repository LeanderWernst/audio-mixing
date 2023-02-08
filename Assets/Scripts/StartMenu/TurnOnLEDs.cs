using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLEDs : MonoBehaviour
{
    private void Awake()
    {
        TurnAllOn();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Turns on all LED materials of all childs of this gameobject
    void TurnAllOn()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend != null)
            {
                Material mat = rend.material;
                if (mat.name.Contains("LED"))
                {
                    mat.EnableKeyword("_EMISSION");
                }
            }
        }
    }

}
