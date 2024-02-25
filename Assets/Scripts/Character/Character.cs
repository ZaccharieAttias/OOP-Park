using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Character
{
    public bool IsOriginal;
    public bool IsAbstract;
    
    public string Name;
    public string Description;

    public List<Attribute> Attributes;
    public List<Method> Methods;

    public SpecialAbility SpecialAbility;
    public UpcastMethod UpcastMethod;

    public List<Character> Parents;
    public List<Character> Childrens;

    public CharacterButton CharacterButton;


    public Character()
    {
        IsOriginal = false;
        IsAbstract = false;

        Name = "Default";
        Description = "Default";

        Attributes = new List<Attribute>();
        Methods = new List<Method>();

        SpecialAbility = null;
        UpcastMethod = null;

        Parents = new List<Character>();
        Childrens = new List<Character>();

        CharacterButton = new CharacterButton();
    }
    public Character(string name, string description, List<Character> parents, SpecialAbility specialAbility, bool isOriginal, bool isAbstract)
    {
        IsOriginal = isOriginal;
        IsAbstract = isAbstract;

        Name = name;
        Description = description;

        Attributes = new List<Attribute>();
        Methods = new List<Method>();

        SpecialAbility = specialAbility;
        UpcastMethod = null;

        Parents = new List<Character>(parents);
        Childrens = new List<Character>();

        CharacterButton = new CharacterButton();
        
        if (RestrictionManager.Instance.AllowBeginnerInheritance) PreDetails();
    }
    public void InitializeCharacter(Character character)
    {
        IsOriginal = character.IsOriginal;
        IsAbstract = character.IsAbstract;

        Name = character.Name;
        Description = character.Description;

        Attributes = character.Attributes;
        Methods = character.Methods;

        SpecialAbility = character.SpecialAbility;
        UpcastMethod = character.UpcastMethod;

        Parents = character.Parents;
        Childrens = character.Childrens;

        CharacterButton = character.CharacterButton;
    }
    public void PreDetails()
    {
        foreach (Character parent in Parents)
        {
            foreach (Attribute attribute in parent.Attributes)
                if (!Attributes.Any(item => item.Name == attribute.Name))
                    if (RestrictionManager.Instance.AllowAccessModifiers is false || attribute.AccessModifier is not AccessModifier.Private)
                        Attributes.Add(attribute);

            foreach (Method method in parent.Methods)
                if (!Methods.Any(item => item.Name == method.Name))
                    if (RestrictionManager.Instance.AllowAccessModifiers is false || method.AccessModifier is not AccessModifier.Private)
                        Methods.Add(method);
        }
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
