using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreTween : MonoBehaviour
{
    public TMP_Text text;
    public string prefix = ""; // 👈 เพิ่ม

    public void Play(int targetValue)
    {
        StartCoroutine(AnimateScore(targetValue));
    }

    IEnumerator AnimateScore(int target)
    {
        int current = 0;
        float duration = 0.8f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;

            float t = time / duration;
            current = Mathf.RoundToInt(Mathf.Lerp(0, target, t));

            text.text = prefix + current.ToString(); // 👈 ใช้ prefix

            yield return null;
        }

        text.text = prefix + target.ToString();

        // 💥 เด้ง
        transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.15f)
            .setEaseOutBack()
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                LeanTween.scale(gameObject, Vector3.one, 0.15f)
                    .setIgnoreTimeScale(true);
            });
    }
}