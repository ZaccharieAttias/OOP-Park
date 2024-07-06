using System.Collections;
using System.Collections.Generic;
using LootLocker.Extension.DataTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayTestManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] Checkpoints;
    public GameObject[] DeathZone;
    public float x_start_pos, y_start_pos;
    public GameObject TestButton;
    public bool IsTestGameplay = false;
    public GameObject MainCamera;

    public void RunTestGameplay()
    {
        if (Player == null)
        {
            Debug.LogError("Player is not set");
            return;
        }
        IsTestGameplay = true;
        Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Player.GetComponent<Movement>().enabled = true;
        Player.GetComponent<GameController>().enabled = true;
        Player.GetComponent<GrabObject>().enabled = true;

        Checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        if (GameObject.FindGameObjectsWithTag("DeathZone") != null)
        {
            DeathZone = GameObject.FindGameObjectsWithTag("DeathZone");
            foreach (GameObject deathzone in DeathZone)
                deathzone.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        MainCamera.GetComponent<CameraFollow>().Player = Player;
    }

    public void ResetTestGameplay()
    {
        IsTestGameplay = false;
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Player.GetComponent<Movement>().enabled = false;
        Player.GetComponent<GameController>().enabled = false;
        Player.GetComponent<GrabObject>().enabled = false;
        Player.transform.position = new Vector3(x_start_pos, y_start_pos, 0);
        Player.GetComponent<Movement>().ResetPlayer();
        Player.GetComponent<GameController>().ResetGame();
        Player.GetComponent<GrabObject>().ResetGrab();
        foreach (GameObject checkpoint in Checkpoints)
            checkpoint.GetComponent<Checkpoint>().ResetCheckpoint();
        foreach (GameObject deathzone in DeathZone)
            deathzone.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        MainCamera.GetComponent<CameraFollow>().Player = null;
    }

    public void SetOnlickButton()
    {   
        if (IsTestGameplay)
            ResetTestGameplay();
        else
            RunTestGameplay();
    }
}
