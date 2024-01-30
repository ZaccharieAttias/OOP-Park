public enum AbilityType
{
    General, Automatic, Jump, Gravity, Speed, DoubleJump, WeakGravity, FastSpeed
}

[System.Serializable]
public class CharacterSpecialAbility
{
    public string Name;
    public string Description;
    public float Value;

    public AbilityType Type;

    public CharacterSpecialAbility(string name, string description, float value, AbilityType abilityType)
    {
        Name = name;
        Description = description;
        Value = value;
        Type = abilityType;
    }

    
}