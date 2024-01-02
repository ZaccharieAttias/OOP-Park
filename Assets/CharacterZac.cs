using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[System.Serializable]
public class CharacterZac : MonoBehaviour
{
    public string name;

    public List<CharacterZac> parents;
    public List<CharacterZac> childrens;
    private RectTransform _rectTransform;

    public int depth;
    public int Mod = 0;

    public int x = 0;
    public int y = 0;

    

    public void Start()
    {
        // set transform position y to depth
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Update()
    {
        // set transform position y to depth
        _rectTransform.anchoredPosition = new Vector2(x, y);
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

    public CharacterZac GetPreviousSibling()
    {
        if (parents.Count == 0 || this.IsLeftMost())
            return null;

        return parents[0].childrens[parents[0].childrens.IndexOf(this) - 1];
    }

    public CharacterZac GetNextSibling()
    {
        if (parents.Count == 0 || this.IsRightMost())
            return null;

        return parents[0].childrens[parents[0].childrens.IndexOf(this) + 1];
    }

    public CharacterZac GetLeftMostSibling()
    {
        if (parents.Count == 0)
            return null;

        if (this.IsLeftMost())
            return this;

        return parents[0].childrens[0];
    }

    public CharacterZac GetLeftMostChild()
    {
        if (childrens.Count == 0)
            return null;

        return childrens[0];
    }

    public CharacterZac GetRightMostChild()
    {
        if (childrens.Count == 0)
            return null;

        return childrens[childrens.Count - 1];
    }














    // public float GetTransformPositionX()
    // {
    //     return _rectTransform.anchoredPosition.x;
    // }

    // public float GetTransformPositionY()
    // {
    //     return _rectTransform.anchoredPosition.y;
    // }

    // public void SetTransformPositionX(float x)
    // {
    //     _rectTransform.anchoredPosition = new Vector2(x, _rectTransform.anchoredPosition.y);
    // }

    // public void SetTransformPositionY(float y)
    // {
    //     _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, y);
    // }

    public void SetDepth(int depth)
    {
        this.depth = depth;
    }

    public int GetDepth()
    {
        return depth;
    }

    public void SetMod(int mod)
    {
        Mod = mod;
    }

    public int GetMod()
    {
        return Mod;
    }
}
