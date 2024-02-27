public class Attribute
{
    public string Owner;
    public string Name;
    public string Description;
    public float Value;

    public AccessModifier AccessModifier;

    
    public Attribute()
    { 
        Owner = null;
        Name = "Default";
        Description = "Default";
        Value = 0;

        AccessModifier = AccessModifier.Public;
    }
    public Attribute(Attribute attribute, string owner)
    {
        Owner = owner;
        Name = attribute.Name;
        Description = attribute.Description;
        Value = attribute.Value;
        
        AccessModifier = attribute.AccessModifier;
    }
}
