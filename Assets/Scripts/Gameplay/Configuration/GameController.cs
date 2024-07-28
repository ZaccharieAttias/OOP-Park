using System.Collections;
using UnityEngine;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;


public class GameController : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public Vector2 CheckpointPosition;
    public FeedbackManager FeedbackManager;
    bool isDead = false;


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

        if (collision.gameObject.CompareTag("Finish"))
            FeedbackManager.ToggleOn();
        if (collision.gameObject.CompareTag("EndPoint") && GameObject.Find("Scripts/PlayTestManager").GetComponent<PlayTestManager>().IsTestGameplay)
            {   
                Waiting();
                GameObject.Find("Scripts/PlayTestManager").GetComponent<PlayTestManager>().ResetTestGameplay();
                GameObject Buttons = GameObject.Find("Canvas/Menus/Gameplay/Buttons");
                Buttons.transform.GetChild(0).gameObject.SetActive(true);
                Buttons.transform.GetChild(1).gameObject.SetActive(true);
                Buttons.transform.GetChild(2).gameObject.SetActive(false);
                Buttons.transform.GetChild(3).gameObject.SetActive(true);
                Buttons.transform.GetChild(4).gameObject.SetActive(true);
            }
    }

    public void UpdateCheckpoint(Vector2 newCheckpointPos)
    {
        CheckpointPosition = newCheckpointPos;
    }

    private void Die()
    {
        if (isDead) return;
        GetComponent<Character>().SetState(CharacterState.DeathB);
        StartCoroutine(Respawn(0.25f));
        FeedbackManager.DeathsCount++;
    }

    IEnumerator Respawn(float time)
    {
        isDead = true;
        Rigidbody2D.velocity = new Vector2(0, 0);
        Rigidbody2D.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(time);

        transform.position = CheckpointPosition;
        transform.localScale = new Vector3(1, 1, 1);
        Rigidbody2D.simulated = true;
        isDead = false;
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
    }
    public void ResetGame()
    {
        FeedbackManager.DeathsCount = 0;
        CheckpointPosition = transform.position;
    }
}
