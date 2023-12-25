using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;


public class ButtonTreeManager : MonoBehaviour
{
    private readonly string _buttonPrefabPath = "Prefabs/Buttons/Character";
    public TreeNode root;
    public GameObject buttonPrefab;
    public CharacterManager characterManager;
    public string imagePath = "Imports/Characters/3/Idle/Idle (1).png";

    public float verticalSpacing = 50f;
    public float horizontalSpacing = 100f;
    public float horizontalPosition;
    public float verticalPosition;
    


    public void startButtonTreeManager(TreeNode root, CharacterManager characterManager)
    {
        this.root = root;
        this.characterManager = characterManager;
        buttonPrefab = Resources.Load<GameObject>(_buttonPrefabPath);
    }

    public void CreateButton(TreeNode characterNode)
    {
        GameObject newPlayerButton = Instantiate(buttonPrefab, transform);
        newPlayerButton.tag = "CharacterButton";

        TreeNode newPlayerScript = newPlayerButton.AddComponent<TreeNode>();
        newPlayerScript.character = characterNode.character;
        newPlayerScript.parent = characterNode.parent;
        newPlayerScript.children = characterNode.children;
        newPlayerScript.depth = characterNode.depth;

        newPlayerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        newPlayerButton.transform.localScale = new Vector3(1, 1, 1);
        newPlayerButton.name = characterNode.character.name;
        newPlayerButton.GetComponent<Button>().onClick.RemoveAllListeners();
        newPlayerButton.GetComponent<Button>().onClick.AddListener(() => characterManager.DisplayCharacterDetails(characterNode.character.name));
        string filePath = Path.Combine(Application.dataPath, imagePath);
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Load image data into the texture
            newPlayerButton.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }

        if (newPlayerScript.parent != null)
        {
            // This is TreeNode
            // public Character character;
            // public List<TreeNode> parent;
            // public List<TreeNode> children;
            // public int depth;
            // I want you to update the parent's children list
            
            foreach (TreeNode parent in newPlayerScript.parent)
            {
                parent.children.Add(newPlayerScript);
            }
            
            newPlayerScript.depth = newPlayerScript.parent[0].depth + 1;
        }
        if(newPlayerScript.character.name == root.character.name)
        {
            Debug.Log("Root");
            horizontalPosition = -147;
            verticalPosition = 52;
            RectTransform rectTransform = newPlayerScript.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(horizontalPosition, verticalPosition, 0f);
        }
        else
        {
            horizontalPosition = newPlayerScript.depth * horizontalSpacing;
            verticalPosition = -newPlayerScript.depth * verticalSpacing;
            UpdateTreeLayout(newPlayerScript, new Vector3(horizontalPosition, verticalPosition, 0f));
        }
    }


    void UpdateTreeLayout(TreeNode node, Vector3 position)
    {
        node.transform.position = position;

        for (int i = 0; i < node.children.Count; i++)
        {
            // Ajouter un décalage pour éviter que les objets ne se chevauchent
            Vector3 childPosition = position + new Vector3(2f * i, -2f, 0f);

            UpdateTreeLayout(node.children[i], childPosition);
        }
        // // Calculer la position horizontale en fonction de la profondeur et du nombre de frères
        // horizontalPosition = (node == root) ? 0 : node.depth * horizontalSpacing;
        // verticalPosition = -node.depth * verticalSpacing;

        // // Positionner le bouton
        // RectTransform rectTransform = node.GetComponent<RectTransform>();
        // rectTransform.anchoredPosition = new Vector2(horizontalPosition, verticalPosition);

        // // Positionner les enfants récursivement
        // for (int i = 0; i < node.children.Count; i++)
        // {
        //     float childHorizontalPosition = (i - (node.children.Count - 1) / 2.0f) * horizontalSpacing;
        //     rectTransform = node.children[i].GetComponent<RectTransform>();
        //     rectTransform.anchoredPosition = new Vector2(horizontalPosition + childHorizontalPosition, verticalPosition - verticalSpacing);
        //     UpdateTreeLayout(node.children[i]);
        // }

    }


}
