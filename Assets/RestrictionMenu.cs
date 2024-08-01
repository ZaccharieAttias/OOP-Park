using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class RestrictionMenu : MonoBehaviour
{
    public static RestrictionMenu Instance;
    public bool AllowSingleInheritance;
    public bool AllowSpecialAbility;
    public bool AllowAccessModifier;
    public bool AllowOverride;
    public bool AllowUpcasting;
    public bool AllowAbstractClass;
    public bool AllowEncapsulation;
    [SerializeField] List<Button> RestrictionButtons = new List<Button>();
    public Button ConfirmButton;

    public void Awake() 
    { 
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Mark()
    {
        var selectedGameObject = EventSystem.current.currentSelectedGameObject;
        bool isSelected = selectedGameObject.GetComponent<Image>().color == Color.green;
        selectedGameObject.GetComponent<Image>().color = isSelected ? Color.white : Color.green;

        ConfirmButton.GetComponent<Button>().interactable = RestrictionButtons.Exists(button => button.GetComponent<Image>().color == Color.green);
    }
    public void SetBool(int index)
    {
        switch (index)
        {
            case 0:
                AllowSingleInheritance = !AllowSingleInheritance;
                break;
            case 2:
                AllowSpecialAbility = !AllowSpecialAbility;
                break;
            case 3:
                AllowAccessModifier = !AllowAccessModifier;
                break;
            case 4:
                AllowOverride = !AllowOverride;
                break;
            case 5:
                AllowUpcasting = !AllowUpcasting;
                break;
            case 6:
                AllowAbstractClass = !AllowAbstractClass;
                break;
            case 7:
                AllowEncapsulation = !AllowEncapsulation;
                break;
        }
    }
    public void Confirm()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OnlineBuilder");
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
