using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBehaviour : MonoBehaviour
{

    [SerializeField] private Color hoverColor;
    [SerializeField] private TextMeshProUGUI tmpUGUI;
    private UnityEngine.UI.Button button;
    private Selectable selectable;
    private Color normalColor;
    private ColorBlock cb;

    // Start is called before the first frame update
    void Start()
    {
        //hoverColor = new Color(155f, 155f, 155f, 255f);
        button = gameObject.GetComponent<UnityEngine.UI.Button>();
        cb = button.colors;
        normalColor = cb.selectedColor;

        // prevent button from being triggered by LEERTASTE key
        selectable = GetComponent<Selectable>();
        selectable.navigation = new Navigation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hover()
    {
        cb.selectedColor = hoverColor;
        button.colors = cb;
        if (tmpUGUI != null) 
        {
            StopAllCoroutines();
            StartCoroutine(FadeTextToFullAlpha(.1f, tmpUGUI));
        }
    }

    public void NoHover()
    {
        var pbb = gameObject.GetComponent<PlayButtonBehaviour>();
        if (pbb != null && pbb.isActive)
        {
            cb.selectedColor = pbb.activeColor;
            cb.normalColor = pbb.activeColor;
        }
        else
        { 
            cb.selectedColor = normalColor;
            cb.normalColor = normalColor;
        }
        button.colors = cb;
        if (tmpUGUI != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeTextToZeroAlpha(.1f, tmpUGUI));
        }
    }


    IEnumerator FadeTextToFullAlpha(float timeInSeconds, TextMeshProUGUI tmpUGUI)
    {
        while (tmpUGUI.color.a < 1.0f)
        {
            tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b, tmpUGUI.color.a + (Time.deltaTime / timeInSeconds));
            yield return null;
        }
    }

    IEnumerator FadeTextToZeroAlpha(float timeInSeconds, TextMeshProUGUI tmpUGUI)
    {
        while (tmpUGUI.color.a > 0.0f)
        {
            tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b, tmpUGUI.color.a - (Time.deltaTime / timeInSeconds));
            yield return null;
        }
    }
}
