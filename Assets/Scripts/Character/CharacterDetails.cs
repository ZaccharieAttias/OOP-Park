using UnityEngine;

[System.Serializable]
public class CharacterDetails : MonoBehaviour
{
    public Character Character;

    public void InitializeCharacter(Character character) { Character = character; }
}
