using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StreakBarUI : MonoBehaviour
{
    public Image bar;
    public int maxStreak = 20;

    [Header("Colors")]
    public Color lowColor = Color.green;
    public Color midColor = Color.yellow;
    public Color highColor = Color.red;

    Coroutine fillRoutine;

    void Start()
    {
        ScoreManager.Instance.OnStreakChanged += UpdateBar;
        UpdateBar(ScoreManager.Instance.streak); // sync ค่าเริ่มต้น
    }

    void OnDestroy()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnStreakChanged -= UpdateBar;
    }

    void UpdateBar(int streak)
    {
        float target = Mathf.Clamp01((float)streak / maxStreak);

        // ⚡ Smooth Fill
        if (fillRoutine != null) StopCoroutine(fillRoutine);
        fillRoutine = StartCoroutine(SmoothFill(target));

        // 🟡 เปลี่ยนสีตาม streak
        if (streak > 15)
            bar.color = highColor;
        else if (streak > 7)
            bar.color = midColor;
        else
            bar.color = lowColor;

        // 💥 เด้งตอนเพิ่ม (เฉพาะตอนเพิ่ม ไม่ใช่ตอนรีเซ็ต)
        if (streak > 0)
        {
            LeanTween.cancel(bar.gameObject);

            bar.transform.localScale = Vector3.one;
            LeanTween.scale(bar.gameObject, Vector3.one * 1.1f, 0.1f)
                .setEaseOutBack()
                .setOnComplete(() =>
                {
                    LeanTween.scale(bar.gameObject, Vector3.one, 0.1f);
                });
        }
    }

    IEnumerator SmoothFill(float target)
    {
        float start = bar.fillAmount;
        float time = 0f;
        float duration = 0.25f;

        while (time < duration)
        {
            time += Time.deltaTime;
            bar.fillAmount = Mathf.Lerp(start, target, time / duration);
            yield return null;
        }

        bar.fillAmount = target;
    }
}