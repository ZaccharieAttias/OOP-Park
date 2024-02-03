using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 9f;
    public float JumpForce = 20f;

    public int MaxJumps = 1;
    public int JumpsLeft = 1;
    public bool isGrounded = false;

    public Transform GroundCheckCircle;
    public Transform GroundCheckBox;
    public float GroundCheckRadius = 0.25f;
    public float GroundCheckBoxLength = 0.07f;
    public float GroundCheckBoxWidth = 1.71f;

    public LayerMask CollisionLayers;
    public Rigidbody2D Rigidbody2D;
    public Animator Animator;
    public SpriteRenderer SpriteRenderer;

    public Vector3 Velocity = Vector3.zero;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        GroundCheckCircle = GameObject.Find("Player/GroundCheckCircle").transform;
        GroundCheckBox = GameObject.Find("Player/GroundCheckBox").transform;

        CollisionLayers = LayerMask.GetMask("Ground");
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        JumpsLeft = MaxJumps;
    }

    public void Update()
    {
        CheckGround();
        ProcessInput();
        UpdateAnimator();
    }
    private void CheckGround()
    {   
        bool circleOverlap = Physics2D.OverlapCircle(GroundCheckCircle.position, GroundCheckRadius, CollisionLayers);
        bool boxOverlap = Physics2D.OverlapArea(GroundCheckBox.position, new Vector2(GroundCheckBox.position.x + GroundCheckBoxWidth, GroundCheckBox.position.y - GroundCheckBoxLength), CollisionLayers);

        isGrounded = circleOverlap || boxOverlap;
        if (isGrounded) JumpsLeft = MaxJumps;

    }
    private void ProcessInput()
    {        
        if (Input.GetKeyDown(KeyCode.UpArrow) && JumpsLeft > 0) PerformJump();
        
        SpriteRenderer.flipX = Rigidbody2D.velocity.x < -0.1f;
    }
    private void UpdateAnimator()
    {
        float characterVelocity = Mathf.Abs(Rigidbody2D.velocity.x);

        Animator.SetFloat("Speed", characterVelocity);
        Animator.SetBool("isGrounded", isGrounded);

        if (isGrounded == false) Animator.SetTrigger("Jump");
    }
    
    public void FixedUpdate() { MovePlayer(); }
    private void MovePlayer()
    {
        Vector3 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * MoveSpeed, Rigidbody2D.velocity.y);
        Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref Velocity, .05f);
    }
    
    private void PerformJump()
    {
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, JumpForce);
        JumpsLeft--;
    }
    
    // private void DrawGroundCheckGizmos()
    // {
    //     Gizmos.color = Color.red;

    //     Gizmos.DrawLine(GroundCheckPoint.position, new Vector3(GroundCheckPoint.position.x, GroundCheckPoint.position.y - GroundCheckBoxLength, GroundCheckPoint.position.z));
    //     Gizmos.DrawLine(GroundCheckPoint.position, new Vector3(GroundCheckPoint.position.x + GroundCheckBoxWidth, GroundCheckPoint.position.y, GroundCheckPoint.position.z));
    //     Gizmos.DrawLine(new Vector3(GroundCheckPoint.position.x + GroundCheckBoxWidth, GroundCheckPoint.position.y, GroundCheckPoint.position.z), new Vector3(GroundCheckPoint.position.x + GroundCheckBoxWidth, GroundCheckPoint.position.y - GroundCheckBoxLength, GroundCheckPoint.position.z));
    //     Gizmos.DrawLine(new Vector3(GroundCheckPoint.position.x, GroundCheckPoint.position.y - GroundCheckBoxLength, GroundCheckPoint.position.z), new Vector3(GroundCheckPoint.position.x + GroundCheckBoxWidth, GroundCheckPoint.position.y - GroundCheckBoxLength, GroundCheckPoint.position.z));

    //     Gizmos.DrawWireSphere(GroundCheckPoint.position, GroundCheckRadius);
    // }
}
