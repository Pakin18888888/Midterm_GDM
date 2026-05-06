using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 3f;

    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y;
    }

    void Update()
    {
        float finalSpeed = speed + GameManagers.Instance.difficulty * 0.5f;

        transform.position += Vector3.left * finalSpeed * Time.deltaTime;

        transform.position = new Vector3(transform.position.x, fixedY, 0);
    }
}