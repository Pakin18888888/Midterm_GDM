using UnityEngine;

public class EnemyMovements : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        float speed = 3f * GameManagers.Instance.difficulty;

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
