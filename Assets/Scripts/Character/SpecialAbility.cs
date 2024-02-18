public class SpecialAbility
{
    public string Name;
    public string Description;
    public float Value;

    public SpecialAbilityType Type;


    public SpecialAbility()
    { 
        Name = "Default";
        Description = "Default";
        Value = 0;

        Type = SpecialAbilityType.General;
    }
    public SpecialAbility(string name, string description, float value, SpecialAbilityType type)
    {
        Name = name;
        Description = description;
        Value = value;

        Type = type;
    }
    public SpecialAbility(SpecialAbility specialAbility)
    {
        Name = specialAbility.Name;
        Description = specialAbility.Description;
        Value = specialAbility.Value;
        
        Type = specialAbility.Type;
    }
}
