using UnityEngine;

public class MagnetLane : MonoBehaviour
{
    [Header("Lane Polarity")]
    [SerializeField] private MagnetPolarity polarity = MagnetPolarity.Positive;

    [Header("Attach Point")]
    [SerializeField] private Transform attachPoint;

    public MagnetPolarity Polarity => polarity;
    public Transform AttachPoint => attachPoint;

    public bool CanAttach(MagnetPolarity playerPolarity)
    {
        return playerPolarity != polarity;
    }
}