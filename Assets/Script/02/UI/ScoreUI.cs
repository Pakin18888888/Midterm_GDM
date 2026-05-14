using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;

    int lastScore;

    void Start()
    {
        UpdateScore();
    }

    void Update()
    {
        if (ScoreManager.Instance == null)
            return;

        if (lastScore != ScoreManager.Instance.score)
        {
            UpdateScore();
        }
    }

    void UpdateScore()
    {
        lastScore = ScoreManager.Instance.score;

        scoreText.text = lastScore.ToString("N0") + " SCORE";

        // 💥 เด้งเบา ๆ
        LeanTween.cancel(scoreText.gameObject);

        scoreText.rectTransform.localScale =
            Vector3.one;

        LeanTween.scale(
            scoreText.gameObject,
            Vector3.one * 1.05f,
            0.08f
        ).setEaseOutBack()
         .setOnComplete(() =>
         {
             LeanTween.scale(
                 scoreText.gameObject,
                 Vector3.one,
                 0.08f
             );
         });
    }
}