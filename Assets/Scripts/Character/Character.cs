using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[System.Serializable]
public class Character
{
    public string name;
    public List<Character> ancestors;
    public List<CharacterAttribute> attributes;
    public List<CharacterMethod> methods;
    public string description;

    public Character()
    {
        ancestors = new List<Character>();
        attributes = new List<CharacterAttribute>();
        methods = new List<CharacterMethod>();
    }
}