using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;


[Serializable]
public class Character
{
    public bool IsOriginal { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public List<Character> Parents;
    public List<Character> Childrens;
    public List<CharacterAttribute> Attributes;
    public List<CharacterMethod> Methods;

    public int X { get; set; }
    public int Y { get; set; }
    public int Mod { get; set; }
    public int Depth { get; set; }

    public GameObject CharacterButton { get; set; }


    public Character(string name, string description, List<Character> parents, bool isOriginal)
    {
        IsOriginal = isOriginal;

        Name = name;
        Description = description;

        Parents = new List<Character>(parents);
        Childrens = new List<Character>();
        Attributes = new List<CharacterAttribute>();
        Methods = new List<CharacterMethod>();

        X = 0;
        Y = 0;
        Mod = 0;
        Depth = 0;

        CharacterButton = null;

        PreDetails();
    }

    private void PreDetails()
    {
        foreach (Character character in Parents)
        {
            foreach (CharacterAttribute attribute in character.Attributes)
                if (attribute.accessModifier != AccessModifier.Private)
                    if (Attributes.Any(item => item.name == attribute.name) == false)
                        Attributes.Add(new CharacterAttribute(attribute.name, attribute.description, attribute.accessModifier));

            foreach (CharacterMethod method in character.Methods)
                if (method.accessModifier != AccessModifier.Private)
                    if (Methods.Any(item => item.name == method.name) == false)
                        Methods.Add(new CharacterMethod(method.name, method.description, method.accessModifier));
        }
    }

    public void InitializeCharacter(Character character)
    {
        IsOriginal = character.IsOriginal;

        Name = character.Name;
        Description = character.Description;

        Parents = character.Parents;
        Childrens = character.Childrens;
        Attributes = character.Attributes;
        Methods = character.Methods;

        X = character.X;
        Y = character.Y;
        Mod = character.Mod;
        Depth = character.Depth;

        CharacterButton = character.CharacterButton;
    }

    public void Dispose()
    {
        // Removing himself from its parnents

        foreach (Character parent in Parents)
        {
            parent.Childrens.Remove(this);
        }
    }


    public bool IsLeaf() { return Childrens.Count == 0; }

    public bool IsLeftMost()
    {
        if (Parents.Count == 0) return true;

        return Parents[0].Childrens[0] == this;
    }

    public bool IsRightMost()
    {
        if (Parents.Count == 0) return true;

        return Parents[0].Childrens[^1] == this;
    }

    public Character GetPreviousSibling()
    {
        if (Parents.Count == 0 || IsLeftMost()) return null;

        return Parents[0].Childrens[Parents[0].Childrens.IndexOf(this) - 1];
    }

    public Character GetNextSibling()
    {
        if (Parents.Count == 0 || IsRightMost()) return null;

        return Parents[0].Childrens[Parents[0].Childrens.IndexOf(this) + 1];
    }

    public Character GetLeftMostSibling()
    {
        if (Parents.Count == 0) return null;
        if (IsLeftMost()) return this;

        return Parents[0].Childrens[0];
    }

    public Character GetLeftMostChild()
    {
        if (Childrens.Count == 0) return null;

        return Childrens[0];
    }

    public Character GetRightMostChild()
    {
        if (Childrens.Count == 0) return null;

        return Childrens[^1];
    }

    public void SetTransformPositionX(float x)
    {
        CharacterButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, CharacterButton.GetComponent<RectTransform>().anchoredPosition.y);
    }

    public void SetTransformPositionY(float y)
    {
        CharacterButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(CharacterButton.GetComponent<RectTransform>().anchoredPosition.x, y);
    }
}