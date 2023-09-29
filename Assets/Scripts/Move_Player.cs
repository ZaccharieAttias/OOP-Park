using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving;

    private Vector2 input; //direction from the input

    private Animator animator;

    public LayerMask solidObjectsLayer;

    private Rigidbody2D rb;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
     }

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                isMoving = true;

                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
                isMoving = false;
            }

        }

        animator.SetBool("isMoving",isMoving);
    }


}