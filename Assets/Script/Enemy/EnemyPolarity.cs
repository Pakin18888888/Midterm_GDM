using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyPolarity : MonoBehaviour
{
    [Header("Polarity")]
    [SerializeField] private MagnetPolarity polarity = MagnetPolarity.Positive;

    [Header("Visual")]
    [SerializeField] private Color positiveColor = Color.red;
    [SerializeField] private Color negativeColor = Color.blue;
    [SerializeField] private ParticleSystem polarityVFX;

    private void Start()
    {
        ApplyVisual();
    }

    public MagnetPolarity GetPolarity()
    {
        return polarity;
    }

    public void SetPolarity(MagnetPolarity newPolarity)
    {
        polarity = newPolarity;
        ApplyVisual();
    }

    public bool IsSamePolarity(MagnetPolarity other)
    {
        return polarity == other;
    }

    private void ApplyVisual()
{
    Color c = polarity == MagnetPolarity.Positive ? positiveColor : negativeColor;

    // ⭐ เปลี่ยน Particle
    if (polarityVFX != null)
    {
        var main = polarityVFX.main;
        main.startColor = c;
    }
}
}