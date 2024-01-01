using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddCharacterManager : MonoBehaviour
{
    private readonly string _buttonPrefabPath = "Prefabs/Buttons/Character";
    private readonly string _editButtonPath = "Canvas/HTMenu/Menu/Characters/Tree/Buttons/Edit";

    private GameObject _buttonPrefab;
    private GameObject _confirmButton;
    private GameObject _cancelButton;
    private GameObject _editButton;

    private List<GameObject> _gameObjects;
    private List<GameObject> _duplicateGameObjects;

    private List<Character> _selectedGameObjects;
    private CharacterManager _characterManager;

    private void Start()
    {
        _buttonPrefab = Resources.Load<GameObject>(_buttonPrefabPath);

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

        SetConfirmationButton();
        SetCancelButton();

        _gameObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();

        ChangeButtonInteractable();
        BuildDuplicate();
    }

    private void SetCancelButton()
    {
        GameObject cancelButton = Instantiate(_buttonPrefab, _editButton.transform.parent);
        cancelButton.name = "Cancel";
        cancelButton.GetComponent<RectTransform>().sizeDelta = _editButton
            .GetComponent<RectTransform>()
            .sizeDelta;
        cancelButton.transform.localPosition = new Vector3(
            _editButton.transform.localPosition.x + 50,
            _editButton.transform.localPosition.y,
            _editButton.transform.localPosition.z
        );

        cancelButton.GetComponent<Button>().onClick.AddListener(() => CancelButtonClicked());

        _cancelButton = cancelButton;
    }

    private void CancelButtonClicked()
    {
        DestroyDuplicate();
        ChangeButtonInteractable();

        _selectedGameObjects.Clear();
        _editButton.GetComponent<Button>().interactable = true;

        Destroy(EventSystem.current.currentSelectedGameObject);
        Destroy(_confirmButton);
    }

    private void SetConfirmationButton()
    {
        GameObject confirmButton = Instantiate(_buttonPrefab, _editButton.transform.parent);
        confirmButton.name = "Confirm";
        confirmButton.GetComponent<RectTransform>().sizeDelta = _editButton
            .GetComponent<RectTransform>()
            .sizeDelta;
        confirmButton.transform.localPosition = new Vector3(
            _editButton.transform.localPosition.x - 50,
            _editButton.transform.localPosition.y,
            _editButton.transform.localPosition.z
        );

        confirmButton.GetComponent<Button>().onClick.AddListener(() => ConfirmButtonClicked());

        _confirmButton = confirmButton;
        _confirmButton.GetComponent<Button>().interactable = false;
    }

    private void ConfirmButtonClicked()
    {
        _characterManager.AddCharacter(_selectedGameObjects);

        DestroyDuplicate();
        ChangeButtonInteractable();

        _selectedGameObjects.Clear();
        _editButton.GetComponent<Button>().interactable = true;

        Destroy(EventSystem.current.currentSelectedGameObject);
        Destroy(_cancelButton);
    }

    private void ChangeButtonInteractable()
    {
        foreach (GameObject characterGameObject in _gameObjects)
        {
            Button button = characterGameObject.GetComponent<Button>();
            button.interactable = !button.interactable;
        }
    }

    private void BuildDuplicate()
    {
        _duplicateGameObjects.Clear();

        foreach (GameObject characterGameObject in _gameObjects)
        {
            GameObject duplicateGameObject = Instantiate(
                characterGameObject,
                characterGameObject.transform.parent
            );
            duplicateGameObject.GetComponent<RectTransform>().sizeDelta = characterGameObject
                .GetComponent<RectTransform>()
                .sizeDelta;
            duplicateGameObject.transform.localScale = new Vector3(1, 1, 1);
            duplicateGameObject.GetComponent<Button>().interactable = true;

            duplicateGameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            duplicateGameObject
                .GetComponent<Button>()
                .onClick.AddListener(() => GameObjectClicked());

            _duplicateGameObjects.Add(duplicateGameObject);
        }
    }

    private void GameObjectClicked()
    {
        GameObject buttonObject = EventSystem.current.currentSelectedGameObject;

        if (
            _selectedGameObjects.Contains(
                buttonObject.GetComponent<CharacterDetails>().GetCurrentCharacter()
            )
        )
        {
            _selectedGameObjects.Remove(
                buttonObject.GetComponent<CharacterDetails>().GetCurrentCharacter()
            );
        }
        else
        {
            _selectedGameObjects.Add(
                buttonObject.GetComponent<CharacterDetails>().GetCurrentCharacter()
            );
        }

        ChangeClickableGameObjects();

        if (_selectedGameObjects.Count > 0)
            _confirmButton.GetComponent<Button>().interactable = true;
        else
            _confirmButton.GetComponent<Button>().interactable = false;
    }

    private void ChangeClickableGameObjects()
    {
        // Reset all characters to default interactable and color
        foreach (GameObject characterGameObject in _duplicateGameObjects)
        {
            Button button = characterGameObject.GetComponent<Button>();
            button.interactable = true;
            characterGameObject.GetComponent<Image>().color = Color.white;
        }

        // Iterate through the selected characters
        foreach (Character selectedCharacter in _selectedGameObjects)
        {
            // Set Color green for the selected character
            GameObject selectedGameObject = _duplicateGameObjects.Find(
                obj =>
                    obj.GetComponent<CharacterDetails>().GetCurrentCharacter().name
                    == selectedCharacter.name
            );
            selectedGameObject.GetComponent<Image>().color = Color.green;

            // Set color and interactability for parents
            foreach (Character parent in selectedCharacter.parents)
            {
                GameObject parentGameObject = _duplicateGameObjects.Find(
                    obj =>
                        obj.GetComponent<CharacterDetails>().GetCurrentCharacter().name
                        == parent.name
                );

                if (parentGameObject != null)
                {
                    parentGameObject.GetComponent<Button>().interactable = false;
                    parentGameObject.GetComponent<Image>().color = Color.black;
                }
            }

            // Set color and interactability for children
            foreach (Character child in selectedCharacter.childrens)
            {
                GameObject childGameObject = _duplicateGameObjects.Find(
                    obj =>
                        obj.GetComponent<CharacterDetails>().GetCurrentCharacter().name
                        == child.name
                );

                if (childGameObject != null)
                {
                    childGameObject.GetComponent<Button>().interactable = false;
                    childGameObject.GetComponent<Image>().color = Color.black;
                }
            }
        }
    }

    private void DestroyDuplicate()
    {
        foreach (GameObject duplicateGameObject in _duplicateGameObjects)
            Destroy(duplicateGameObject);
    }
}
