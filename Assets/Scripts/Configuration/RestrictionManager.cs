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
    public bool AllowAbstractClasses;
    public bool AllowEncapsulation;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    { 
        Instance = this;

        ApplyRestrictions(); 
    }

    private void ApplyRestrictions()
    {
        GameObject popupsGameObject = GameObject.Find("Canvas/Popups");      
    }
}
