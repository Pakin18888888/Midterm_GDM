using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerLaneSwitcher laneSwitcher;
    [SerializeField] private PlayerPolarity polarity;
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerAnimationDriver animationDriver;

    public bool IsOnTopLane => laneSwitcher != null && laneSwitcher.IsOnTopLane;
    public MagnetPolarity CurrentPolarity => polarity != null ? polarity.CurrentPolarity : MagnetPolarity.Positive;

    private void Reset()
    {
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (laneSwitcher == null) laneSwitcher = GetComponent<PlayerLaneSwitcher>();
        if (polarity == null) polarity = GetComponent<PlayerPolarity>();
        if (combat == null) combat = GetComponent<PlayerCombat>();
        if (health == null) health = GetComponent<PlayerHealth>();
        if (animationDriver == null) animationDriver = GetComponentInChildren<PlayerAnimationDriver>();
    }

    public void StartGameplay()
    {
        movement?.SetRunning(true);
        movement?.SetControlEnabled(true);
        laneSwitcher?.SetControlEnabled(true);
        combat?.SetControlEnabled(true);
        animationDriver?.PlayStartRun();

        AudioManager.I?.StartRunLoop();
    }

    public void SetControlEnabled(bool enabled)
    {
        movement?.SetControlEnabled(enabled);
        laneSwitcher?.SetControlEnabled(enabled);
        combat?.SetControlEnabled(enabled);
    }

    public void JumpLane()
    {
        laneSwitcher?.TrySwitchLane();
        animationDriver?.SetJump(true);
    }
    public void EndJump()
    {
        animationDriver?.SetJump(false);
    }

    public void Attack()
    {
        combat?.TryAttack();
        animationDriver?.PlayAttack();
    }

    public void TogglePolarity()
    {
        polarity?.Toggle();
    }

    public void OnHit()
    {
        movement?.Stun();
        animationDriver?.PlayHit();

        AudioManager.I?.PlayPlayerHit();
    }

    public void EndHit()
    {
        animationDriver?.EndHit();
    }

    public void OnDeath()
    {
        SetControlEnabled(false);
        movement?.Stop();
        animationDriver?.PlayDeath();

        AudioManager.I?.StopRunLoop();
    }

    public void OnWin()
    {
        StopPlayer();
    }

    public void ForceMoveToTopLane()
    {
        laneSwitcher?.ForceMoveToTopLane();
        animationDriver?.SetJump(true);
    }

    public void ForceMoveToBottomLane()
    {
        laneSwitcher?.ForceMoveToBottomLane();
        animationDriver?.SetJump(true);
    }

    public void StopPlayer()
    {
        movement?.Stop();
        movement?.SetRunning(false);
        movement?.SetControlEnabled(false);

        laneSwitcher?.SetControlEnabled(false);
        combat?.SetControlEnabled(false);

        animationDriver?.SetJump(false);
        animationDriver?.PlayIdle();

        AudioManager.I?.StopRunLoop();
    }
}