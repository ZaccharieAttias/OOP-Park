using System.Collections;
using System.Collections.Generic;
using LootLocker.Extension.DataTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System.Linq;

public class PlayTestManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] Checkpoints;
    public GameObject[] DeathZone;
    public float x_start_pos, y_start_pos;
    public GameObject TestButton;
    public bool IsTestGameplay = false;
    public GameObject MainCamera;
    public JsonUtilityManager JsonUtilityManager;
    private string PreviousPath;
    public GameObject TreeContent;
    public GameObject SpecialAbilityTreeContent;
    public GameObject[] bricks;

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
        MainCamera.GetComponent<CameraFollow>().StartPosition = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);

        PreviousPath = JsonUtilityManager.FolderPath;
        JsonUtilityManager.SetPath(Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Temp");
        JsonUtilityManager.Save();

        foreach (var character in CharactersData.CharactersManager.CharactersCollection)
            character.IsOriginal = true;
        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CharactersCollection.First());
        GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>().LoadFromJson();

        RestrictionManager.Instance.OnlineBuild = false;

        bricks = GameObject.FindGameObjectsWithTag("Brick");
    }

    public void ResetTestGameplay()
    {
        IsTestGameplay = false;
        RestrictionManager.Instance.OnlineBuild = true;

        foreach (Transform child in TreeContent.transform)
            Destroy(child.gameObject);
        GameObject PopUp = GameObject.Find("Canvas/Popups");
        PopUp.GetComponent<CharacterSelectionManager>().CleanContent();
        PopUp.GetComponent<CharacterSelectionManager>().MenuInitialization();
        PopUp.GetComponent<SpecialAbilityManager>().SpecialAbilityGameObjects.Clear();

        JsonUtilityManager.SetPath(Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Temp");
        JsonUtilityManager.Load();
        JsonUtilityManager.SetPath(PreviousPath);

        
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Player.GetComponent<Movement>().enabled = false;
        Player.GetComponent<GameController>().enabled = false;
        Player.GetComponent<GrabObject>().enabled = false;
        Player.transform.position = new Vector3(x_start_pos, y_start_pos, 0);
        Player.GetComponent<GameController>().ResetGame();
        Player.GetComponent<GrabObject>().ResetGrab();
        foreach (GameObject checkpoint in Checkpoints)
            checkpoint.GetComponent<Checkpoint>().ResetCheckpoint();
        foreach (GameObject deathzone in DeathZone)
            deathzone.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        MainCamera.GetComponent<CameraFollow>().Player = null;
        MainCamera.GetComponent<CameraFollow>().ResetPosition();

        foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Temp"))
            File.Delete(file);
        AssetDatabase.Refresh();

        GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>().LoadFromJson();
        GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>().ApplyPowerup(CharactersData.CharactersManager.CurrentCharacter);

        foreach (Transform child in PopUp.transform)
            child.gameObject.SetActive(false);

        //trouvez ts les objets qui ont le tag "Brick" et activez les
        foreach (GameObject brick in bricks)
            brick.GetComponent<BreakingBrick>().Activate();
        
    }

    public void SetOnlickButton()
    {   
        if (IsTestGameplay)
            ResetTestGameplay();
        else
            RunTestGameplay();
    }
}
