[System.Serializable]
public class CharacterSpecialAbility
{
    public string Name;
    public string Description;
    public float Value;
    public SpecialAbility Type;


    public CharacterSpecialAbility()
    { 
        Name = "";
        Description = "";
        Value = 0;
        Type = SpecialAbility.General;
    }
    public CharacterSpecialAbility(string name, string description, float value, SpecialAbility type)
    {
        Name = name;
        Description = description;
        Value = value;
        Type = type;
    }
    public CharacterSpecialAbility(CharacterSpecialAbility specialAbility)
    {
        Name = specialAbility.Name;
        Description = specialAbility.Description;
        Value = specialAbility.Value;
        Type = specialAbility.Type;
    }
}
