using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBehaviour : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Button button;
    [SerializeField] private Color hoverColor;
    private Color normalColor;
    private ColorBlock cb;

    // Start is called before the first frame update
    void Start()
    {
        cb = button.colors;
        normalColor = cb.selectedColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hover()
    {
        cb.selectedColor = hoverColor;
        button.colors = cb;
    }

    public void NoHover()
    {
        cb.selectedColor = normalColor;
        button.colors = cb;
    }
}
