using UnityEngine;

public class PlayerLaneController : MonoBehaviour
{
    public float topY = -2.5f;
    public float bottomY = -4.63f;
    public float moveSpeed = 10f;

    private float targetY;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetY = bottomY;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            targetY = topY;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            targetY = bottomY;
        }
    }

    void FixedUpdate()
    {
        float newY = Mathf.Lerp(rb.position.y, targetY, moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(new Vector2(rb.position.x, newY));
    }
}