using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public GameObject GameManager;
    public ParticleSystem landingParticles;
    private Vector3? savedRespawnPoint = null;
    private float landedTime = 0f;
    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded = true;
    private Vector3 inputMove;
    private bool jumpPressed = false;

    private int platformsHoppedCount = 0;
    private GameObject lastPlatformLandedOn;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        moveSpeed = DifficultyData.speed;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        
        if (Mathf.Abs(moveX) < 0.1f) moveX = 0f;
        if (Mathf.Abs(moveZ) < 0.1f) moveZ = 0f;

        inputMove = new Vector3(moveX, 0f, moveZ).normalized;

        if (Input.GetKey(KeyCode.Space) && isGrounded)
            jumpPressed = true;

        
        anim.SetFloat("MoveX", moveX);
        anim.SetFloat("MoveZ", moveZ);

        bool isActuallyMoving = inputMove.magnitude > 0f && isGrounded;
        anim.SetBool("isMoving", isActuallyMoving);

        
        anim.SetBool("isGrounded", isGrounded);
    }
    public void SaveRespawnPoint(Vector3 pos)
    {
        savedRespawnPoint = pos;
    }

    void FixedUpdate()
    {
        Vector3 velocity = new Vector3(inputMove.x * moveSpeed, rb.linearVelocity.y, inputMove.z * moveSpeed);
        rb.linearVelocity = velocity;

        if (jumpPressed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isGrounded = false;
            jumpPressed = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Ground") && !isGrounded))
        {
            isGrounded = true;
            landedTime = Time.time;

            
            
            if (collision.gameObject != lastPlatformLandedOn)
            {
                platformsHoppedCount++;
                lastPlatformLandedOn = collision.gameObject;

                GameManager.GetComponent<GameManager>().UpdatePlatformHoppedDisplay(platformsHoppedCount);
            }

            if (landingParticles != null)
                landingParticles.Play();
        }

        if (collision.gameObject.CompareTag("Fallen"))
        {
            if (savedRespawnPoint.HasValue)
            {
                
                rb.linearVelocity = Vector3.zero;
                transform.position = savedRespawnPoint.Value + Vector3.up * 1f;
                savedRespawnPoint = null; 
            }
            else
            {
                GameManager.GetComponent<GameManager>().TriggerGameOver();
            }
        }

        if (collision.gameObject.CompareTag("End"))
        {
            GameManager.GetComponent<GameManager>().TriggerGameWin();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
