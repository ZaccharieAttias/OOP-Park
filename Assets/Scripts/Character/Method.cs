public class Method
{
    public string Name;
    public string Description;
    public Attribute Attribute;

    public AccessModifier AccessModifier;


    public Method()
    { 
        Name = "Default";
        Description = "Default";
        Attribute = new Attribute();

        AccessModifier = AccessModifier.Public;
    }
    public Method(string name, string description, Attribute attribute, AccessModifier accessModifier)
    {
        Name = name;
        Description = description;
        Attribute = attribute;
        
        AccessModifier = accessModifier;
    }
    public Method(Method method)
    {
        Name = method.Name;
        Description = method.Description;
        Attribute = method.Attribute;
        
        AccessModifier = method.AccessModifier;
    }
    public Method(Method method, Attribute attribute) // Fix in the future
    {
        Name = method.Name;
        Description = method.Description;
        Attribute = attribute;
        
        AccessModifier = method.AccessModifier;
    }
}
