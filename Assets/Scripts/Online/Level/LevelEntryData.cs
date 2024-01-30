using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LevelEntryData : MonoBehaviour
{
    public int ID;
    public string LevelName;
    public TMP_Text NameText;
    public Image LevelIcon;
    public string TextFileURL;

    public void Start()
    {
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;

        NameText.text = LevelName;
        LevelIcon.sprite = LevelIcon.sprite;
    }

    public void LoadLevel()
    {
        StartCoroutine(DownloadTextFile(TextFileURL));
    }

    private IEnumerator DownloadTextFile(string textfileURL)
    {
        UnityWebRequest www = UnityWebRequest.Get(textfileURL);
        yield return www.SendWebRequest();

        string filePath = Directory.GetCurrentDirectory() + "/Assets/Screenshots/" + LevelName + "-Data.txt";
        File.WriteAllText(filePath, www.downloadHandler.text);

        //Convert txt file to json file
        string json = File.ReadAllText(filePath);
        FileStream fileStream = new FileStream (filePath, FileMode.Create);
        using (StreamWriter writer = new StreamWriter (fileStream)) {
            writer.Write (json);
        }
        filePath = Directory.GetCurrentDirectory() + "/Assets/Screenshots/" + LevelName + "-Data.json";

        AssetDatabase.Refresh();
        yield return new WaitForSeconds(1f);
        GameObject.Find("LevelManager").GetComponent<SaveHandler>().OnLoad(filePath);
        yield return new WaitForSeconds(2f);
        GameObject.Find("Canvas/GameplayScreen/DownloadScreen").SetActive(false);
    }
}
