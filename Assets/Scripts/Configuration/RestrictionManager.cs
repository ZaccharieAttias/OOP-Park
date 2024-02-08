using UnityEngine;


public class RestrictionManager : MonoBehaviour
{
    public static RestrictionManager Instance;

    public bool AllowSingleInheritance;
    public bool AllowMultipleInheritance;
    public bool AllowBeginnerInheritance;
    
    public bool AllowAccessModifiers;
    public bool AllowOverride;
    public bool AllowUpcasting;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    { 
        Instance = this;

        ApplyRestrictions(); 
    }

    private void ApplyRestrictions()
    {
        if (AllowSingleInheritance || AllowMultipleInheritance) GameObject.Find("Canvas/Popups").AddComponent<CharacterCreationManager>();
        if (AllowUpcasting) GameObject.Find("Canvas/Popups").AddComponent<UpcastingManager>();
    }
}
