using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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
    public List<Button> RestrictionButtons = new List<Button>();
    public Button ConfirmButton;
    public Button SkipButton;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        SetButtons();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetButtons()
    {
        var transform = GameObject.Find("Canvas/Menus/Panel/Window/Inner/ButtonsGrid").transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            int index = i;
            var button = transform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => SetBool(index));
            button.onClick.AddListener(Mark);
            RestrictionButtons.Add(button);
        }
        transform = GameObject.Find("Canvas/Menus/Panel/Window/Inner/Buttons").transform;
        ConfirmButton = transform.GetChild(0).GetComponent<Button>();
        ConfirmButton.onClick.AddListener(Confirm);

        SkipButton = transform.GetChild(1).GetComponent<Button>();
        SkipButton.onClick.AddListener(Confirm);
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
            case 1:
                AllowSpecialAbility = !AllowSpecialAbility;
                break;
            case 2:
                AllowAccessModifier = !AllowAccessModifier;
                break;
            case 3:
                AllowOverride = !AllowOverride;
                break;
            case 4:
                AllowUpcasting = !AllowUpcasting;
                break;
            case 5:
                AllowAbstractClass = !AllowAbstractClass;
                break;
            case 6:
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
        Destroy(gameObject.GetComponent<RestrictionMenu>());
    }
}
