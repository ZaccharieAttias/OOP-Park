using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    private Vector3 InitialPosition;
    private float timeOnAnotherBox = 0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7) GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        if (collision.gameObject.layer == 9)
        {
            timeOnAnotherBox += Time.deltaTime;
            if (timeOnAnotherBox > 0.25f) GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        }
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
    }
}
