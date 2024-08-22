using UnityEngine;


public class UpcastMethod
{
    public Method CharacterMethod;
    public float Amount;

    public TypeCastingTrackerManager TypeCastingTrackerManager = GameObject.Find("Canvas/Popups").GetComponent<TypeCastingTrackerManager>();


    public UpcastMethod(Method characterMethod, float amount)
    {
        CharacterMethod = characterMethod;
        Amount = amount;

        TypeCastingTrackerManager.AmountDescriptionText.text = ((int)Amount).ToString();
    }
}
