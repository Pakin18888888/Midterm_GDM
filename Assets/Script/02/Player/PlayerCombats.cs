using UnityEngine;

public class PlayerCombats : MonoBehaviour
{
    public void TryAttack(GameObject enemy)
    {
        EnemyHealths hp = enemy.GetComponent<EnemyHealths>();
        if (hp != null)
        {
            hp.TakeDamage(1);
        }
    }
}
