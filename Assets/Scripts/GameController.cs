using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour
{
    Vector2 startPos;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Die();
        }
    }

    void Die()
    {
        StartCoroutine(Respawn(0.25f));
    }

    IEnumerator Respawn(float time)
    {
        rb.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(time);
        transform.position = startPos;
        transform.localScale = new Vector3(1, 1, 1);
        rb.simulated = true;
    }
}
