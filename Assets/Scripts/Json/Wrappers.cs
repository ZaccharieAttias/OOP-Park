using System.Collections.Generic;


[System.Serializable]
public class CollectionWrapper
{
    public List<CharacterAttribute> AttributesCollection;
    public List<CharacterMethod> MethodsCollection;
    public Dictionary<SpecialAbility, List<CharacterSpecialAbility>> SpecialAbilitiesCollection;
    public List<Character> CharactersCollection;
}