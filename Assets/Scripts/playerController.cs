using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{

    public timeManager timeManager;

    public float moveSpeed;
    public float jumpForce;
    public float slideSpeed;
    public float leftForce;
    public float slideFreezeTime;
    public float wallPushForce;
    public float wallJumpForce;
    public float slopeJumpForce;

    public float sohX;
    public float cahX;
    public float toaX;
    public float sohY;
    public float cahY;
    public float toaY;

    public GameObject sohPrefab;
    public GameObject cahPrefab;
    public GameObject toaPrefab;

    private float originalMoveSpeed;
    private float originalJumpForce;
    private int instantiateNumber;

    private Rigidbody2D rb;

    private bool jumpPressed = false;
    private bool canJump = false;
    private bool canMove = true;
    private bool isSliding = false;
    private bool isFrozen = false;
    private bool isTouchingWall = false;
    private bool isAirborne = true;
    private bool timeHasFrozen = false;

    void Start()
    {
        originalMoveSpeed = moveSpeed;
        originalJumpForce = jumpForce;
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
            jumpForce = wallJumpForce;
            isTouchingWall = true;
            StartCoroutine(HandleWallCollision());
        }

        if (collision.gameObject.CompareTag("Slope"))
        {
            jumpForce = slopeJumpForce;
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            jumpForce = originalJumpForce;
        }

        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Slope"))
        {
            canJump = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isAirborne = true;
        // Reset jump ability when leaving the platform
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Slope")) 
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
        yield return new WaitForSeconds(slideFreezeTime);
        if (isAirborne == true)
        {
            isSliding = false;
        }
        else if (isAirborne == false)
        {
            isSliding = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Die"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        if (collision.CompareTag("Next"))
        {
            instantiateNumber = Random.Range(1, 4);
            
            if (instantiateNumber == 1)
            {
                Instantiate(sohPrefab, new Vector2(transform.position.x + sohX, sohY), Quaternion.identity);
            }
            else if (instantiateNumber == 2)
            {
                Instantiate(cahPrefab, new Vector2(transform.position.x + cahX, cahY), Quaternion.identity);
            }
            else if (instantiateNumber == 3)
            {
                Instantiate(toaPrefab, new Vector2(transform.position.x + toaX, toaY), Quaternion.identity);
            }
        }

        if (collision.CompareTag("TimeSlow"))
        {
            timeManager.SlowTime();
            if (timeHasFrozen == false)
            {
                leftForce = leftForce * 11f;
            }
            timeHasFrozen = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wall trigger"))
        {
            moveSpeed = originalMoveSpeed;
            jumpForce = originalJumpForce;
        }

        if (other.CompareTag("Wall trigger") && isSliding == true)
        {
            moveSpeed = 0;
        }
        
    }
}
