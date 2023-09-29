using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving = false;

    private bool isFacingRight = true;

    private Vector2 input; //direction from the input

    private Animator animator;

    public LayerMask solidObjectsLayer;

    private Rigidbody2D rb;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
     }

    private void Update()
    {

            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;
            if (input.y != 0) input.x = 0;

            if (input != Vector2.zero)
            {
                Moving();
            }
            else
            {
                isMoving = false;
                animator.SetBool("isMoving",isMoving);
            }
    }

    private void Moving(){
        isMoving = true;
        animator.SetBool("isMoving",isMoving);
        animator.SetFloat("moveX", input.x);
        animator.SetFloat("moveY", input.y);

        if (input.x > 0) isFacingRight = true;
        else if (input.x < 0 ) isFacingRight = false;
        animator.SetBool("isFacingRight",isFacingRight);

        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
}