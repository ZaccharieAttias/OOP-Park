using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTreeManager : MonoBehaviour
{
    private readonly string _imagePath = "Imports/Characters/3/Idle/Idle (1).png";
    private readonly string _buttonPrefabPath = "Prefabs/Buttons/Character";

    [SerializeField] private GameObject _lines;

    private GameObject _buttonPrefab;
    
    private Character _root;
    private GameObject _rootButton;
    private CharacterManager _characterManager;

    public LineRenderer _lineRenderer;
    
    private float _leftBorder;
    private float _rightBorder;
    private float _upBorder;
    private float _downBorder;
    private float _horizontalArea;
    private float _verticalArea;
    private float _verticalSpacing;

    private void Start()
    {
        _buttonPrefab = Resources.Load<GameObject>(_buttonPrefabPath);
        _lines = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Lines");

        _leftBorder = -300f;
        _rightBorder = 100f;
        _upBorder = 50f;
        _downBorder = -200f;
        _verticalSpacing = 0;
        _horizontalArea = _rightBorder - _leftBorder;
        _verticalArea = _upBorder - _downBorder;
    }

    public void startButtonTreeManager(Character root, CharacterManager characterManager)
    {
        Start();
        _root = root;
        _characterManager = characterManager;
    }

    public void CreateButton(Character characterNode)
    {
        GameObject newPlayerButton = Instantiate(_buttonPrefab, transform);
        
        newPlayerButton.tag = "CharacterButton";

        newPlayerButton.AddComponent<CharacterDetails>().InitializeCharacter(characterNode);
        newPlayerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        newPlayerButton.transform.localScale = new Vector3(1, 1, 1);
        newPlayerButton.name = characterNode.name;
        newPlayerButton.GetComponent<Button>().onClick.RemoveAllListeners();
        newPlayerButton.GetComponent<Button>().onClick.AddListener(() => _characterManager.DisplayCharacterDetails(characterNode.name));
        RectTransform rectTransform = newPlayerButton.GetComponent<RectTransform>();
        
        string filePath = Path.Combine(Application.dataPath, _imagePath);
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            newPlayerButton.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }

        Dictionary<int, List<GameObject>> depthObjects = CalculatePosition();
        foreach (KeyValuePair<int, List<GameObject>> depthObject in depthObjects)
            UpdateTreeLayout(depthObject.Key, depthObject.Value);

        DrawLines();
    }

    private void UpdateTreeLayout(int depth, List<GameObject> objects)
    {
        if (depth == 0)
        {
            objects[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-147f, 50f, 0f);
            return;
        }

        int objectsCounter = objects.Count;
        float horizontalSpacing = _horizontalArea / objectsCounter;
        float horizontalPosition = 0;
        float verticalPosition = 0;

        for (int i = 0; i < objectsCounter; i++)
        {
            horizontalPosition = _leftBorder + (i * horizontalSpacing);
            verticalPosition = _upBorder - (depth * _verticalSpacing);
            objects[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(horizontalPosition, verticalPosition, 0f);
        }
    }

    private Dictionary<int, List<GameObject>> CalculatePosition()
    {
        Dictionary<int, List<GameObject>> depthObjects = new Dictionary<int, List<GameObject>>();
        List<GameObject> allObjects = _characterManager.GetCurrentCollection();

        foreach (GameObject obj in allObjects)
        {
            int depth = obj.GetComponent<CharacterDetails>().GetCurrentCharacter().depth;

            if (depthObjects.ContainsKey(depth)) depthObjects[depth].Add(obj);
            else
            {
                List<GameObject> newList = new List<GameObject> { obj };
                depthObjects.Add(depth, newList);
            }
        }

        _verticalSpacing = _verticalArea / depthObjects.Count;
        return depthObjects;
    }

    private void DrawLines()
    {
        List<GameObject> allObjects = _characterManager.GetCurrentCollection();

        foreach (Transform child in _lines.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject obj in allObjects)
        {
            Character character = obj.GetComponent<CharacterDetails>().GetCurrentCharacter();
            Transform parentTransform = obj.transform.parent;
            foreach(Character child in character.childrens)
            {
                string childNameToFind = child.name;
                Transform childTransform = parentTransform.Find(childNameToFind);
                GameObject childObject = childTransform.gameObject;

                GameObject temp = new GameObject(obj.name + "to" + child.name);
                temp.transform.SetParent(_lines.transform);
                GameObject line = temp;
                
                line.AddComponent<Image>();
                line.transform.localScale = new Vector3(1, 1, 1);
                line.AddComponent<LinesCreator>();
                line.GetComponent<LinesCreator>().SetPoints(obj.transform, childObject.transform);
                line.GetComponent<LinesCreator>().Settings();
            }
        }
    }
}
