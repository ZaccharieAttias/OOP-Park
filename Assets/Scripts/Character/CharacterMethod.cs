[System.Serializable]
public class CharacterMethod
{
    public string Name;
    public string Description;
    public AccessModifier AccessModifier;
    public CharacterAttribute Attribute;


    public CharacterMethod(string name, string description, AccessModifier accessModifier)
    {
        Name = name;
        Description = description;
        AccessModifier = accessModifier;
    }
}
