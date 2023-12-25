using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;


public class ButtonTreeManager : MonoBehaviour
{
    private readonly string _buttonPrefabPath = "Prefabs/Buttons/Character";
    public Character root;
    public GameObject buttonPrefab;
    public CharacterManager characterManager;
    public string imagePath = "Imports/Characters/3/Idle/Idle (1).png";

    private float leftBorder = -300f;
    private float rightBorder = 100f;
    private float upBorder = 50f;
    private float downBorder = -200f;
    private float xArea;
    private float yArea;
    private float verticalSpacing;

    public void Start()
    {
        xArea = rightBorder - leftBorder;
        yArea = upBorder - downBorder;
    }
    

    public void startButtonTreeManager(Character root, CharacterManager characterManager)
    {
        this.root = root;
        this.characterManager = characterManager;
        buttonPrefab = Resources.Load<GameObject>(_buttonPrefabPath);
    }

    public void CreateButton(Character characterNode)
    {
        GameObject newPlayerButton = Instantiate(buttonPrefab, transform);
        newPlayerButton.tag = "CharacterButton";

        Character newPlayerScript = newPlayerButton.AddComponent<Character>();
        newPlayerScript = characterNode;
        newPlayerScript.name = characterNode.name;
        
        newPlayerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        newPlayerButton.transform.localScale = new Vector3(1, 1, 1);
        newPlayerButton.name = characterNode.name;
        newPlayerButton.GetComponent<Button>().onClick.RemoveAllListeners();
        newPlayerButton.GetComponent<Button>().onClick.AddListener(() => characterManager.DisplayCharacterDetails(characterNode.name));
        RectTransform rectTransform = newPlayerButton.GetComponent<RectTransform>();
        string filePath = Path.Combine(Application.dataPath, imagePath);
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Load image data into the texture
            newPlayerButton.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }







        Dictionary<int, List<GameObject>> depthObjects = CalculatePosition();
        foreach (KeyValuePair<int, List<GameObject>> depthObject in depthObjects)
        {
            UpdateTreeLayout(depthObject.Key, depthObject.Value);
        }
    }





    public void UpdateTreeLayout(int depth, List<GameObject> objects)
    {
        if (depth == 0)
        {
            objects[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-147f, 50f, 0f);
            return;
        }
        
        int nbrOfObjects = objects.Count;
        float horizontalSpacing = xArea / nbrOfObjects, horizontalPosition, verticalPosition;

        for (int i = 0; i < nbrOfObjects; i++)
        {
            // Add an offset to prevent objects from overlapping
            horizontalPosition = leftBorder + (i * horizontalSpacing);
            Debug.Log("horizontalPosition: " + horizontalPosition);
            verticalPosition = upBorder - (depth * verticalSpacing);
            Debug.Log("verticalPosition: " + verticalPosition);
            objects[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(horizontalPosition, verticalPosition, 0f);
        }
    }



















    public Dictionary<int, List<GameObject>> CalculatePosition()
    {
        Dictionary<int, List<GameObject>> depthObjects = new Dictionary<int, List<GameObject>>();

        List<GameObject> allObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();


        foreach (GameObject obj in allObjects)
        {
            int depth = obj.GetComponent<Character>().depth;

            if (depthObjects.ContainsKey(depth))
            {
                depthObjects[depth].Add(obj);
            }
            else
            {
                List<GameObject> newList = new List<GameObject> { obj };
                depthObjects.Add(depth, newList);
            }
        }

        verticalSpacing = yArea / depthObjects.Count;
        return depthObjects;

        
    }


}
