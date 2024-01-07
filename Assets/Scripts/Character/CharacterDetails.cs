using UnityEngine;


public class CharacterDetails : MonoBehaviour
{
    [SerializeField] public Character Character { get; set;}


    public void InitializeCharacter(Character character) { Character = character; }
}
