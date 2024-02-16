public static class UpcastMethodsData
{
    public static UpcastMethodData PackData(Character character)
    {
        var upcastMethod = character.UpcastMethod?.CharacterMethod;
        UpcastMethodData upcastMethodData = new()
        {
            Owner = upcastMethod is null ? null : MethodsData.FindMethodOwner(character, upcastMethod),
            Name = upcastMethod?.Name,
            Amount = upcastMethod is null ? 0 : character.UpcastMethod.Amount
        };

        return upcastMethodData;
    }
    public static CharacterUpcastMethod UnpackData(CharacterData characterData)
    {
        var upcastMethodData = characterData.UpcastMethod.Name is null ? null : CharactersData.CharactersManager.CharactersCollection.Find(character => character.Name == characterData.UpcastMethod.Owner).Methods.Find(method => method.Name == characterData.UpcastMethod.Name);
        
        CharacterUpcastMethod upcastMethod = new(upcastMethodData, characterData.UpcastMethod.Amount);
        
        return upcastMethodData is null ? null : upcastMethod;
    }
}


[System.Serializable]
public class UpcastMethodData
{
    public string Owner;
    public string Name;
    public float Amount;
}
