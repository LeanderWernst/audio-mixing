using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonBehaviour : MonoBehaviour
{
    [SerializeField] public Color activeColor;
    private Color normalColor;
    private ColorBlock cb;
    private UnityEngine.UI.Button button;

    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
        cb = button.colors;
        normalColor = cb.selectedColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeButtonColor()
    {
        if (isActive)
        {
            cb.selectedColor = activeColor;
            cb.normalColor = activeColor;
            button.colors = cb;
        }
        else 
        {
            cb.selectedColor = normalColor;
            cb.normalColor = normalColor;
            button.colors = cb;
        }
    }
}
