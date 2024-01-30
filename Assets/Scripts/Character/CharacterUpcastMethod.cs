[System.Serializable]
public class CharacterUpcastMethod
{
    public Character Character;
    public CharacterMethod CharacterMethod;
    public int Quantity;

    public CharacterUpcastMethod(Character character, CharacterMethod characterMethod, int quantity)
    {
        Character = character;
        CharacterMethod = characterMethod;
        Quantity = quantity;
    }

    public void UpdateUpcast()
    {
        Quantity--;
        if (Quantity == 0) Character.UpcastMethods.Remove(this);
    }
}