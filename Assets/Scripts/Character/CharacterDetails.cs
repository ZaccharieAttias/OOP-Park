using UnityEngine;

public class CharacterDetails : MonoBehaviour
{
    [SerializeField] public Character Character;

    public void InitializeCharacter(Character character)
    {
        Character = character;
    }

    public Character GetCurrentCharacter()
    {
        return this.Character;
    }
}
