using UnityEngine;


public class UpcastMethod
{
    public Method CharacterMethod;
    public float Amount;

    public UpcastingTrackerManager UpcastingTrackerManager = GameObject.Find("Canvas/Popups").GetComponent<UpcastingTrackerManager>();


    public UpcastMethod(Method characterMethod, float amount)
    {
        CharacterMethod = characterMethod;
        Amount = amount;

        UpcastingTrackerManager.AmountDescriptionText.text = ((int)Amount).ToString();
    }
}
