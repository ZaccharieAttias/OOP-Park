using UnityEngine;


public class RestrictionManager : MonoBehaviour
{
    public static RestrictionManager Instance;
    
    public bool AllowSingleInheritance;
    public bool AllowMultipleInheritance;

    public bool AllowAccessModifiers;
    public bool AllowOverride;
    public bool AllowUpcasting;


    public void Start()
    {
        Instance = this;
        
        AllowSingleInheritance = false;
        AllowMultipleInheritance = true;
        AllowAccessModifiers = false;
        
        ApplyRestrictions();
    }

    private void ApplyRestrictions()
    {
        ApplyInheritanceRestriction();
    }

    private void ApplyInheritanceRestriction()
    {
        if (AllowSingleInheritance == false && AllowMultipleInheritance == false) return;

        new GameObject("CharacterFactory").AddComponent<CharacterFactory>();
    }
}
