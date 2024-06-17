using UnityEngine;


public class RestrictionManager : MonoBehaviour
{
    public static RestrictionManager Instance;

    public bool AllowSingleInheritance;
    public bool AllowBeginnerInheritance;
    
    public bool AllowSpecialAbility;
    public bool AllowAccessModifier;
    public bool AllowOverride;
    public bool AllowUpcasting;
    public bool AllowAbstractClass;
    public bool AllowEncapsulation;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    { 
        Instance = this;

        ApplyRestrictions(); 
    }

    private void ApplyRestrictions()
    {
        // GameObject popupsGameObject = GameObject.Find("Canvas/Popups");
        // GameObject SpecialAbilitiesContent = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/SpecialAbility");
        // if (!AllowSpecialAbilities)
        // {
        //     SpecialAbilitiesContent.SetActive(false);
        // }
    }
}
