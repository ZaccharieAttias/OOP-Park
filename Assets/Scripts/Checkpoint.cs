using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    GameController gameController; 
    public Transform respawnPoint;

    void Awake()
    {
        gameController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameController.UpdateCheckpoint(respawnPoint.position);
        }
    }
}
