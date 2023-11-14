/*using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;

    private bool isJumping;

    public Transform groundCheck;
    public float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capCollider;

    private Vector3 velocity = Vector3.zero;
    float horizontalMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");        
        Flip(rb.velocity.x);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
        if (isJumping && isGrounded())
        {
            animator.SetTrigger("isJumping");
        }

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
        animator.SetBool("isGrounded", isGrounded());
        
    }

    void FixedUpdate()
    {
        MovePlayer(horizontalMovement);
    }

    void MovePlayer(float _horizontalMovement)
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);

        if(isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }else if(_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnCollisonEnter2D(Collision2D collision)
    {
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capCollider.bounds.center, capCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}*/

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private int amountOfJumpsLeft;
    public int amountOfJumps = 1;

    private bool isJumping;
    private bool isGrounded;
    private bool canJump;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 velocity = Vector3.zero;
    float horizontalMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        amountOfJumpsLeft = amountOfJumps;
    }

    void Update()
    {
        CheckGround();

        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;
        CheckIfCanJump();
        //if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        if (Input.GetKeyDown(KeyCode.UpArrow) && canJump)
        {
            isJumping = true;
            animator.SetTrigger("Jump");
        }

        Flip(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("Speed", characterVelocity);      
    }

    void FixedUpdate()
    {
        MovePlayer(horizontalMovement);
    }

    void MovePlayer(float _horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        if(isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            amountOfJumpsLeft--;
            isJumping = false;
        }
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if(amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);

    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }else if(_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
