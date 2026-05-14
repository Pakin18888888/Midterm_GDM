using UnityEngine;
using System;
using System.Collections;

public class PlayerPolaritys : MonoBehaviour
{
    public static PlayerPolaritys Instance;

    public PolarityType currentPolarity = PolarityType.Positive;

    public Action<PolarityType> OnPolarityChanged;

    public SpriteRenderer sr;
    ChunkData currentChunk;
    float laneSwitchCooldown = 0f;
    bool isSwitchingLane = false;
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

    void Update()
    {
        if (!GameManagers.Instance.usePolarity)
            return;

        laneSwitchCooldown -= Time.deltaTime;

        if (laneSwitchCooldown > 0f)
            return;

        // 🔥 ต้องเช็คก่อน
        if (isSwitchingLane)
            return;

        CheckLanePolarity();
    }

    void CheckLanePolarity()
    {
        if (movement == null)
            return;

        LanePolarity[] lanes =
            FindObjectsOfType<LanePolarity>();

        foreach (LanePolarity lane in lanes)
        {
            // เอาเฉพาะ lane ใกล้ player
            if (Mathf.Abs(
                lane.transform.position.x
                - transform.position.x
            ) > 3f)
            {
                continue;
            }

            // ===== player อยู่เลนบน =====

            if (movement.IsTopLane())
            {
                if (lane.transform.position.y < 0)
                    continue;
            }

            // ===== player อยู่เลนล่าง =====

            else
            {
                if (lane.transform.position.y > 0)
                    continue;
            }

            // ===== polarity ชน =====

            if (lane.lanePolarity
                == currentPolarity)
            {
                RejectFromLane(
                    lane.lanePolarity
                );
            }

            break;
        }
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
        // ด่านพัก
        if (!GameManagers.Instance.usePolarity)
        {
            sr.color = Color.white;
            return;
        }

        sr.color =
            currentPolarity == PolarityType.Positive
            ? Color.red
            : Color.blue;
    }

    public IEnumerator LaneSwitchRoutine()
    {
        isSwitchingLane = true;

        yield return new WaitForSeconds(0.35f);

        isSwitchingLane = false;
    }
    void RejectFromLane(PolarityType laneType)
    {
        laneSwitchCooldown = 0.8f;

        if (movement.IsTopLane())
        {
            movement.GoBottomLane();
        }
        else
        {
            movement.GoTopLane();
        }
    }
}