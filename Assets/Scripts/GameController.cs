using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 startPos;

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
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(time);
        transform.position = startPos;
    }
}
