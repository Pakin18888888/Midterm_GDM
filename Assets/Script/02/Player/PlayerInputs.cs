using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerMovements movement;
    private PlayerPolaritys polarity;

    void Awake()
    {
        movement = GetComponent<PlayerMovements>();
        polarity = GetComponent<PlayerPolaritys>();
    }

    // 🔫 ยิง
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            movement.StartShooting();
        }

        if (ctx.canceled)
        {
            movement.StopShooting();
        }
    }

    // 🦘 สลับเลน
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (movement.IsTopLane())
        {
            movement.GoBottomLane();
        }
        else
        {
            movement.GoTopLane();
        }
    }

    // ⚡ สลับขั้ว
    public void OnSwitchPolarity(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        polarity.SwitchPolarity();
    }
}