using UnityEngine;
using UnityEngine.UI;

public class StreakBarUI : MonoBehaviour
{
    public Image bar;
    public int maxStreak = 20; // ถึงค่านี้ = เต็ม

    void Update()
    {
        float value = Mathf.Clamp01((float)ScoreManager.Instance.streak / maxStreak);
        bar.fillAmount = value;
    }
}