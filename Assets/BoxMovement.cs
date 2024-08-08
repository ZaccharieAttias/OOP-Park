using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    private Vector3 InitialPosition;
    private float timeOnAnotherBox = 0f;
    public float timeOnGround = 0f;
    public bool isOnGround = false;
    public bool isOnAnotherBox = false;
    public bool isFirstTime = true;

    public void Update()
    {
        if (isOnGround && isFirstTime)
        {
            timeOnGround += Time.deltaTime;
            if (timeOnGround > 1.5f)
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                isFirstTime = false;

                SetAngle();
            }
        }

        if (isOnAnotherBox && isFirstTime)
        {
            timeOnAnotherBox += Time.deltaTime;
            if (timeOnAnotherBox > 3f)
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                isFirstTime = false;

                SetAngle();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7) 
        {
            isOnGround = true;
            timeOnGround = 0f;
            isFirstTime = true;
        }
        if (collision.gameObject.layer == 9)
        {
            isOnAnotherBox = true;
            timeOnAnotherBox = 0f;
            isFirstTime = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7) 
            isOnGround = false;
        if (collision.gameObject.layer == 9)
            isOnAnotherBox = false;
    }
    public void SetAngle()
    {
        float angle = transform.rotation.eulerAngles.z;
        if (angle > 315 || angle < 45)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (angle > 45 && angle < 135)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (angle > 135 && angle < 225)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (angle > 225 && angle < 315)
            transform.rotation = Quaternion.Euler(0, 0, 270);
    }
    public void SetPosition(Vector3 position)
    {
        InitialPosition = position;
    }
    public void Respawn()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.position = InitialPosition;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void ResetTimers()
    {
        timeOnGround = 0f;
        timeOnAnotherBox = 0f;
        isOnGround = false;
        isOnAnotherBox = false;
        isFirstTime = true;
    }
}
