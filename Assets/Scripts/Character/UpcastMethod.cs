public class UpcastMethod
{
    public Method CharacterMethod;
    public float Amount;

    public UpcastTrackerManager UpcastTrackerManager; 


    public UpcastMethod()
    {
        CharacterMethod = null;
        Amount = 0;

        UpcastTrackerManager.AmountDescriptionText.text = ((int)Amount).ToString();
    }
    public UpcastMethod(Method characterMethod, float amount)
    {
        CharacterMethod = characterMethod;
        Amount = amount;

        UpcastTrackerManager.AmountDescriptionText.text = ((int)Amount).ToString();
    }
    public UpcastMethod(UpcastMethod upcastMethod)
    {
        CharacterMethod = upcastMethod.CharacterMethod;
        Amount = upcastMethod.Amount;

        UpcastTrackerManager.AmountDescriptionText.text = ((int)Amount).ToString();
    }
}
