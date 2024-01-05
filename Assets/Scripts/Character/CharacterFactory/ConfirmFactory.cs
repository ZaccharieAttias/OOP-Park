using UnityEngine;
using UnityEngine.UI;


public class ConfirmFactory : MonoBehaviour
{
    private const string spritePath = "Sprites/Icons/Confirm";
    private const string characterFactoryPath = "Canvas/HTMenu/Menu/Characters/Tree/Buttons/CharacterFactory";


    public void Start() { InitializeGameObject(); }

    private void InitializeGameObject()
    {
        GameObject characterFactory = GameObject.Find(characterFactoryPath);

        name = "ConfirmFactory";

        transform.SetParent(characterFactory.transform);
        transform.localPosition = new Vector3(-68, -237, 0);
        transform.localScale = new Vector3(1, 1, 1);

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(20, 20);

        Image image = GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(spritePath);
        image.color = Color.white;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => characterFactory.GetComponent<CharacterFactory>().ConfirmFactory());
        button.interactable = false;

        gameObject.SetActive(false);
    }
}
