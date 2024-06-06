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

    public List<CharacterB> Parents;
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

        Parents = new List<CharacterB>();
        Childrens = new List<CharacterB>();

        CharacterButton = new CharacterButton();

    }
    public CharacterB(string name, string description, List<CharacterB> parents, SpecialAbility specialAbility, bool isOriginal, bool isAbstract)
    {
        IsOriginal = isOriginal;
        IsAbstract = isAbstract;

        Name = name;
        Description = description;

        Attributes = new List<Attribute>();
        Methods = new List<Method>();

        SpecialAbility = specialAbility;
        UpcastMethod = null;

        Parents = new List<CharacterB>(parents);
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

        Parents = character.Parents;
        Childrens = character.Childrens;

        CharacterButton = character.CharacterButton;
    }
    public void PreDetails()
    {
        foreach (CharacterB parent in Parents)
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

    public CharacterB GetLeftMostSibling() { return IsRoot() ? null : Parents[0].Childrens[0]; }
    public CharacterB GetLeftMostChild() { return IsLeaf() ? null : Childrens[0]; }

    public CharacterB GetRightMostSibling() { return IsRoot() ? null : Parents[0].Childrens[^1]; }
    public CharacterB GetRightMostChild() { return IsLeaf() ? null : Childrens[^1]; }

    public CharacterB GetPreviousSibling() { return IsLeftMost() ? null : Parents[0].Childrens[Parents[0].Childrens.IndexOf(this) - 1]; }
    public CharacterB GetNextSibling() { return IsRightMost() ? null : Parents[0].Childrens[Parents[0].Childrens.IndexOf(this) + 1]; }

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
