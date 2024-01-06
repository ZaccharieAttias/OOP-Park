using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Character
{
    public string name;
    public string description;

    public List<Character> parents;
    public List<Character> childrens;
    public List<CharacterAttribute> attributes;
    public List<CharacterMethod> methods;

    private int _depth;
    private int _mod = 0;
    private int _x = 0;
    private int _y = 0;

    public bool isOriginal;

    public GameObject CharacterButton { get; set; }


    public Character(string name, string description, List<Character> parents, bool isOriginal)
    {
        this.name = name;
        this.description = description;

        this.parents = new List<Character>();
        this.childrens = new List<Character>();
        this.attributes = new List<CharacterAttribute>();
        this.methods = new List<CharacterMethod>();

        this.isOriginal = isOriginal;

        PreDetails(parents);
    }

    private void PreDetails(List<Character> parents)
    {
        foreach (Character character in parents)
        {
            foreach (CharacterAttribute attribute in character.attributes)
                if (attribute.accessModifier != AccessModifier.Private)
                    if (!(this.attributes.Any(item => item.name == attribute.name)))
                        this.attributes.Add(new CharacterAttribute(attribute.name, attribute.description, attribute.accessModifier));

            foreach (CharacterMethod method in character.methods)
                if (method.accessModifier != AccessModifier.Private)
                    if (!(this.methods.Any(item => item.name == method.name)))
                        this.methods.Add(new CharacterMethod(method.name, method.description, method.accessModifier));

            this.parents.Add(character);
        }
    }

    public void InitializeCharacter(Character character)
    {
        this.name = character.name;
        this.description = character.description;

        this.parents = character.parents;
        this.childrens = character.childrens;
        this.attributes = character.attributes;
        this.methods = character.methods;
    }

    public void Dispose()
    {
        // Removing himself from its parnents

        foreach (Character parent in parents)
        {
            parent.childrens.Remove(this);
        }
    }


    public bool IsLeaf()
    {
        return childrens.Count == 0;
    }

    public bool IsLeftMost()
    {
        if (parents.Count == 0)
            return true;

        return parents[0].childrens[0] == this;
    }

    public bool IsRightMost()
    {
        if (parents.Count == 0)
            return true;

        return parents[0].childrens[parents[0].childrens.Count - 1] == this;
    }

    public Character GetPreviousSibling()
    {
        if (parents.Count == 0 || this.IsLeftMost())
            return null;

        return parents[0].childrens[parents[0].childrens.IndexOf(this) - 1];
    }

    public Character GetNextSibling()
    {
        if (parents.Count == 0 || this.IsRightMost())
            return null;

        return parents[0].childrens[parents[0].childrens.IndexOf(this) + 1];
    }

    public Character GetLeftMostSibling()
    {
        if (parents.Count == 0)
            return null;

        if (this.IsLeftMost())
            return this;

        return parents[0].childrens[0];
    }

    public Character GetLeftMostChild()
    {
        if (childrens.Count == 0)
            return null;

        return childrens[0];
    }

    public Character GetRightMostChild()
    {
        if (childrens.Count == 0)
            return null;

        return childrens[childrens.Count - 1];
    }

    public int GetDepth()
    {
        return this._depth;
    }

    public void SetDepth(int _depth)
    {
        this._depth = _depth;
    }

    public int GetX()
    {
        return this._x;
    }

    public void SetX(int x)
    {
        this._x = x;
    }

    public int GetY()
    {
        return this._y;
    }

    public void SetY(int y)
    {
        this._y = y;
    }

    public int GetMod()
    {
        return this._mod;
    }

    public void SetMod(int mod)
    {
        this._mod = mod;
    }

    public GameObject GetCharacterButton()
    {
        return this.CharacterButton;
    }

    public void SetCharacterButton(GameObject CharacterButton)
    {
        this.CharacterButton = CharacterButton;
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
