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

    public float sinASin;
    public float sinACos;
    public float sinATan;
    public float cosASin;
    public float cosACos;
    public float cosATan;
    public float tanASin;
    public float tanACos;
    public float tanATan;


    public float fabX;
    public float fabY;
    

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

    private float a = 1;

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

            if (instantiateNumber == 1 && a == 1)
            {
                Instantiate(sohPrefab, new Vector2(transform.position.x + (fabX + sinASin), fabY), Quaternion.identity);
                a = 1;
            }
            else if (instantiateNumber == 1 && a == 2)
            {
                Instantiate(sohPrefab, new Vector2(transform.position.x + (fabX + sinACos), fabY), Quaternion.identity);
                a = 1;
            }
            else if (instantiateNumber == 1 && a == 3)
            {
                Instantiate(sohPrefab, new Vector2(transform.position.x + (fabX + sinATan), fabY), Quaternion.identity);
                a = 1;
            }


            else if (instantiateNumber == 2 && a == 1)
            {
                Instantiate(cahPrefab, new Vector2(transform.position.x + (fabX + cosASin), fabY), Quaternion.identity);
                a = 2;
            }
            else if (instantiateNumber == 2 && a == 2)
            {
                Instantiate(cahPrefab, new Vector2(transform.position.x + (fabX + cosACos), fabY), Quaternion.identity);
                a = 2;
            }
            else if (instantiateNumber == 2 && a == 3)
            {
                Instantiate(cahPrefab, new Vector2(transform.position.x + (fabX + cosATan), fabY), Quaternion.identity);
                a = 2;
            }


            else if (instantiateNumber == 3 && a == 1)
            {
                Instantiate(toaPrefab, new Vector2(transform.position.x + (fabX + tanASin), fabY), Quaternion.identity);
                a = 3;
            }
            else if (instantiateNumber == 3 && a == 2)
            {
                Instantiate(toaPrefab, new Vector2(transform.position.x + (fabX + tanACos), fabY), Quaternion.identity);
                a = 3;
            }
            else if (instantiateNumber == 3 && a == 3)
            {
                Instantiate(toaPrefab, new Vector2(transform.position.x + (fabX + tanATan), fabY), Quaternion.identity);
                a = 3;
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
            timeManager.Instance.timeSlowDuration = timeManager.Instance.timeSlowDuration * 0.97f;
            
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
