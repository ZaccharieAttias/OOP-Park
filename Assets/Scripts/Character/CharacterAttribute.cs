using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[System.Serializable]
public class CharacterAttribute
{
    public string name;
    public string description;
    public AccessModifier accessModifier;

    public CharacterAttribute(string name, string description, AccessModifier accessModifier)
    {
        this.name = name;
        this.description = description;
        this.accessModifier = accessModifier;
    }
}
