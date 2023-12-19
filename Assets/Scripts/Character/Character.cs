using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[System.Serializable]
public class Character
{
    public string name;
    public Character[] ancestors;
    public CharacterAttribute[] attributes;
    public CharacterMethod[] methods;
    public string description;
}