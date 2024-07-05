using System.Collections;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public Vector2 CheckpointPosition;
    public FeedbackManager FeedbackManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        CheckpointPosition = transform.position;

        FeedbackManager = GameObject.Find("Canvas/Popups").GetComponent<FeedbackManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("DeathZone"))
            Die();
    }

    public void UpdateCheckpoint(Vector2 newCheckpointPos)
    {
        CheckpointPosition = newCheckpointPos;
    }

    private void Die()
    {
        FeedbackManager.DeathsCount++;
        StartCoroutine(Respawn(0.25f));
    }

    IEnumerator Respawn(float time)
    {
        Rigidbody2D.velocity = new Vector2(0, 0);
        Rigidbody2D.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(time);

        transform.position = CheckpointPosition;
        transform.localScale = new Vector3(1, 1, 1);
        Rigidbody2D.simulated = true;
    }
    public void ResetGame()
    {
        FeedbackManager.DeathsCount = 0;
        CheckpointPosition = transform.position;
    }
}
