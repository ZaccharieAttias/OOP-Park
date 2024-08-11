using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    public GameController gameController;
    public Transform respawnPoint;

    public SpriteRenderer spriteRenderer;
    public Sprite passive, active;
    public Collider2D col;

    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameController.UpdateCheckpoint(respawnPoint.position);
            spriteRenderer.sprite = active;
            col.enabled = false;
            //activate checkpoint
            GameObject [] Checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
            foreach (GameObject checkpoint in Checkpoints)
            {
                if (checkpoint != this.gameObject)
                {
                    checkpoint.GetComponent<Checkpoint>().col.enabled = true;
                }
            }
        }
    }
    public void ResetCheckpoint()
    {
        spriteRenderer.sprite = passive;
        col.enabled = true;
    }
}
