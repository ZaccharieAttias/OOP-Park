using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;

public class ButtonListenerManager : MonoBehaviour
{
    private List<GameObject> _characterGameObjects;
    private List<GameObject> _duplicateCharacterGameObjects = new List<GameObject>();
    private List<Character> selectedCharacters = new List<Character>();
    private GameObject editButton;
    private CharacterManager characterManager;

    public void Start()
    {
        editButton = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/TreePanel/Edit");
        editButton.GetComponent<Button>().onClick.AddListener(SelectAncestors);
        characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }

    public void SelectAncestors()
    {
        _characterGameObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();

        ChangeButtonInteractable();
        BuildDuplicate();
    }

    private void ChangeButtonInteractable()
    {
        foreach (GameObject characterGameObject in _characterGameObjects)
        {
            Button button = characterGameObject.GetComponent<Button>();
            button.interactable = !button.interactable;
        }
    }

    private void BuildDuplicate()
    {
        _duplicateCharacterGameObjects.Clear(); // Clear the list before rebuilding

        foreach (GameObject characterGameObject in _characterGameObjects)
        {
            GameObject duplicateGameObject = Instantiate(characterGameObject, characterGameObject.transform.parent);
            duplicateGameObject.GetComponent<RectTransform>().sizeDelta = characterGameObject.GetComponent<RectTransform>().sizeDelta;
            duplicateGameObject.transform.localScale = new Vector3(1, 1, 1);
            duplicateGameObject.GetComponent<Button>().interactable = true;


            duplicateGameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            duplicateGameObject.GetComponent<Button>().onClick.AddListener(ButtonSelection);
            _duplicateCharacterGameObjects.Add(duplicateGameObject);
        }
    }

    private void DestroyDuplicate()
    {
        foreach (GameObject duplicateGameObject in _duplicateCharacterGameObjects)
        {
            Destroy(duplicateGameObject);
        }
    }

    private void ButtonSelection()
    {
        GameObject buttonObject = EventSystem.current.currentSelectedGameObject;
        selectedCharacters.Add(buttonObject.GetComponent<Character>());
        buttonObject.GetComponent<Button>().interactable = false;

        
        if (selectedCharacters.Count == 1)
        {
            ChangeButtonInteractable();

            // foreach (Character character in selectedCharacters)
            // {
            //     GameObject buttonObject = _characterGameObjects.Find(button => button.GetComponent<Character>().character == character).gameObject;
            //     buttonObject.GetComponent<Button>().interactable = true;
            // }
            characterManager.AddCharacter(selectedCharacters);
            DestroyDuplicate();
            selectedCharacters.Clear();
        }
    }
}