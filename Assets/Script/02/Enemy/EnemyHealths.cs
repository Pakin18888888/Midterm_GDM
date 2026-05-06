using UnityEngine;

public class EnemyHealths : MonoBehaviour
{
    public int hp = 2;
    public int scoreValue = 10; // 🔥 ให้คะแนนต่อ 1 ตัว

    bool isDead = false;

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        hp -= dmg;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        Debug.Log("DIE");
        if (isDead) return;
        isDead = true;

        if (EnemyManager.Instance != null)
            EnemyManager.Instance.UnregisterEnemy();

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(scoreValue);

        if (ComboManager.Instance != null)
            ComboManager.Instance.ShowScore(scoreValue, transform.position);

        GetComponent<EnemyControllers>().Die();
    }
}