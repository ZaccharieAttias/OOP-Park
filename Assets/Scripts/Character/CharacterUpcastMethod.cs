using UnityEngine;


[System.Serializable]
public class CharacterUpcastMethod
{
    public CharacterMethod CharacterMethod;
    public float Amount;

    public UpcastTrackerManager UpcastTrackerManager;


    public CharacterUpcastMethod(CharacterMethod characterMethod, float amount)
    {
        CharacterMethod = characterMethod;
        Amount = amount;

        UpcastTrackerManager = GameObject.Find("Canvas/Popups").GetComponent<UpcastTrackerManager>();
        UpcastTrackerManager.AmountDescriptionText.text = ((int)Amount).ToString();
    }
}
