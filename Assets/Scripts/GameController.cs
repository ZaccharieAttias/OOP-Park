using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour
{
    Vector2 checkpointPosition;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        checkpointPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Die();
        }
    }

    public void UpdateCheckpoint(Vector2 newCheckpointPos)
    {
        checkpointPosition = newCheckpointPos;
    }

    void Die()
    {
        StartCoroutine(Respawn(0.25f));
    }

    IEnumerator Respawn(float time)
    {
        rb.velocity = new Vector2(0, 0);
        rb.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(time);
        transform.position = checkpointPosition;
        transform.localScale = new Vector3(3, 3, 1);
        rb.simulated = true;
    }
}
