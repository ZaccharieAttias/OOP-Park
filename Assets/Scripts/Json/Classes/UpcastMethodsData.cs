using System.Linq;

public static class UpcastMethodsData
{
    public static UpcastMethodData PackData(CharacterB character)
    {
        var upcastMethod = character.UpcastMethod?.CharacterMethod;
        UpcastMethodData upcastMethodData = new()
        {
            Owner = upcastMethod?.Owner,
            Name = upcastMethod?.Name,
            Amount = upcastMethod is null ? 0 : character.UpcastMethod.Amount
        };

        return upcastMethodData;
    }
    public static UpcastMethod UnpackData(CharacterData characterData)
    {
        var upcastMethodData = CharactersData.CharactersManager.CharactersCollection
            .FirstOrDefault(character => character.Name == characterData.UpcastMethod.Owner)?.Methods
            .FirstOrDefault(method => method.Name == characterData.UpcastMethod.Name);

        return upcastMethodData is null ? null : new(upcastMethodData, characterData.UpcastMethod.Amount);
    }
}


public class UpcastMethodData
{
    public string Owner;
    public string Name;
    public float Amount;
}
