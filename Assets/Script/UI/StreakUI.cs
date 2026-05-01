using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StreakUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text streakText;

    public void UpdateUI(int streak, float timer, float maxTime)
    {
        fillImage.fillAmount = 1f - (timer / maxTime);

        if (streak > 1)
        {
            streakText.text = "x" + streak;
            streakText.gameObject.SetActive(true);
        }
        else
        {
            streakText.gameObject.SetActive(false);
        }
    }
}