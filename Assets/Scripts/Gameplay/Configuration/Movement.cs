using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Character move and jump example. Built-in component CharacterController (3D) is used. It can be replaced by 2D colliders.
/// </summary>
public class Movement : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Rigidbody2D Rigidbody2D;

    public CharactersManager CharactersManager;
    public Powerup Powerup;

    [Header("Object Ref:")]
    public Transform GroundCheckCircle;
    public Transform GroundCheckBox;
    public Transform firePoint;
    [SerializeField] public List<GameObject> fireballs;
    public Transform WallCheck;
    public GameObject Gun;

    [Header("Movement:")]
    private float horizontalInput;
    public float MoveSpeed = 9f;
    public float JumpForce = 20f;
    public int MaxJumps = 1;
    public int JumpsLeft = 1;
    public Vector3 Velocity;
    public float WallSlideSpeed = 2f;
    public float WallJumpingDirection = 1f;
    public float WallJumpingCounter = 1f;
    public Vector2 WallJumpingVector;

    [Header("Ground Check:")]

    public List<LayerMask> CollisionLayers;
    public bool isGrounded = false;
    public float GroundCheckRadius = 0.25f;
    public float GroundCheckBoxLength = 0.07f;
    public float GroundCheckBoxWidth = 1.71f;

    [Header("Wall Check:")]
    public LayerMask WallLayer;
    public bool IsWallSliding = false;
    public bool IsWallJumping = false;

    public bool AllowToWallJump = false;

    [Header("Timers:")]
    public float attackCooldown = Mathf.Infinity;
    public float cooldownTimer = Mathf.Infinity;
    public float PowerupTimer = 0f;
    public float WallJumpingTime = 0.2f;
    public float WallJumpingDuration = 0.4f;


    public Character Character;
    public Vector3 _speed = Vector3.zero;

    public void Awake() { InitializeProperties(); }
    private void InitializeProperties()
    {
        CollisionLayers = new List<LayerMask>();
        CollisionLayers.Add(LayerMask.GetMask("Ground"));
        CollisionLayers.Add(LayerMask.GetMask("Grabbable"));
        Rigidbody2D = GetComponent<Rigidbody2D>();
        GroundCheckCircle = GameObject.Find("Player/GroundCheckCircle").transform;
        GroundCheckBox = GameObject.Find("Player/GroundCheckBox").transform;
        firePoint = GameObject.Find("Player/FirePoint").transform;
        Powerup = GetComponent<Powerup>();
        CharactersManager = GetComponent<CharactersManager>();
        Gun = GameObject.Find("Player/GunPivot");
        WallLayer = LayerMask.GetMask("Wall");
        WallCheck = GameObject.Find("Grid/Walls").transform;


        Velocity = Vector3.zero;
        JumpsLeft = MaxJumps;
        fireballs = new List<GameObject>();
        foreach (Transform child in GameObject.Find("FireballManager").transform)
        {
            fireballs.Add(child.gameObject);
        }
        WallJumpingVector = new Vector2(24f, 18f);

        Character.Animator.SetBool("Ready", true);
    }

    public void Update()
    {
        PowerupTimer = 0f;
        horizontalInput = Input.GetAxis("Horizontal");

        var direction = Vector2.zero;
        CheckGround();
        CheckWallSliding();
        PerformWallSlide();
        if (Input.GetKeyDown(KeyCode.W) && JumpsLeft > 0) PerformJump();
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (horizontalInput > 0.01f)//inverse gravity
                transform.localScale = new Vector3(1, 1, 1);
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);
            if (Rigidbody2D.gravityScale < 0)
                if (horizontalInput > 0.01f)
                    transform.localScale = new Vector3(1, -1, 1);
                else if (horizontalInput < -0.01f)
                    transform.localScale = new Vector3(-1, -1, 1);

        }
        if (Input.GetKey(KeyCode.A)) direction.x = -1;
        if (Input.GetKey(KeyCode.D)) direction.x = 1;

        if (direction.x != 0)
        {
            Turn(direction.x);
        }

        Move(direction);

        if (Rigidbody2D.velocity.y == 0) JumpsLeft = MaxJumps;

        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     Character.SetState(CharacterState.DeathB);
        // }

        if (Input.GetKeyDown(KeyCode.Q) && cooldownTimer > attackCooldown) PerformAttack();

        if (AllowToWallJump) WallJump();
        cooldownTimer += Time.deltaTime;
    }

    private void PerformJump()
    {
        //si la gravité est inversée, on saute vers le bas
        if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "InverseGravity") Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, -JumpForce);
        else Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, JumpForce);
        JumpsLeft--;

        if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "MultipleJumps") Powerup.PreviousUpcastMethod.UpcastingTrackerManager.UpdateUpcastingMethod(1);
    }

    private void CheckGround()
    {
        bool circleOverlap;
        bool boxOverlap;
        foreach (LayerMask layer in CollisionLayers)
        {
            circleOverlap = Physics2D.OverlapCircle(GroundCheckCircle.position, GroundCheckRadius, layer);
            if (transform.localScale.x > 0)
                boxOverlap = Physics2D.OverlapArea(GroundCheckBox.position, new Vector2(GroundCheckBox.position.x + GroundCheckBoxWidth, GroundCheckBox.position.y - GroundCheckBoxLength), layer);
            else
                boxOverlap = Physics2D.OverlapArea(GroundCheckBox.position, new Vector2(GroundCheckBox.position.x - GroundCheckBoxWidth, GroundCheckBox.position.y - GroundCheckBoxLength), layer);

            isGrounded = circleOverlap || boxOverlap;
            if (isGrounded) break;
        }
    }

    public void Move(Vector2 direction)
    {
        if (isGrounded)
        {
            if (direction != Vector2.zero)
            {
                Character.SetState(CharacterState.Run);
            }
            else if (Character.GetState() < CharacterState.DeathB)
            {
                Character.SetState(CharacterState.Idle);
            }
        }
        else
        {
            Character.SetState(CharacterState.Jump);
        }

        Move();
    }

    public void Turn(float direction)
    {
        Character.transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
    }

    public void Move()
    {
        Vector3 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * MoveSpeed, Rigidbody2D.velocity.y);
        Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref Velocity, .05f);

        PowerupTimer += Time.deltaTime;
        if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "Speed") Powerup.PreviousUpcastMethod.UpcastingTrackerManager.UpdateUpcastingMethod(PowerupTimer);
        if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "Gravity") Powerup.PreviousUpcastMethod.UpcastingTrackerManager.UpdateUpcastingMethod(PowerupTimer);
        if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "Grappling") Powerup.PreviousUpcastMethod.UpcastingTrackerManager.UpdateUpcastingMethod(10);
        if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "InverseGravity") Powerup.PreviousUpcastMethod.UpcastingTrackerManager.UpdateUpcastingMethod(PowerupTimer);
    }
    private void PerformAttack()////////////////////
    {
        //Animator.SetTrigger("Shoot");
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<ProjectileB>().SetDirection(Mathf.Sign(transform.localScale.x), MoveSpeed + 10);

        if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "FireballShoot") Powerup.PreviousUpcastMethod.UpcastingTrackerManager.UpdateUpcastingMethod(1);

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
    private void CheckWallSliding()
    {
        bool boxOverlap;
        if (transform.localScale.x > 0)
            boxOverlap = Physics2D.OverlapArea(GroundCheckBox.position, new Vector2(GroundCheckBox.position.x + GroundCheckBoxWidth, GroundCheckBox.position.y - GroundCheckBoxLength), WallLayer);
        else
            boxOverlap = Physics2D.OverlapArea(GroundCheckBox.position, new Vector2(GroundCheckBox.position.x - GroundCheckBoxWidth, GroundCheckBox.position.y - GroundCheckBoxLength), WallLayer);

        IsWallSliding = boxOverlap && !isGrounded && horizontalInput != 0;

    }
    private void PerformWallSlide()
    {
        if (IsWallSliding)
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Mathf.Clamp(Rigidbody2D.velocity.y, -WallSlideSpeed, float.MaxValue));
        }
    }
    private void WallJump()
    {
        if (IsWallSliding)
        {
            IsWallJumping = false;
            WallJumpingDirection = -transform.localScale.x;
            WallJumpingCounter = WallJumpingTime;
            CancelInvoke(nameof(StopwallJumping));
        }
        else
            WallJumpingCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W) && WallJumpingCounter > 0)
        {
            IsWallJumping = true;
            Rigidbody2D.velocity = new Vector2(WallJumpingDirection * WallJumpingVector.x, WallJumpingVector.y);
            WallJumpingCounter = 0;

            if (transform.localScale.x != WallJumpingDirection)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            Invoke(nameof(StopwallJumping), WallJumpingDuration);
            if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "WallJump") Powerup.PreviousUpcastMethod.UpcastingTrackerManager.UpdateUpcastingMethod(1);
        }
    }
    private void StopwallJumping()
    {
        IsWallJumping = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (transform.localScale.x > 0)
        {
            Vector3 halfPosition = new Vector3(GroundCheckBox.position.x + GroundCheckBoxWidth / 2, GroundCheckBox.position.y - GroundCheckBoxLength / 2, GroundCheckBox.position.z);
            Gizmos.DrawWireCube(halfPosition, new Vector3(GroundCheckBoxWidth, GroundCheckBoxLength, 0));
        }
        else
        {
            Vector3 halfPosition = new Vector3(GroundCheckBox.position.x - GroundCheckBoxWidth / 2, GroundCheckBox.position.y - GroundCheckBoxLength / 2, GroundCheckBox.position.z);
            Gizmos.DrawWireCube(halfPosition, new Vector3(GroundCheckBoxWidth, GroundCheckBoxLength, 0));
        }

        Gizmos.DrawWireSphere(GroundCheckCircle.position, GroundCheckRadius);
    }
}