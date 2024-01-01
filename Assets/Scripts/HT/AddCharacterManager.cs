using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddCharacterManager : MonoBehaviour
{
    private readonly string _editButtonPath = "Canvas/HTMenu/Menu/Characters/Tree/Buttons/Edit";
    
    private GameObject _editButton;
    
    private List<GameObject> _gameObjects;
    private List<GameObject> _duplicateGameObjects;
    
    private List<Character> _selectedGameObjects;
    private CharacterManager _characterManager;


    private void Start()
    {
        _editButton = GameObject.Find(_editButtonPath);
        _editButton.GetComponent<Button>().onClick.AddListener(() => SelectParents());

        _gameObjects = new List<GameObject>();
        _duplicateGameObjects = new List<GameObject>();
        _selectedGameObjects = new List<Character>();

        _characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }

    private void SelectParents()
    {
        _editButton.GetComponent<Button>().interactable = false;
        _gameObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();

        ChangeButtonInteractable();
        BuildDuplicate();
    }

    private void ChangeButtonInteractable()
    {
        foreach (GameObject characterGameObject in _gameObjects)
        {
            //characterGameObject.GetComponent<LayoutElement>().ignoreLayout = false;
            Button button = characterGameObject.GetComponent<Button>();
            button.interactable = !button.interactable;
        }
    }

    private void BuildDuplicate()
    {
        _duplicateGameObjects.Clear();

        foreach (GameObject characterGameObject in _gameObjects)
        {
            GameObject duplicateGameObject = Instantiate(characterGameObject, characterGameObject.transform.parent);
            //characterGameObject.GetComponent<LayoutElement>().ignoreLayout = true;
            duplicateGameObject.GetComponent<RectTransform>().sizeDelta = characterGameObject.GetComponent<RectTransform>().sizeDelta;
            duplicateGameObject.transform.localScale = new Vector3(1, 1, 1);
            duplicateGameObject.GetComponent<Button>().interactable = true;

            duplicateGameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            duplicateGameObject.GetComponent<Button>().onClick.AddListener(() => GameObjectClicked());
            
            _duplicateGameObjects.Add(duplicateGameObject);
        }
    }

    private void GameObjectClicked()
    {
        GameObject buttonObject = EventSystem.current.currentSelectedGameObject;
        buttonObject.GetComponent<Button>().interactable = false;

        _selectedGameObjects.Add(buttonObject.GetComponent<CharacterDetails>().GetCurrentCharacter());

        if (_selectedGameObjects.Count == 1)
        {
            _characterManager.AddCharacter(_selectedGameObjects);

            DestroyDuplicate();
            ChangeButtonInteractable();
            
            _selectedGameObjects.Clear();
            _editButton.GetComponent<Button>().interactable = true;
        }
    }

    private void DestroyDuplicate()
    {
        foreach (GameObject duplicateGameObject in _duplicateGameObjects)
            Destroy(duplicateGameObject);
    }
}
