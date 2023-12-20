using UnityEngine;
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

    public Character(string name, string description, List<CharacterAttribute> attributes, List<CharacterMethod> methods, List<Character> ancestors)
    {
        this.name = name;
        this.description = description;
        this.attributes = attributes;
        this.methods = methods;
        this.ancestors = ancestors;
    }
}