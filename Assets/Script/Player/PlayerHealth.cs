using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase
{
    [SerializeField] private float invincibleTime = 0.5f;
    [SerializeField] private Knockback2D knockback;
    [SerializeField] private HitFlash hitFlash;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private PlayerRoot player;

    private bool invincible;

    protected override void Awake()
    {
        base.Awake();
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    public override void TakeDamage(int amount, Vector2 fromPosition)
    {
        if (invincible) return;

        base.TakeDamage(amount, fromPosition);

        if (currentHP > 0)
        {
            hitFlash?.Play();
            knockback?.Apply((Vector2)transform.position - fromPosition);
            player?.OnHit();
            StartCoroutine(CoInvincible());
        }

        if (hpSlider != null)
            hpSlider.value = currentHP;
    }

    protected override void Die()
    {
        GameFlowController.I?.GameOver();
    }

    private System.Collections.IEnumerator CoInvincible()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;

        player?.EndHit();
    }
}