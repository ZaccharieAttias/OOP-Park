using UnityEngine;
using UnityEngine.UI;




public class stageCollision : MonoBehaviour
{
    public CharacterChallangeManager CharacterChallangeManager;


    public void Start()
    {
        CharacterChallangeManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterChallangeManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.name == "Stage1") CharacterChallangeManager.SetChallenge1();
            if (gameObject.name == "Stage2") CharacterChallangeManager.SetChallenge2();
            if (gameObject.name == "Stage3") CharacterChallangeManager.SetChallenge3();
        }
    }
}