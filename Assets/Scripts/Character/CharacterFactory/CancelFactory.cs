using UnityEngine;
using UnityEngine.UI;


public class CancelFactory : MonoBehaviour
{
    private const string spritePath = "Sprites/Icons/Delete";
    private const string characterFactoryPath = "Canvas/HTMenu/Menu/Characters/Tree/Buttons/CharacterFactory";


    public void Start() { InitializeGameObject(); }

    private void InitializeGameObject()
    {
        GameObject characterFactory = GameObject.Find(characterFactoryPath);

        name = "CancelFactory";
        
        transform.SetParent(characterFactory.transform);
        transform.localPosition = new Vector3(-168, -237, 0);
        transform.localScale = new Vector3(1, 1, 1);

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(20, 20);
        
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(spritePath);
        image.color = Color.white;
        
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => characterFactory.GetComponent<CharacterFactory>().CancelFactory());
        button.interactable = false;

        gameObject.SetActive(false);
    }
}
