[System.Serializable]
public class CharacterMethod
{
    public string Name;
    public string Description;
    public CharacterAttribute Attribute;

    public AccessModifier AccessModifier;


    public CharacterMethod()
    { 
        Name = "";
        Description = "";
        Attribute = new CharacterAttribute();

        AccessModifier = AccessModifier.Public;
    }
    public CharacterMethod(string name, string description, CharacterAttribute attribute, AccessModifier accessModifier)
    {
        Name = name;
        Description = description;
        Attribute = attribute;
        
        AccessModifier = accessModifier;
    }
    public CharacterMethod(CharacterMethod method)
    {
        Name = method.Name;
        Description = method.Description;
        Attribute = method.Attribute;
        
        AccessModifier = method.AccessModifier;
    }
}
