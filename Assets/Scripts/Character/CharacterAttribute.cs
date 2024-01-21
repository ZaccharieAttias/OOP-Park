[System.Serializable]
public class CharacterAttribute
{
    public string Name;
    public string Description;
    public float Value;

    public AccessModifier AccessModifier;

    
    public CharacterAttribute(string name, string description, float value, AccessModifier accessModifier)
    {
        Name = name;
        Description = description;
        Value = value;
        
        AccessModifier = accessModifier;
    }
}
