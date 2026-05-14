using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash Instance;

    public Image flashImage;

    void Awake()
    {
        Instance = this;
    }

    public void Flash()
    {
        LeanTween.cancel(flashImage.rectTransform);

        Color c = flashImage.color;

        c.a = 0.45f;

        flashImage.color = c;

        LeanTween.value(
            flashImage.gameObject,
            0.45f,
            0f,
            0.25f
        ).setOnUpdate((float val) =>
        {
            Color color = flashImage.color;
            color.a = val;
            flashImage.color = color;
        });
    }
}