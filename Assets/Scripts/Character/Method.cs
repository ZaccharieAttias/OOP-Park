public class Method
{
    public string Owner;
    public string Name;
    public string Description;
    public Attribute Attribute;

    public AccessModifier AccessModifier;


    public Method()
    { 
        Owner = null;
        Name = "Default";
        Description = "Default";
        Attribute = new Attribute();

        AccessModifier = AccessModifier.Public;
    }
    public Method(Method method, string owner, Attribute attribute)
    {
        Owner = owner;
        Name = method.Name;
        Description = method.Description;
        Attribute = attribute;
        
        AccessModifier = method.AccessModifier;
    }
}
