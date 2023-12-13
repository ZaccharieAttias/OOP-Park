using UnityEngine;


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