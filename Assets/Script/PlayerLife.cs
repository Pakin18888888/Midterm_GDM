using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerMovement playerController;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Invincibility")]
    [SerializeField] private float invincibleTime = 0.5f;
    public bool IsInvincible => _invincible;

    [Header("Combat Config")]
    [SerializeField] private CombatConfig combatConfig;
    [SerializeField] private HitFlash hitFlash;
    [SerializeField] private Knockback2D knockback;

    [Header("HP")]
    [SerializeField] private int currentHP;

    [Header("UI")]
    [SerializeField] private Slider hpSlider;

    private bool _invincible;
    private Coroutine _coInv;
    private Coroutine _coHitReset;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
        if (playerController == null) playerController = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        ResetHP();
    }

    public void Kill()
    {
        if (_invincible) return;

        playerController?.PlayDeath();
        GameManager.I?.PlayerDied();
    }

    public void SetControlEnabled(bool enabled)
    {
        if (playerController != null)
            playerController.SetControlEnabled(enabled);
    }

    public void StopMotion()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public void TeleportTo(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetInvincible(float seconds)
    {
        if (_coInv != null) StopCoroutine(_coInv);
        if (seconds <= 0f)
        {
            _invincible = false;
            return;
        }

        _coInv = StartCoroutine(CoInvincible(seconds));
    }

    private IEnumerator CoInvincible(float seconds)
    {
        _invincible = true;
        yield return new WaitForSeconds(seconds);
        _invincible = false;
        _coInv = null;
    }

    public void ResetHP()
    {
        int max = (combatConfig != null) ? combatConfig.playerMaxHP : 5;
        currentHP = max;

        if (hpSlider != null)
        {
            hpSlider.maxValue = max;
            hpSlider.value = currentHP;
        }

        if (anim != null)
        {
            anim.SetBool("isDead", false);
            anim.SetBool("isHit", false);
        }
    }

    public void TakeDamage(int amount, Vector2 fromPosition)
    {
        if (_invincible) return;

        currentHP -= Mathf.Max(0, amount);
        currentHP = Mathf.Max(0, currentHP);

        hitFlash?.Play();
        CameraShake2D.I?.Shake(.08f, .10f);

        Vector2 dir = (Vector2)transform.position - fromPosition;
        knockback?.Apply(dir);

        if (currentHP <= 0)
        {
            Kill();
            return;
        }

        if (anim != null)
        {
            anim.SetBool("isHit", true);

            if (_coHitReset != null)
                StopCoroutine(_coHitReset);

            _coHitReset = StartCoroutine(ResetHitAnim());
        }

        if (invincibleTime > 0f)
            SetInvincible(invincibleTime);
    }

    private IEnumerator ResetHitAnim()
    {
        yield return new WaitForSeconds(0.2f);
        if (anim != null)
            anim.SetBool("isHit", false);
    }

    private void Update()
    {
        if (hpSlider == null) return;

        if (Mathf.Abs(hpSlider.value - currentHP) > 0.01f)
            hpSlider.value = Mathf.Lerp(hpSlider.value, currentHP, Time.deltaTime * 10f);
        else
            hpSlider.value = currentHP;
    }
}