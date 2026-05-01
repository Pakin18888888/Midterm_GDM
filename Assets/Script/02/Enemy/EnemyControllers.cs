using UnityEngine;

public class EnemyControllers : MonoBehaviour
{
    // public EnemyData data;

    // public void Init(EnemyData d)
    // {
        // data = d;
    // }

    public void Die()
    {
        Destroy(gameObject);
    }
}
