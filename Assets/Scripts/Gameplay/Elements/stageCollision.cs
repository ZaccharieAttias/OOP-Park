using UnityEngine;


public class StageCollision : MonoBehaviour
{
    [Header("Scripts")]
    public CharacterChallengeManager CharacterChallengeManager;


    public void Start()
    {
        InitializeScripts();
    }
    public void InitializeScripts()
    {
        CharacterChallengeManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandleStageChallenge();
        }
    }

    public void HandleStageChallenge()
    {
        switch (gameObject.name)
        {
            case "Stage1":
                CharacterChallengeManager.SetChallenge1();
                break;
            case "Stage2":
                CharacterChallengeManager.SetChallenge2();
                break;
            case "Stage3":
                CharacterChallengeManager.SetChallenge3();
                break;
        }
    }
}