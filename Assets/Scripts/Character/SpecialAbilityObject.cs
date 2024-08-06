using System.Collections.Generic;
using UnityEngine;


public class SpecialAbilityObject
{
    public GameObject Button;
    public string Name;
    public SpecialAbility SpecialAbility;
    public int X;
    public int Y;
    public int Mod;
    public int Depth;


    public List<SpecialAbilityObject> Childrens;
    public SpecialAbilityObject Parent;



    public SpecialAbilityObject(SpecialAbility specialAbility)
    {
        Button = null;
        Name = "";
        X = 0;
        Y = 0;
        Mod = 0;
        Depth = 0;
        Childrens = new List<SpecialAbilityObject>();
        Parent = null;
        SpecialAbility = specialAbility;
    }


    public bool IsRoot() { return Parent == null; }
    public bool IsLeaf() { return Childrens.Count == 0; }


    public bool IsLeftMost() { return IsRoot() || Parent.Childrens[0] == this; }
    public bool IsRightMost() { return IsRoot() || Parent.Childrens[^1] == this; }


    public SpecialAbilityObject GetLeftMostSibling() { return IsRoot() ? null : Parent.Childrens[0]; }
    public SpecialAbilityObject GetLeftMostChild() { return IsLeaf() ? null : Childrens[0]; }


    public SpecialAbilityObject GetRightMostSibling() { return IsRoot() ? null : Parent.Childrens[^1]; }
    public SpecialAbilityObject GetRightMostChild() { return IsLeaf() ? null : Childrens[^1]; }


    public SpecialAbilityObject GetPreviousSibling() { return IsLeftMost() ? null : Parent.Childrens[Parent.Childrens.IndexOf(this) - 1]; }
    public SpecialAbilityObject GetNextSibling() { return IsRightMost() ? null : Parent.Childrens[Parent.Childrens.IndexOf(this) + 1]; }


    public void SetTransformPositionX(float x)
    {
        RectTransform rectTransform = Button.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
    }
    public void SetTransformPositionY(float y)
    {
        RectTransform rectTransform = Button.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
    }
}

