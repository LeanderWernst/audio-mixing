using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class FadeText
{
    public static IEnumerator In(float timeInSeconds, TextMeshProUGUI text)
    {
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / timeInSeconds));
            yield return null;
        }
    }

    public static IEnumerator Out(float timeInSeconds, TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(5f);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / timeInSeconds));
            yield return null;
        }
    }
}
