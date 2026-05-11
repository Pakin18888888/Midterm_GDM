using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLogo : MonoBehaviour
{
    public CanvasGroup logoGroup;

    public float fadeDuration = 1f;
    public float waitTime = 1f;

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // Fade In
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            logoGroup.alpha =
                Mathf.Lerp(0, 1, t / fadeDuration);

            yield return null;
        }

        // รอ
        yield return new WaitForSeconds(waitTime);

        // Fade Out
        t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            logoGroup.alpha =
                Mathf.Lerp(1, 0, t / fadeDuration);

            yield return null;
        }

        // ไปซีนต่อไป
        SceneManager.LoadScene("START");
    }
}