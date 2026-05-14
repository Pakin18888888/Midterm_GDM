using UnityEngine;
using TMPro;

public class StreakUI : MonoBehaviour
{
    public TMP_Text text;

    [Header("Animation")]
    public float pulseScale = 1.15f;

    float idleTime;

    void Start()
    {
        ScoreManager.Instance.OnStreakChanged += UpdateUI;

        UpdateUI(ScoreManager.Instance.streak);
    }

    void OnDestroy()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnStreakChanged -= UpdateUI;
    }

    void Update()
    {
        if (ScoreManager.Instance == null)
            return;

        // 🔥 สั่นเบา ๆ ตอน streak สูง
        if (ScoreManager.Instance.streak >= 15)
        {
            idleTime += Time.deltaTime;

            float rot =
                Mathf.Sin(idleTime * 4f) * 3f;

            text.rectTransform.rotation =
                Quaternion.Euler(0, 0, rot);
        }
        else
        {
            text.rectTransform.rotation =
                Quaternion.identity;
        }
    }

    void UpdateUI(int streak)
    {
        // ❌ ไม่มี streak = ซ่อน
        if (streak <= 0)
        {
            text.text = "";
            return;
        }

        // 📝 ข้อความ
        text.text = "STREAK " + streak;

        // 🎨 สี
        if (streak >= 15)
        {
            text.color = Color.red;
            text.fontSize = 80;
        }
        else if (streak >= 8)
        {
            text.color = Color.yellow;
            text.fontSize = 70;
        }
        else
        {
            text.color = Color.white;
            text.fontSize = 60;
        }

        // 💥 เด้ง
        LeanTween.cancel(text.gameObject);

        text.rectTransform.localScale = Vector3.zero;

        LeanTween.scale(
            text.gameObject,
            Vector3.one * pulseScale,
            0.15f
        ).setEaseOutBack()
         .setOnComplete(() =>
         {
             LeanTween.scale(
                 text.gameObject,
                 Vector3.one,
                 0.1f
             );
         });

        // ⚡ x3 feel
        if (streak >= 15)
        {
            LeanTween.rotateZ(
                text.gameObject,
                Random.Range(-10f, 10f),
                0.08f
            ).setLoopPingPong(2);
        }
    }
}