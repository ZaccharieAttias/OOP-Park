using UnityEngine;
using UnityEngine.UI;


public class AddFactory : MonoBehaviour
{   
    private const string spritePath = "Sprites/Icons/Plus";
    private const string characterFactoryPath = "Canvas/HTMenu/Menu/Characters/Tree/Buttons/CharacterFactory";
    

    public void Start() { InitializeGameObject(); }

    private void InitializeGameObject()
    {
        GameObject characterFactory = GameObject.Find(characterFactoryPath);

        name = "AddFactory";

        transform.SetParent(characterFactory.transform);
        transform.localPosition = new Vector3(-118, -237, 0);
        transform.localScale = new Vector3(1, 1, 1);

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(20, 20);

        Image image = GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(spritePath);
        image.color = Color.white;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => characterFactory.GetComponent<CharacterFactory>().InitializeFactory());
        button.interactable = true;
    }
}
