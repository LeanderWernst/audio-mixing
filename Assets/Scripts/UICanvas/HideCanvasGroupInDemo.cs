using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCanvasGroupInDemo : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
    }

}
