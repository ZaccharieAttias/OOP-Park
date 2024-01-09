using UnityEngine;
using UnityEngine.UI;


public class CancelFactory : MonoBehaviour
{
    public void Start() { InitializeGameObject(); }

    private void InitializeGameObject()
    {
        GameObject characterFactory = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/CharacterFactory");

        name = "CancelFactory";
        transform.localPosition = new Vector3(-168, -237, 0);
        
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Sprites/Icons/Restart");
        image.color = Color.white;
        
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => characterFactory.GetComponent<CharacterFactory>().CancelFactory());
        button.interactable = false;

        gameObject.SetActive(false);
    }
}
