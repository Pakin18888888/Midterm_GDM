using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRowUI : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text nameText;
    public TMP_Text scoreText;
    public Image background;

    public void SetData(int rank, string name, int score)
    {
        rankText.text = rank.ToString();
        nameText.text = name;
        scoreText.text = score.ToString();

        // 🟡 Highlight YOU
        if (rank == 1)
        {
            background.color = new Color(1f, 0.8f, 0.2f);

            // LeanTween.scale(gameObject,
            //     Vector3.one * 1.03f,
            //     0.5f)
            //     .setLoopPingPong();
        }
        else if (rank == 2)
        {
            background.color =
                new Color(.8f, .8f, .8f);
}
        else if (rank == 3)
        {
            background.color =
                new Color(.8f, .6f, .4f);
        }
        else if (name ==  PlayerNameManager.Instance.GetName())
        {
            background.color = new Color(0.2f, 0.8f, 1f); // ฟ้า
            scoreText.color = Color.black;
        }
        else
        {
            if (rank % 2 == 0)
                background.color = new Color(0.2f, 0.2f, 0.2f);
            else
                background.color = new Color(0.3f, 0.3f, 0.3f);

            scoreText.color = Color.white;
        }
    }

    public void PlayFocusLoop()
    {
        // เด้งเบา ๆ ไปมา
        LeanTween.scale(gameObject, Vector3.one * 1.05f, 0.6f)
            .setEaseInOutSine()
            .setLoopPingPong();

        // กระพริบสี
        Color original = background.color;

        LeanTween.value(gameObject, 0f, 1f, 0.8f)
            .setLoopPingPong()
            .setOnUpdate((float t) =>
            {
                background.color = Color.Lerp(original, Color.green, t);
            });
    }

    public void PlayAnimation()
    {
        transform.localScale = Vector3.zero;

        LeanTween.scale(gameObject, Vector3.one, 0.3f)
            .setEaseOutBack();
    }

    public void HighlightYou()
    {
        background.color = new Color(0.2f, 0.8f, 1f); // ฟ้า

        LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.2f)
            .setEaseOutBack()
            .setLoopPingPong()
            .setIgnoreTimeScale(true);
    }

    public void Highlight()
    {
        var img = GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        {
            img.color = new Color(0.2f, 0.8f, 1f); // สีฟ้า
        }
    }
}