using UnityEngine;


public class UpcastingManager : MonoBehaviour
{
    public UpcastingUI UpcastingUI;
    public UpcastingLogic UpcastingLogic;
    public CharacterManager CharacterManager;


    public void Start()
    { 
        InitializeGameObject();
        InitializeProperties(); 
    }
    private void InitializeGameObject() 
    { 
        transform.SetParent(GameObject.Find("Canvas/GameplayScreen/Popups").transform); 
        transform.localPosition = new Vector3(0, 0, 0);    
    }
    private void InitializeProperties()
    {
        GameObject upcastingUI = Instantiate(Resources.Load<GameObject>("Popups/Upcasting"), transform);
        upcastingUI.name = "UpcastingUI";
        upcastingUI.AddComponent<UpcastingUI>();
        UpcastingUI = upcastingUI.GetComponent<UpcastingUI>();

        GameObject upcastingLogic = new("UpcastingLogic", typeof(UpcastingLogic));
        upcastingLogic.transform.SetParent(transform);        
        UpcastingLogic = upcastingLogic.GetComponent<UpcastingLogic>();
        
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }
    
    public void Update()
    { 
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpcastingLogic.LoadPopup(CharacterManager.CurrentCharacter);
            UpcastingUI.ToggleActivation();
            Time.timeScale = Time.timeScale == 0 ? 1f : 0;
        }
    }
}
