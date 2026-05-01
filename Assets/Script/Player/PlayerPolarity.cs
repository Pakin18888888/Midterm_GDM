using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerPolarity : MonoBehaviour
{
    [SerializeField] private MagnetPolarity currentPolarity = MagnetPolarity.Positive;
    [SerializeField] private SpriteRenderer[] indicators;
    [SerializeField] private Light2D[] lights;
    [SerializeField] private Color positiveColor = Color.red;
    [SerializeField] private Color negativeColor = Color.blue;

    public MagnetPolarity CurrentPolarity => currentPolarity;

    private void Start()
    {
        ApplyVisual();
    }

    public void Toggle()
    {
        currentPolarity = currentPolarity == MagnetPolarity.Positive
            ? MagnetPolarity.Negative
            : MagnetPolarity.Positive;

        ApplyVisual();
    }

    public void SetPolarity(MagnetPolarity polarity)
    {
        currentPolarity = polarity;
        ApplyVisual();
    }

    private void ApplyVisual()
    {
        Color c = currentPolarity == MagnetPolarity.Positive ? positiveColor : negativeColor;

        if (indicators != null)
        {
            for (int i = 0; i < indicators.Length; i++)
                if (indicators[i] != null) indicators[i].color = c;
        }

        if (lights != null)
        {
            for (int i = 0; i < lights.Length; i++)
                if (lights[i] != null) lights[i].color = c;
        }
    }
}