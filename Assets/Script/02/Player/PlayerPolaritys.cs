using UnityEngine;
using System;

public class PlayerPolaritys : MonoBehaviour
{
    public static PlayerPolaritys Instance;

    public PolarityType currentPolarity = PolarityType.Positive;

    public Action<PolarityType> OnPolarityChanged;

    public SpriteRenderer sr;

    PlayerMovements movement;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovements>();

        ApplyVisual();
    }

    public void SwitchPolarity()
    {
        currentPolarity =
            currentPolarity == PolarityType.Positive
            ? PolarityType.Negative
            : PolarityType.Positive;

        ApplyVisual();

        OnPolarityChanged?.Invoke(currentPolarity);
    }

    void ApplyVisual()
    {
        sr.color =
            currentPolarity == PolarityType.Positive
            ? Color.red
            : Color.blue;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        LanePolarity lane = other.GetComponent<LanePolarity>();

        if (lane == null) return;

        // ❌ ขั้วเหมือนกัน
        if (lane.lanePolarity == currentPolarity)
        {
            RejectFromLane(lane.lanePolarity);
        }
    }

    void RejectFromLane(PolarityType laneType)
    {
        // ถ้าโดนผลักจากเลนบน → กลับล่าง
        if (laneType == PolarityType.Positive)
        {
            movement.GoBottomLane();
        }
        else
        {
            movement.GoTopLane();
        }
    }
}