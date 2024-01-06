using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTreeManager : MonoBehaviour
{
    private readonly string _imagePath = "Imports/Characters/3/Idle/Idle (1).png";
    private readonly string _buttonPrefabPath = "Prefabs/Buttons/Character";

    private GameObject _buttonPrefab;
    
    private CharacterManager _characterManager;

    public TreeBuilder _treeBuilder;
    
    private float _leftBorder;
    private float _rightBorder;
    private float _upBorder;
    private float _downBorder;
    private float _horizontalArea;
    private float _verticalArea;
    private float _verticalSpacing;

    public void InitializeButtonTreeManager(Character root, CharacterManager characterManager)
    {
        _buttonPrefab = Resources.Load<GameObject>(_buttonPrefabPath);

        _leftBorder = -300f;
        _rightBorder = 100f;
        _upBorder = 50f;
        _downBorder = -200f;
        _verticalSpacing = 0;
        _horizontalArea = _rightBorder - _leftBorder;
        _verticalArea = _upBorder - _downBorder;
        _treeBuilder = gameObject.AddComponent<TreeBuilder>();
        _treeBuilder.SetRoot(root);
        _characterManager = characterManager;
    }

    public void CreateButton(Character characterNode)
    {
        GameObject newPlayerButton = Instantiate(_buttonPrefab, transform);
        newPlayerButton.tag = "CharacterButton";
        newPlayerButton.name = characterNode.name;

        newPlayerButton.AddComponent<CharacterDetails>().InitializeCharacter(characterNode);
        characterNode.SetCharacterButton(newPlayerButton);
        newPlayerButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
        newPlayerButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
        newPlayerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        newPlayerButton.GetComponent<Button>().onClick.RemoveAllListeners();
        newPlayerButton.GetComponent<Button>().onClick.AddListener(() => _characterManager.DisplayCharacterDetails(characterNode.name));
        
        
        string filePath = Path.Combine(Application.dataPath, _imagePath);
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            newPlayerButton.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
        _treeBuilder.BuildTree();
        gameObject.transform.parent.GetComponent<TreeFocus>().SetTargetItem(GetRootButton().GetComponent<RectTransform>());
    }

    public GameObject GetRootButton()
    {
        return _treeBuilder.GetRootCharacterButton();
    }
}
