using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;


public class MethodsPopupManager : MonoBehaviour
{
    public List<CharacterMethod> allMethods = new List<CharacterMethod>();
    private Character currentCharacter;

    public Transform methodsPanel;

    public GameObject buttonPrefab;

    public CharacterManager characterManager;



    public void ShowMethodsPopup(Character currentChar)
    {
        ClearMethodsPanel();

        currentCharacter = currentChar;

        foreach (CharacterMethod method in allMethods)
        {
            GameObject methodButton = Instantiate(buttonPrefab, methodsPanel);
            methodButton.name = method.name;

            TMP_Text buttonText = methodButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.name;

            MarkMethodInPopup(methodButton, method);
        }

        gameObject.SetActive(true);
    }

    private void MarkMethodInPopup(GameObject methodButton, CharacterMethod method)
    {
        bool hasMethod = currentCharacter.methods.Any(m => m.name == method.name);

        Image image = methodButton.GetComponent<Image>();
        image.color = hasMethod ? Color.green : Color.white;

        Button button = methodButton.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnMethodButtonClick(method, hasMethod));
    }

    private void OnMethodButtonClick(CharacterMethod method, bool hasMethod)
    {
        if (hasMethod)
        {
            currentCharacter.methods.Remove(currentCharacter.methods.Find(item => item.name == method.name));
        }

        else
        {
            currentCharacter.methods.Add(new CharacterMethod(method.name, method.description, method.accessModifier));
        }

        characterManager.DisplayCharacterDetails(currentCharacter.name);

        ShowMethodsPopup(currentCharacter);
    }

    private void ClearMethodsPanel()
    {
        foreach (Transform child in methodsPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
