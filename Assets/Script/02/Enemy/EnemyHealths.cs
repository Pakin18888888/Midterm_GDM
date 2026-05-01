using UnityEngine;

public class EnemyHealths : MonoBehaviour
{ public int hp = 2;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            GetComponent<EnemyControllers>().Die();
        }
    }
}
