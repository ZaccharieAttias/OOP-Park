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
        editButton = GameObject.Find("Canvas/HT Menu/Menu/Characters/Tree/TreePanel/Edit");
        editButton.GetComponent<Button>().onClick.AddListener(SelectAncestors);
        characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }

    public void SelectAncestors()
    {
        _characterGameObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();

        ChangeButtonInteractable();
        BuildDuplicate();

        if (selectedCharacters.Count == 1)
        {
            ChangeButtonInteractable();

            foreach (Character character in selectedCharacters)
            {
                GameObject buttonObject = _characterGameObjects.Find(button => button.GetComponent<TreeNode>().character == character).gameObject;
                buttonObject.GetComponent<Button>().interactable = true;
            }
            characterManager.AddCharacter(selectedCharacters);

            DestroyDuplicate();
        }
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
            Debug.Log(characterGameObject.GetComponent<TreeNode>().character.name);
            GameObject duplicateGameObject = Instantiate(characterGameObject);
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
        selectedCharacters.Add(buttonObject.GetComponent<TreeNode>().character);
        buttonObject.GetComponent<Button>().interactable = false;
    }
}
