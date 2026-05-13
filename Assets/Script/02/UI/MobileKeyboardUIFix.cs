using UnityEngine;

public class MobileKeyboardFix : MonoBehaviour
{
    public RectTransform panel;

    Vector2 originalPos;

    void Start()
    {
        originalPos =
            panel.anchoredPosition;
    }

    void Update()
    {
        if (TouchScreenKeyboard.visible)
        {
            panel.anchoredPosition =
                Vector2.Lerp(
                    panel.anchoredPosition,
                    new Vector2(
                        originalPos.x,
                        350
                    ),
                    Time.deltaTime * 10
                );
        }
        else
        {
            panel.anchoredPosition =
                Vector2.Lerp(
                    panel.anchoredPosition,
                    originalPos,
                    Time.deltaTime * 10
                );
        }
    }
}