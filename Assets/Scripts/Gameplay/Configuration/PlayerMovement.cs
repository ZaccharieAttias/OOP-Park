using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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

    public Vector3 Velocity;
    public float attackCooldown = Mathf.Infinity;
    public float cooldownTimer = Mathf.Infinity;
    public Transform firePoint;
    [SerializeField] public List<GameObject> fireballs;
    private float horizontalInput;


    public void Awake() { InitializeProperties(); }
    private void InitializeProperties()
    {
        CollisionLayers = LayerMask.GetMask("Ground");
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        Velocity = Vector3.zero;
        JumpsLeft = MaxJumps;

        GroundCheckCircle = GameObject.Find("Player/GroundCheckCircle").transform;
        GroundCheckBox = GameObject.Find("Player/GroundCheckBox").transform;
        firePoint = GameObject.Find("Player/FirePoint").transform;
        fireballs = new List<GameObject>();
        foreach (Transform child in GameObject.Find("FireballManager").transform)
        {
            fireballs.Add(child.gameObject);
        }
    }

    public void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        CheckGround();
        ProcessInput();
        MovePlayer();
        UpdateAnimator();

        if (Rigidbody2D.velocity.y == 0) JumpsLeft = MaxJumps;
        cooldownTimer += Time.deltaTime;
    }
    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && JumpsLeft > 0) PerformJump();
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            if (horizontalInput > 0.01f)
                transform.localScale = new Vector3(3, 3, 3);
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-3, 3, 3);
        }
        if (Input.GetKeyDown(KeyCode.Q) && cooldownTimer > attackCooldown ) PerformAttack();
    }
    private void MovePlayer()
    {
        Vector3 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * MoveSpeed, Rigidbody2D.velocity.y);
        Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref Velocity, .05f);
    }
    private void CheckGround()
    {   
        bool circleOverlap = Physics2D.OverlapCircle(GroundCheckCircle.position, GroundCheckRadius, CollisionLayers);
        bool boxOverlap;
        if (transform.localScale.x > 0) 
            boxOverlap = Physics2D.OverlapArea(GroundCheckBox.position, new Vector2(GroundCheckBox.position.x + GroundCheckBoxWidth, GroundCheckBox.position.y - GroundCheckBoxLength), CollisionLayers);
        else 
            boxOverlap = Physics2D.OverlapArea(GroundCheckBox.position, new Vector2(GroundCheckBox.position.x - GroundCheckBoxWidth, GroundCheckBox.position.y - GroundCheckBoxLength), CollisionLayers);

        isGrounded = circleOverlap || boxOverlap;
    }
    private void UpdateAnimator()
    {
        Animator.SetFloat("Speed", Mathf.Abs(Rigidbody2D.velocity.x));
        Animator.SetBool("isGrounded", isGrounded);

        if (isGrounded == false) Animator.SetTrigger("Jump");
    }

    private void PerformJump()
    {
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, JumpForce);
        JumpsLeft--;
    }

    private void PerformAttack()
    {
        Animator.SetTrigger("Shoot");
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x), MoveSpeed + 10);
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Count; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (transform.localScale.x > 0) {
            Vector3 halfPosition = new Vector3(GroundCheckBox.position.x + GroundCheckBoxWidth / 2, GroundCheckBox.position.y - GroundCheckBoxLength / 2, GroundCheckBox.position.z);
            Gizmos.DrawWireCube(halfPosition, new Vector3(GroundCheckBoxWidth, GroundCheckBoxLength, 0));}
        else
        {
            Vector3 halfPosition = new Vector3(GroundCheckBox.position.x - GroundCheckBoxWidth / 2, GroundCheckBox.position.y - GroundCheckBoxLength / 2, GroundCheckBox.position.z);
            Gizmos.DrawWireCube(halfPosition, new Vector3(GroundCheckBoxWidth, GroundCheckBoxLength, 0));
        }
        
        Gizmos.DrawWireSphere(GroundCheckCircle.position, GroundCheckRadius);
    }
}
