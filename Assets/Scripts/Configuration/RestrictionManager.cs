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
    public bool OnlineGame;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    { 
        Instance = this;

        if (RestrictionMenu.Instance != null)
        {
            AllowSingleInheritance = RestrictionMenu.Instance.AllowSingleInheritance;
            AllowBeginnerInheritance = RestrictionMenu.Instance.AllowBeginnerInheritance;
            AllowSpecialAbility = RestrictionMenu.Instance.AllowSpecialAbility;
            AllowAccessModifier = RestrictionMenu.Instance.AllowAccessModifier;
            AllowOverride = RestrictionMenu.Instance.AllowOverride;
            AllowUpcasting = RestrictionMenu.Instance.AllowUpcasting;
            AllowAbstractClass = RestrictionMenu.Instance.AllowAbstractClass;
            AllowEncapsulation = RestrictionMenu.Instance.AllowEncapsulation;
            OnlineGame = true;
            RestrictionMenu.Instance.Destroy();
        }



        ApplyRestrictions(); 
    }

    private void ApplyRestrictions()
    {
        GameObject popupsGameObject = GameObject.Find("Canvas/Popups");
        // GameObject SpecialAbilitiesContent = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/SpecialAbility");
        // if (!AllowSpecialAbilities)
        // {
        //     SpecialAbilitiesContent.SetActive(false);
        // }
    }
}
