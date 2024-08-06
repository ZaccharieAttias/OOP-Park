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
            Amount = upcastMethod != null ? character.UpcastMethod.Amount : 0
        };

        return upcastMethodData;
    }
    public static UpcastMethod UnpackData(CharacterData characterData)
    {
        var upcastMethodData = CharactersData.CharactersManager.CharactersCollection
            .FirstOrDefault(character => character.Name == characterData.UpcastMethod.Owner)?.Methods
            .FirstOrDefault(method => method.Name == characterData.UpcastMethod.Name);

        return upcastMethodData != null ? new UpcastMethod(upcastMethodData, characterData.UpcastMethod.Amount) : null;
    }
}


public class UpcastMethodData
{
    public string Owner;
    public string Name;
    public float Amount;
}