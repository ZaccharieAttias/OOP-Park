using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;
using UnityEngine.EventSystems;


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

        
        //Character newPlayerScript = newPlayerButton.AddComponent<Character>();
        //newPlayerScript.InitializeCharacter(characterNode);

        newPlayerButton.AddComponent<CharacterDetails>().InitializeCharacter(characterNode);
        newPlayerButton.GetComponent<CharacterDetails>().character.description = "Gottcha Bitch";




        // if (characterManager._charactersCollection.Count == 2)
        // {
        //     Debug.Log("newCharacter: " + characterManager._charactersCollection[characterManager._charactersCollection.Count-1].ancestors[0].childrens[0].name);
        //     Debug.Log("newCharacter: " + characterManager._charactersCollection[0].childrens[0].name);
        // }

        
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

        DrawLines();
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
            //Debug.Log("horizontalPosition: " + horizontalPosition);
            verticalPosition = upBorder - (depth * verticalSpacing);
            //Debug.Log("verticalPosition: " + verticalPosition);
            objects[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(horizontalPosition, verticalPosition, 0f);
        }
    }

    public Dictionary<int, List<GameObject>> CalculatePosition()
    {
        Dictionary<int, List<GameObject>> depthObjects = new Dictionary<int, List<GameObject>>();
        List<GameObject> allObjects = characterManager.GetCurrentCollection();

        foreach (GameObject obj in allObjects)
        {
            //Debug.Log("name: " + obj.GetComponent<Character>().name + "depth: " + obj.GetComponent<Character>().depth);
            int depth = obj.GetComponent<CharacterDetails>().character.depth;

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

    public void DrawLines()
    {
        List<GameObject> allObjects = characterManager.GetCurrentCollection();

        //foreach (GameObject obj in allObjects)
        //{
        //    Character character = obj.GetComponent<Character>();
        //    Transform myTransform = obj.transform;
        //    foreach(Character child in character.childrens)
        //    {
        //        Debug.Log("child: " + child.name);
        //        string childNameToFind = child.name;
        //        Transform childTransform = myTransform.Find(childNameToFind);
        //        GameObject childObject = childTransform.gameObject;
        //        Debug.Log("childObject: " + childObject.name);

        //        LineRenderer lineRenderer = childObject.AddComponent<LineRenderer>();
        //        lineRenderer.positionCount = 2;
        //        lineRenderer.SetPosition(0, obj.transform.position);
        //        lineRenderer.SetPosition(1, childObject.transform.position);
        //        lineRenderer.material.color = Color.red;
        //    }
        //}


        /*
        Transform myTransform = newPlayerButton.transform;
        Debug.Log(myTransform.name);
        Transform parentTransform = myTransform.parent;
        Debug.Log(parentTransform.name);

        string siblingNameToFind = newCharacter.ancestors[0].name;

        Transform siblingTransform = parentTransform.Find(siblingNameToFind);  
        Debug.Log(siblingTransform.name);
        
        GameObject foundObject = siblingTransform.gameObject;



        LineRenderer lineRenderer = foundObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, foundObject.transform.position);
        lineRenderer.SetPosition(1, newPlayerButton.transform.position);
        lineRenderer.material.color = Color.red;
        */
    }
}
