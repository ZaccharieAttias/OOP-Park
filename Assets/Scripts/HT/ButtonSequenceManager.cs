using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class ButtonListenerManager : MonoBehaviour
{
    private List<Button> characterButtons; // Reference to character buttons

    private Dictionary<Button, List<ButtonClickedListener>> originalListeners = new Dictionary<Button, List<ButtonClickedListener>>();

    private List<Character> selectedCharacters = new List<Character>();

    public CharacterManager characterManager;


    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(SelectAncestors());
    }

    public void SelectAncestors()
    {
        characterButtons = GameObject.FindGameObjectsWithTag("CharacterButton")
                           .Select(obj => obj.GetComponent<Button>())
                           .ToList();
        
        StoreOriginalListeners();
        UpdateListeners();

        if (selectedCharacters.Count == 1)
        {
            RevertListeners();
            //selecedCharacters make interactable
            foreach (Character character in selectedCharacters)
            {
                GameObject buttonObject = characterButtons.Find(button => button.GetComponent<TreeNode>().character == character).gameObject;
                buttonObject.GetComponent<Button>().interactable = true;
            }
            characterManager.AddCharacter(selectedCharacters);
        }


    }

    private void StoreOriginalListeners()
    {
        foreach (Button button in characterButtons)
        {
            List<ButtonClickedListener> listenerList = button.GetComponents<ButtonClickedListener>().ToList();
            originalListeners.Add(button, listenerList);
        }
    }

    private void UpdateListeners()
    {
        foreach (Button button in characterButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ButtonSelection);
        }
    }

    private void RevertListeners()
    {
        foreach (Button button in characterButtons)
        {
            button.onClick.RemoveAllListeners();
            List<ButtonClickedListener> originalListenerList = originalListeners[button];
            foreach (ButtonClickedListener listener in originalListenerList)
            {
                button.onClick.AddListener(listener.OnButtonClick);
            }
        }
    }

    private void ButtonSelection()
    {
        GameObject buttonObject = EventSystem.current.currentSelectedGameObject;
        selectedCharacters.Add(buttonObject.GetComponent<TreeNode>().character);
        buttonObject.GetComponent<Button>().interactable = false;
    }
}











