using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 startPos;
    spriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(time);
        transform.position = startPos;
        spriteRenderer.enabled = true;
    }
}
