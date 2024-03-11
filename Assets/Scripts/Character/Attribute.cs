public class Attribute
{
    public string Owner;
    public string Name;
    public string Description;
    public float Value;

    public AccessModifier AccessModifier;

    public bool Getter;
    public bool Setter;

    
    public Attribute()
    { 
        Owner = null;
        Name = "Default";
        Description = "Default";
        Value = 0;

        AccessModifier = AccessModifier.Public;
        Getter = false;
        Setter = false;
    }
    public Attribute(Attribute attribute, string owner)
    {
        Owner = owner;
        Name = attribute.Name;
        Description = attribute.Description;
        Value = attribute.Value;
        
        AccessModifier = attribute.AccessModifier;
        Getter = attribute.Getter;
        Setter = attribute.Setter;
    }
}
