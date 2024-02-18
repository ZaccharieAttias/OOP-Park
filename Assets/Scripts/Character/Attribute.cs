public class Attribute
{
    public string Name;
    public string Description;
    public float Value;

    public AccessModifier AccessModifier;

    
    public Attribute()
    { 
        Name = "Default";
        Description = "Default";
        Value = 0;

        AccessModifier = AccessModifier.Public;
    }
    public Attribute(string name, string description, float value, AccessModifier accessModifier)
    {
        Name = name;
        Description = description;
        Value = value;

        AccessModifier = accessModifier;
    }
    public Attribute(Attribute attribute)
    {
        Name = attribute.Name;
        Description = attribute.Description;
        Value = attribute.Value;
        
        AccessModifier = attribute.AccessModifier;
    }
}
