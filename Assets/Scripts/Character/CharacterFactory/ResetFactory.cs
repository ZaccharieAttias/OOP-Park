using UnityEngine;
using UnityEngine.UI;


public class ResetFactory : MonoBehaviour
{
    public void Start() { InitializeGameObject(); }
    private void InitializeGameObject()
    {
        GameObject characterFactory = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/CharacterFactory");

        name = "ResetFactory";
        transform.localPosition = new Vector3(-329, -431, 0);
        
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(35, 35);

        Image image = GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Sprites/Icons/Restart");
        image.color = Color.white;
        
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => characterFactory.GetComponent<CharacterFactory>().ResetFactory());
        button.interactable = false;

        gameObject.SetActive(false);
    }
}
