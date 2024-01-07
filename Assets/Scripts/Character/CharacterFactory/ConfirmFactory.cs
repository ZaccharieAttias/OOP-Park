using UnityEngine;
using UnityEngine.UI;


public class ConfirmFactory : MonoBehaviour
{
    public void Start() { InitializeGameObject(); }

    private void InitializeGameObject()
    {
        GameObject characterFactory = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/CharacterFactory");

        name = "ConfirmFactory";
        transform.localPosition = new Vector3(-68, -237, 0);

        Image image = GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Sprites/Icons/Confirm");
        image.color = Color.white;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => characterFactory.GetComponent<CharacterFactory>().ConfirmFactory());
        button.interactable = false;

        gameObject.SetActive(false);
    }
}
