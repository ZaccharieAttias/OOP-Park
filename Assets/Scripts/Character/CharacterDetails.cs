using UnityEngine;

public class CharacterDetails : MonoBehaviour
{
    [SerializeField] private Character character;

    public void InitializeCharacter(Character character)
    {
        this.character = character;
    }

    public Character GetCurrentCharacter()
    {
        return this.character;
    }
}
