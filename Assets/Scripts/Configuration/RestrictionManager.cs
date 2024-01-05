using UnityEngine;

public class RestrictionManager : MonoBehaviour
{
    public static RestrictionManager Instance { get; private set; }

    public bool AllowSingleInheritance { get; set; }
    public bool AllowMultipleInheritance { get; set; }

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

        GameObject prefab = Resources.Load<GameObject>("Prefabs/Buttons/Inheritance");

        Instantiate(prefab);
    }
}
