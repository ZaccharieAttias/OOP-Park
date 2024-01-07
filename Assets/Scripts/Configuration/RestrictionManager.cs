using UnityEngine;


public class RestrictionManager : MonoBehaviour
{
    public static RestrictionManager Instance { get; set; }

    public bool AllowSingleInheritance { get; set; }
    public bool AllowMultipleInheritance { get; set; }

    public bool AllowAccessModifiers { get; set; } // Still didnt do
    public bool AllowOverride { get; set; } // Still didnt do
    public bool AllowUpcasting { get; set; } // Still didnt do

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AllowSingleInheritance = true;
        AllowMultipleInheritance = false;
        
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
