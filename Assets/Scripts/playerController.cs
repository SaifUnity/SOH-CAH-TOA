using UnityEngine;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool jumpPressed;
    private bool canJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        // Handle horizontal movement
        Vector2 targetVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = targetVelocity;

        // Handle jumping
        if (jumpPressed && canJump == true)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpPressed = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is colliding with platform
        if (collision.gameObject.CompareTag("Platform"))
        {
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Reset jump ability when leaving the platform
        if (collision.gameObject.CompareTag("Platform"))
        {
            canJump = false;
        }
    }
}
