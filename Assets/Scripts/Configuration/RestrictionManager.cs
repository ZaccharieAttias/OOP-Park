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


    public void Start()
    {
        Instance = this;
        
        ApplyRestrictions();
    }
    private void ApplyRestrictions()
    {
        if (AllowInheritance) ApplyRestriction<CharacterFactory>();
        if (AllowUpcasting) ApplyRestriction<UpcastingManager>();
    }
    private void ApplyRestriction<T>() where T : MonoBehaviour { new GameObject(typeof(T).Name).AddComponent<T>(); }
}
