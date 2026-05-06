using UnityEngine;
using System;

public class PlayerPolaritys : MonoBehaviour
{
    public static PlayerPolaritys Instance;

    public PolarityType currentPolarity = PolarityType.Positive;

    public Action<PolarityType> OnPolarityChanged;

    public SpriteRenderer sr;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ApplyVisual();
    }

    public void SwitchPolarity()
    {
        currentPolarity = currentPolarity == PolarityType.Positive
            ? PolarityType.Negative
            : PolarityType.Positive;

        ApplyVisual();
        OnPolarityChanged?.Invoke(currentPolarity);
    }

    void ApplyVisual()
    {
        if (currentPolarity == PolarityType.Positive)
            sr.color = Color.red;
        else
            sr.color = Color.blue;
    }
}