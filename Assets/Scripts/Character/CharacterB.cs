using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CharacterB
{
    public bool IsOriginal;
    public bool IsAbstract;

    public string Name;
    public string Description;

    public List<Attribute> Attributes;
    public List<Method> Methods;

    public SpecialAbility SpecialAbility;
    public UpcastMethod UpcastMethod;

    public CharacterB Parent;
    public List<CharacterB> Childrens;

    public CharacterButton CharacterButton;

    public CharacterB()
    {
        IsOriginal = false;
        IsAbstract = false;

        Name = "Default";
        Description = "Default";

        Attributes = new List<Attribute>();
        Methods = new List<Method>();

        SpecialAbility = null;
        UpcastMethod = null;

        Parent = null;
        Childrens = new List<CharacterB>();

        CharacterButton = new CharacterButton();

    }
    public CharacterB(string name, string description, CharacterB parent, SpecialAbility specialAbility, bool isOriginal, bool isAbstract)
    {
        IsOriginal = isOriginal;
        IsAbstract = isAbstract;

        Name = name;
        Description = description;

        Attributes = new List<Attribute>();
        Methods = new List<Method>();

        SpecialAbility = specialAbility;
        UpcastMethod = null;

        Parent = parent;
        Childrens = new List<CharacterB>();

        CharacterButton = new CharacterButton();

        if (RestrictionManager.Instance.AllowBeginnerInheritance) PreDetails();
    }
    public void InitializeCharacter(CharacterB character)
    {
        IsOriginal = character.IsOriginal;
        IsAbstract = character.IsAbstract;

        Name = character.Name;
        Description = character.Description;

        Attributes = character.Attributes;
        Methods = character.Methods;

        SpecialAbility = character.SpecialAbility;
        UpcastMethod = character.UpcastMethod;

        Parent = character.Parent;
        Childrens = character.Childrens;

        CharacterButton = character.CharacterButton;
    }
    public void PreDetails()
    {   
        foreach (var attribute in Parent?.Attributes)
            if (!Attributes.Any(item => item.Name == attribute.Name))
                if (RestrictionManager.Instance.AllowAccessModifier is false || attribute.AccessModifier is not AccessModifier.Private)
                    Attributes.Add(attribute);

        foreach (var method in Parent?.Methods)
            if (!Methods.Any(item => item.Name == method.Name))
                if (RestrictionManager.Instance.AllowAccessModifier is false || method.AccessModifier is not AccessModifier.Private)
                    Methods.Add(method);
    }

    public bool IsRoot() { return Parent is null; }
    public bool IsLeaf() { return Childrens.Count == 0; }

    public bool IsLeftMost() { return IsRoot() || Parent.Childrens[0] == this; }
    public bool IsRightMost() { return IsRoot() || Parent.Childrens[^1] == this; }

    public CharacterB GetLeftMostSibling() { return IsRoot() ? null : Parent.Childrens[0]; }
    public CharacterB GetLeftMostChild() { return IsLeaf() ? null : Childrens[0]; }

    public CharacterB GetRightMostSibling() { return IsRoot() ? null : Parent.Childrens[^1]; }
    public CharacterB GetRightMostChild() { return IsLeaf() ? null : Childrens[^1]; }

    public CharacterB GetPreviousSibling() { return IsLeftMost() ? null : Parent.Childrens[Parent.Childrens.IndexOf(this) - 1]; }
    public CharacterB GetNextSibling() { return IsRightMost() ? null : Parent.Childrens[Parent.Childrens.IndexOf(this) + 1]; }

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
