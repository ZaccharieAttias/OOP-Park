[System.Serializable]
public class CharacterSpecialAbility
{
    public string Name;
    public string Description;
    public float Value;

    public SpecialAbility Type;

    public CharacterSpecialAbility(string name, string description, float value, SpecialAbility abilityType)
    {
        Name = name;
        Description = description;
        Value = value;
        Type = abilityType;
    }

    
}