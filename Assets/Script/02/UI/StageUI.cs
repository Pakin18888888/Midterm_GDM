using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    public static StageUI Instance;

    public TMP_Text text;

    void Awake()
    {
        Instance = this;
    }

    public void ShowStage(int stage, bool rest)
    {
        if (rest)
        {
            text.text = "REST STAGE";
        }
        else
        {
            text.text = "STAGE " + stage;
        }

        text.alpha = 0;

        LeanTween.cancel(text.gameObject);

        LeanTween.value(text.gameObject, 0, 1, 0.25f)
            .setOnUpdate((float v) =>
            {
                text.alpha = v;
            });

        LeanTween.delayedCall(1.2f, () =>
        {
            LeanTween.value(text.gameObject, 1, 0, 0.3f)
                .setOnUpdate((float v) =>
                {
                    text.alpha = v;
                });
        });
    }
}