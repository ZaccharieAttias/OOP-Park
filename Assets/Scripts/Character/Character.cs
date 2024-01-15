using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public bool IsOriginal;
    public bool HasBeenNamed;

    public string Name;
    public string Description;

    public List<CharacterAttribute> Attributes;
    public List<CharacterMethod> Methods;

    public List<Character> Parents;
    public List<Character> Childrens;

    public CharacterButton CharacterButton;


    public Character(string name, string description, List<Character> parents, bool isOriginal, bool hasBeenNamed)
    {
        IsOriginal = isOriginal;
        HasBeenNamed = hasBeenNamed;

        Name = name;
        Description = description;

        Attributes = new List<CharacterAttribute>();
        Methods = new List<CharacterMethod>();

        Parents = new List<Character>(parents);
        Childrens = new List<Character>();

        CharacterButton = new CharacterButton();
    }

    public Character(string name, string description, List<Character> parents, List<Character> childrens, List<CharacterAttribute> attributes, List<CharacterMethod> methods, bool isOriginal, CharacterButton characterButton, bool hasBeenNamed)
    {
        IsOriginal = isOriginal;
        HasBeenNamed = hasBeenNamed;

        Name = name;
        Description = description;

        Attributes = new List<CharacterAttribute>(attributes);
        Methods = new List<CharacterMethod>(methods);

        Parents = new List<Character>(parents);
        Childrens = new List<Character>(childrens);

        CharacterButton = characterButton;
    }

    public void InitializeCharacter(Character character)
    {
        IsOriginal = character.IsOriginal;
        HasBeenNamed = character.HasBeenNamed;

        Name = character.Name;
        Description = character.Description;

        Attributes = character.Attributes;
        Methods = character.Methods;

        Parents = character.Parents;
        Childrens = character.Childrens;

        CharacterButton = character.CharacterButton;
    }

    public bool IsRoot() { return Parents.Count == 0; }
    public bool IsLeaf() { return Childrens.Count == 0; }

    public bool IsLeftMost() { return IsRoot() || Parents[0].Childrens[0] == this; }
    public bool IsRightMost() { return IsRoot() || Parents[0].Childrens[^1] == this; }

    public Character GetLeftMostSibling() { return IsRoot() ? null : Parents[0].Childrens[0]; }
    public Character GetLeftMostChild() { return IsLeaf() ? null : Childrens[0]; }

    public Character GetRightMostSibling() { return IsRoot() ? null : Parents[0].Childrens[^1]; }
    public Character GetRightMostChild() { return IsLeaf() ? null : Childrens[^1]; }

    public Character GetPreviousSibling() { return IsLeftMost() ? null : Parents[0].Childrens[Parents[0].Childrens.IndexOf(this) - 1]; }
    public Character GetNextSibling() { return IsRightMost() ? null : Parents[0].Childrens[Parents[0].Childrens.IndexOf(this) + 1]; }

    public void SetTransformPositionX(float x)
    {
        RectTransform rectTransform = CharacterButton.Button.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
    }
    public void SetTransformPositionY(float y)
    {
        RectTransform rectTransform = CharacterButton.Button.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
    }
}
