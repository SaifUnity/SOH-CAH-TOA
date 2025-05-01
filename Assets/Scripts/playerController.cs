using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float slideSpeed = 2f;
    public float leftForce = 5f;
    public float slideFreezeTime = 0.5f;
    public float wallPushForce = 2f;

    private float originalMoveSpeed;

    private Rigidbody2D rb;

    private bool jumpPressed = false;
    private bool canJump = false;
    private bool canMove = true;
    private bool isSliding = false;
    private bool isFrozen = false;
    private bool isTouchingWall = false;
    private bool isAirborne = true;

    void Start()
    {
        originalMoveSpeed = moveSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isAirborne == false)
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {

        if (isFrozen == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (isFrozen == false)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

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
            isSliding = false;
            isFrozen = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (isTouchingWall == true)
            {
                rb.AddForce(Vector2.left * leftForce, ForceMode2D.Impulse);
            }

            jumpPressed = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isAirborne = false;
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveSpeed = wallPushForce;
            isTouchingWall = true;
            StartCoroutine(HandleWallCollision());
        }

        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Wall"))
        {
            canJump = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isAirborne = true;
        // Reset jump ability when leaving the platform
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Wall")) 
        {
            canMove = true;
            canJump = false;
            isTouchingWall = false;
        } 
    }
    private IEnumerator HandleWallCollision()
    {
        canMove = false;
        isFrozen = true;
        yield return new WaitForSeconds(slideFreezeTime);
        isFrozen = false;
        isSliding = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Die"))
        {

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wall trigger"))
        {
            moveSpeed = originalMoveSpeed;
        }

        if (other.CompareTag("Wall trigger") && isSliding == true)
        {
            moveSpeed = 0;
        }
    }
}
