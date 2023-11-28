using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    GameController gameController; 
    public Transform respawnPoint;

    SpriteRenderer spriteRenderer;
    public Sprite passive, active;
    Collider2D col;

    void Awake()
    {
        gameController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameController.UpdateCheckpoint(respawnPoint.position);
            spriteRenderer.sprite = active;
            col.enabled = false;
        }
    }
}
