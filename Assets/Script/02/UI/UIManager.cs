using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image fadeImage;

    void Awake()
    {
        Instance = this;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeRoutine(0f, 1f, 0.8f));
    }

    IEnumerator FadeRoutine(float start, float end, float duration)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(start, end, t / duration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = end;
        fadeImage.color = c;
    }
}