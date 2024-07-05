using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    public GameController gameController; 
    public Transform respawnPoint;

    public SpriteRenderer spriteRenderer;
    public Sprite passive, active;
    public Collider2D col;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        gameController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<GameController>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameController.UpdateCheckpoint(respawnPoint.position);
            spriteRenderer.sprite = active;
            col.enabled = false;
        }
    }
}
