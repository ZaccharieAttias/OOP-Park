[System.Serializable]
public class CharacterMethod
{
    public string Name;
    public string Description;
    public AccessModifier AccessModifier;
    public CharacterAttribute Attribute;

    public CharacterMethod() 
    { 
        Name = "";
        Description = "";
        AccessModifier = AccessModifier.Public;
        Attribute = new CharacterAttribute();
    }
    public CharacterMethod(string name, string description, AccessModifier accessModifier)
    {
        Name = name;
        Description = description;
        AccessModifier = accessModifier;
    }
    public CharacterMethod(CharacterMethod method)
    {
        Name = method.Name;
        Description = method.Description;
        AccessModifier = method.AccessModifier;
        Attribute = method.Attribute;
    }
}
