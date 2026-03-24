using UnityEngine;

public class MagnetLane : MonoBehaviour
{
    [Header("Lane Polarity")]
    [SerializeField] private MagnetPolarity polarity = MagnetPolarity.Positive;

    [Header("Attach Point")]
    [SerializeField] private Transform attachPoint;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color positiveColor = Color.red;
    [SerializeField] private Color negativeColor = Color.blue;

    public MagnetPolarity Polarity => polarity;
    public Transform AttachPoint => attachPoint;

    private void Start()
    {
        ApplyVisual();
    }

    public bool CanAttach(MagnetPolarity playerPolarity)
    {
        return playerPolarity != polarity;
    }

    public void SetPolarity(MagnetPolarity newPolarity)
    {
        polarity = newPolarity;
        ApplyVisual();
    }

    public void TogglePolarity()
    {
        polarity = polarity == MagnetPolarity.Positive
            ? MagnetPolarity.Negative
            : MagnetPolarity.Positive;

        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (sr != null)
            sr.color = polarity == MagnetPolarity.Positive ? positiveColor : negativeColor;
    }
}