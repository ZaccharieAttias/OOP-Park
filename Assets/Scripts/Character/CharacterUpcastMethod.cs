using UnityEngine;


[System.Serializable]
public class CharacterUpcastMethod
{
    public Method CharacterMethod;
    public float Amount;

    public UpcastTrackerManager UpcastTrackerManager;


    public CharacterUpcastMethod(Method characterMethod, float amount)
    {
        CharacterMethod = characterMethod;
        Amount = amount;

        UpcastTrackerManager = GameObject.Find("Canvas/Popups").GetComponent<UpcastTrackerManager>();
        UpcastTrackerManager.AmountDescriptionText.text = ((int)Amount).ToString();
    }
}
