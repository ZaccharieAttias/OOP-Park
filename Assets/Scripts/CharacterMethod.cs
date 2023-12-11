using UnityEngine;


[System.Serializable]
public class CharacterMethod
{
    public string name;
    public string description;
    public AccessModifier accessModifier;


    public CharacterMethod(string name, string description, AccessModifier accessModifier)
    {
        this.name = name;
        this.description = description;
        this.accessModifier = accessModifier;
    }
}
