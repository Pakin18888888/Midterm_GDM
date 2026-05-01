using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float speed = 5f;

    public void SetRunning(bool isRunning)
    {
        speed = isRunning ? 5f : 0f;
    }
    void Update()
    {
        if (!GameManagers.Instance.isRunning) return;

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    public void Stop()
    {
        speed = 0f;
    }
}
