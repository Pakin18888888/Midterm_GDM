using UnityEngine;
using System.Collections;

public class PlayerHealths : MonoBehaviour
{
    public int hp = 3;

    public float invincibleTime = 1.5f; // ⏱️ คูลดาวน์
    private bool isInvincible = false;
    private bool isDead = false;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;

    public System.Action<int> OnHPChanged;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        StartCoroutine(StartInvincible());
    }

    IEnumerator StartInvincible()
    {
        isInvincible = true;

        // 🔥 กันชนกับ enemy ตั้งแต่เริ่ม
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            true
        );

        yield return new WaitForSeconds(1f);

        // 🔁 กลับมาชนปกติ
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            false
        );

        isInvincible = false;
    }

    public void TakeDamage(int dmg)
    {
        if (isInvincible || isDead) return;

        hp -= dmg;
        OnHPChanged?.Invoke(hp);

        Debug.Log("Player Hit!");
        ScoreManager.Instance.ResetStreak();

        // 💥 effect
        rb.AddForce(Vector2.left * 5f, ForceMode2D.Impulse);
        CameraShakes.Instance.Shake(.5f, 2f);

        StartCoroutine(InvincibleRoutine());

        if (hp <= 0)
        {
            Die();
        }
    }

    IEnumerator InvincibleRoutine()
    {
        isInvincible = true;

        // 👾 ไม่ชน enemy
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            true
        );

        // ✨ กระพริบ
        float t = 0f;
        while (t < invincibleTime)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            t += 0.1f;
        }

        sr.enabled = true;

        // 🔁 กลับมาชนปกติ
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            false
        );

        isInvincible = false;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        GameManagers.Instance.GameState(false);
    }
}