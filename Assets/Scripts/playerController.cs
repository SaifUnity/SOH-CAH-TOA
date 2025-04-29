using UnityEngine;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float slideSpeed = 2f;
    public float leftForce = 5f;

    private Rigidbody2D rb;

    private bool jumpPressed = false;
    private bool canJump = false;
    private bool canMove = true;
    private bool isSliding = false;

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
        if (canMove == true)
        {
            // Handle horizontal movement
            Vector2 targetVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = targetVelocity;

            rb.gravityScale = 1;
        }
        else if (canMove == false)
        {
            rb.gravityScale = slideSpeed;
        }
        

        // Handle jumping
        if (jumpPressed == true && canJump == true)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (isSliding == true)
            {
                rb.AddForce(Vector2.left * leftForce, ForceMode2D.Impulse);
            }

            jumpPressed = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            canMove = false;

            isSliding = true;
        }

        // Check if the player is colliding with platform
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Wall"))
        {
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Reset jump ability when leaving the platform
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Wall")) 
        {
            canMove = true;
            canJump = false;
            isSliding = false;
        }
    }
}
