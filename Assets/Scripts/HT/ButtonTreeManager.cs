using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class ButtonTreeManager : MonoBehaviour
{
    public TreeNode root;
    public GameObject simpleButton;
    public CharacterManager characterManager;
    public string imagePath  = "Imports/Characters/3/Idle/Idle (1).png";

    public float verticalSpacing = 50f;

    void Start()
    {
        CreationOfTheTreeLayout();
    }

    public TreeNode CreateButton(Character newCharacter, TreeNode parent)
    {
        // // Créer un nouvel objet pour le bouton
        // GameObject buttonObject = new GameObject(buttonText);
        // TreeNode buttonNode = buttonObject.AddComponent<TreeNode>();

        // // Ajouter un bouton Unity UI
        // buttonNode.button = buttonObject.AddComponent<Button>();
        // buttonNode.button.GetComponentInChildren<Text>().text = buttonText;


        GameObject newPlayerButton = Instantiate(simpleButton, transform);
        TreeNode buttonNode = newPlayerButton.AddComponent<TreeNode>();
        newPlayerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        newPlayerButton.transform.localPosition = new Vector3(-147, -100, 0);
        newPlayerButton.transform.localScale = new Vector3(1, 1, 1);
        newPlayerButton.name = newCharacter.name;
        newPlayerButton.GetComponent<Button>().onClick.RemoveAllListeners();
        newPlayerButton.GetComponent<Button>().onClick.AddListener(() => characterManager.DisplayCharacterDetails(newCharacter.name));
        string filePath = Path.Combine(Application.dataPath, imagePath);
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Load image data into the texture
            newPlayerButton.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }

        // Configurer la hiérarchie parent-enfant
        if (parent != null)
        {
            buttonNode.parent = parent;
            buttonNode.depth = parent.depth + 1;
            parent.children.Add(buttonNode);
        }

        // Mettre à jour la disposition de l'arbre
        UpdateTreeLayout(buttonNode);
        return buttonNode;
    }


    void UpdateTreeLayout(TreeNode node)
    {
        // Calculer la position horizontale en fonction de la profondeur et du nombre de frères
        float horizontalSpacing = 100f;
        float horizontalPosition = (node == root) ? 0 : node.depth * horizontalSpacing;
        float verticalPosition = -node.depth * verticalSpacing;

        // Positionner le bouton
        RectTransform rectTransform = node.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(horizontalPosition, verticalPosition);

        // Positionner les enfants récursivement
        for (int i = 0; i < node.children.Count; i++)
        {
            float childHorizontalPosition = (i - (node.children.Count - 1) / 2.0f) * horizontalSpacing;
            rectTransform = node.children[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(horizontalPosition + childHorizontalPosition, verticalPosition - verticalSpacing);
            UpdateTreeLayout(node.children[i]);
        }

    }


    void CreationOfTheTreeLayout()
    {
        UpdateTreeLayout(root);



        //for all the children of the gameobject read UpdateTreeLayout
        for (int i = 0; i < root.children.Count; i++) 
        {
            Debug.Log(root.children[i].name);
           UpdateTreeLayout(root.children[i]);
        }

    }
}
