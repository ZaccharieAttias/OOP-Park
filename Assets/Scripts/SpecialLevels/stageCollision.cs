using UnityEngine;
using UnityEngine.UI;




public class stageCollision : MonoBehaviour
{
    public CharacterChallengeManager CharacterChallengeManager;


    public void Start()
    {
        CharacterChallengeManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.name == "Stage1") CharacterChallengeManager.SetChallenge1();
            if (gameObject.name == "Stage2") CharacterChallengeManager.SetChallenge2();
            if (gameObject.name == "Stage3") CharacterChallengeManager.SetChallenge3();
        }
    }
}