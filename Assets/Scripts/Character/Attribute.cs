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
        
        Getter = false;
        Setter = false;

        AccessModifier = attribute.AccessModifier;
    }

    public Attribute(Attribute attribute, string owner, float value, bool getter, bool setter)
    {
        Owner = owner;
        Name = attribute.Name;
        Description = attribute.Description;
        Value = value;

        Getter = getter;
        Setter = setter;

        AccessModifier = attribute.AccessModifier;
    }
}
