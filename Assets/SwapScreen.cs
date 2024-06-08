using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapScreen : MonoBehaviour
{
    public Button SwapButton;
    public CharacterEditor1 CharacterEditor;

    public void Start()
    {
        CharacterEditor = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        SwapButton = GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>();
        SwapButton.onClick.AddListener(() => CharacterEditor.LoadFromJson());
    }
}
