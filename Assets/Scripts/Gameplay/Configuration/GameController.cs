using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using System.Collections;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public Vector2 CheckpointPosition;
    public FeedbackManager FeedbackManager;
    public bool isDead = false;

    public AiModelData AiModelData;

    [Header("Audio:")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip victorySound;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        AiModelData = GameObject.Find("Scripts/AiModelData").GetComponent<AiModelData>();

        Rigidbody2D = GetComponent<Rigidbody2D>();
        CheckpointPosition = transform.position;

        FeedbackManager = GameObject.Find("Canvas/Popups").GetComponent<FeedbackManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("DeathZone"))
        {
            Die();
        }

        if (collision.gameObject.CompareTag("Finish")){ GetComponent<Character>().Animator.SetBool("Victory", true); FeedbackManager.ToggleOn(); }

        if (collision.gameObject.CompareTag("EndPoint") && GameObject.Find("Scripts/PlayTestManager").GetComponent<PlayTestManager>().IsTestGameplay)
        {
            Waiting();
            SoundManager.instance.PlaySound(victorySound);
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
        StartCoroutine(Respawn(1f));
        AiModelData.DeathsCount++;

    }

    IEnumerator Respawn(float time)
    {
        isDead = true;
        Rigidbody2D.velocity = new Vector2(0, 0);
        Rigidbody2D.simulated = false;
        var predecentlocalScale = transform.localScale;
        GetComponent<Character>().SetState(CharacterState.DeathB);
        SoundManager.instance.PlaySound(deathSound);
        yield return new WaitForSeconds(0.45f);
        transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(time);

        transform.position = CheckpointPosition;
        transform.localScale = predecentlocalScale;
        Rigidbody2D.simulated = true;
        isDead = false;
    }


    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
    }
    public void ResetGame()
    {
        AiModelData.DeathsCount = 0;
        CheckpointPosition = transform.position;
    }
    public void returnLastPosition()
    {
        transform.position = CheckpointPosition;
    }
}
