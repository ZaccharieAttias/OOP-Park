using UnityEngine;


[System.Serializable]
public class CharacterUpcastMethod
{
    public Character Character;
    public CharacterMethod CharacterMethod;
    public float Amount;

    public UpcastTrackerManager UpcastTrackerManager;


    public CharacterUpcastMethod(Character character, CharacterMethod characterMethod, float amount)
    {
        Character = character;
        CharacterMethod = characterMethod;
        Amount = amount;

        UpcastTrackerManager = GameObject.Find("Canvas/Popups").GetComponent<UpcastTrackerManager>();
        UpcastTrackerManager.AmountDescriptionText.text = ((int)Amount).ToString();
    }
}
