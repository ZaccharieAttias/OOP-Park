using UnityEngine;


public class RestrictionManager : MonoBehaviour
{
    public static RestrictionManager Instance;

    public bool AllowInheritance;
    public bool AllowSingleInheritance;
    public bool AllowMultipleInheritance;
    public bool AllowBeginnerInheritance;
    
    public bool AllowAccessModifiers;
    public bool AllowOverride;
    public bool AllowUpcasting;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties() { Instance = this; }
}
