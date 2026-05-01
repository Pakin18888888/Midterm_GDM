using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 10f;

    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    public int maxJump = 2; // 👈 สำคัญ
    private int jumpCount = 0;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        // รีเซ็ตเมื่อแตะพื้น
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // กดกระโดด
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (jumpCount < maxJump)
            {
                Jump();
                jumpCount++;
            }
        }
    }

    void Jump()
    {
        // กันเด้งแรงเกิน
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}