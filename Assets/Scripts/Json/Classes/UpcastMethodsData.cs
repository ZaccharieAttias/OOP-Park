public static class UpcastMethodsData
{
    public static UpcastMethodData PackData(Character character)
    {
        var upcastMethod = character.UpcastMethod?.CharacterMethod;
        UpcastMethodData upcastMethodData = new()
        {
            Owner = upcastMethod == null ? null : MethodsData.FindMethodOwner(character, upcastMethod),
            Name = upcastMethod?.Name,
            Amount = upcastMethod == null ? 0 : character.UpcastMethod.Amount
        };
        
        return upcastMethodData;
    }
    public static CharacterUpcastMethod UnpackData(CharacterData characterData)
    {
        if (characterData.UpcastMethod.Name == null) return null;
        
        CharacterMethod method = CharactersData.CharacterManager.CharactersCollection.Find(character => character.Name == characterData.UpcastMethod.Owner).Methods.Find(method => method.Name == characterData.UpcastMethod.Name);
        float amount = characterData.UpcastMethod.Amount;

        CharacterUpcastMethod upcastMethod = new(method, amount);
        return upcastMethod;
    }
}


[System.Serializable]
public class UpcastMethodData
{
    public string Owner;
    public string Name;
    public float Amount;
}
