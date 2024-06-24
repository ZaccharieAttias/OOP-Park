using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ChapterTwoLevelThree : MonoBehaviour
{
    public void Start()
    {
        string OriginalFolderPath = Path.Combine(Application.dataPath, "Resources/Test", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        string PlayFolderPath = Path.Combine(Application.dataPath, "Resources/CharactersData", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);


        if (Directory.Exists(PlayFolderPath))
        {
            Directory.Delete(PlayFolderPath, true);
        }
        Directory.CreateDirectory(PlayFolderPath);


        // Traverse the files in FolderPath/Original and hold their names and content of each file
        string[] files = Directory.GetFiles(OriginalFolderPath);
        foreach (string file in files)
        {
            if (file.Contains(".meta"))
            {
                continue;
            }
            string fileName = Path.GetFileName(file);
            string fileContent = File.ReadAllText(file);


            File.WriteAllText(PlayFolderPath + "/" + fileName, fileContent);
        }
    }
}