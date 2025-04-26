using UnityEngine;

public class playerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;

    private bool jumpPressed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check for jump input (input should stay in Update)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        // Handle horizontal movement with constant velocity
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

        // Handle jumping
        if (jumpPressed)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpPressed = false;
        }
    }
}
