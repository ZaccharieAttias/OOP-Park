[System.Serializable]
public class CharacterAttribute
{
    public string Name;
    public string Description;
    public float Value;

    public AccessModifier AccessModifier;

    
    public CharacterAttribute()
    { 
        Name = "";
        Description = "";
        Value = 0;
        AccessModifier = AccessModifier.Public;
    }
    public CharacterAttribute(string name, string description, float value, AccessModifier accessModifier)
    {
        Name = name;
        Description = description;
        Value = value;
        
        AccessModifier = accessModifier;
    }
    public CharacterAttribute(CharacterAttribute attribute)
    {
        Name = attribute.Name;
        Description = attribute.Description;
        Value = attribute.Value;
        
        AccessModifier = attribute.AccessModifier;
    }
}
