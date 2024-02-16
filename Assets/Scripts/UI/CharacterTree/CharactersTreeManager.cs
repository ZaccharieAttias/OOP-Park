using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class CharactersTreeManager : MonoBehaviour
{
    public int NodeSize;
    public int SiblingDistance;
    public int TreeDistance;
    public int DepthDistance;

    public ScrollRect ScrollView;
    public GameObject AllGameObject;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        NodeSize = 100;
        SiblingDistance = 100;
        TreeDistance = 5;
        DepthDistance = 150;

        ScrollView = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/ScrollView").GetComponent<ScrollRect>();
        AllGameObject = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/ScrollView/ViewPort/All");
    }

    public void BuildTree(Character root, Character latest)
    {
        ResetLines();
        CalculateNodePositions(root);
        DrawLines(root);
        CentrelizeTree(root);
        UpdateContentsSizes(root);

        ScrollViewFocus.FocusOnItem(ScrollView, latest.CharacterButton.Button.GetComponent<RectTransform>());
        StartCoroutine(ScrollView.FocusOnItemCoroutine(latest.CharacterButton.Button.GetComponent<RectTransform>(), 1.0f));
    }
    private void ResetLines()
    {
        foreach (Transform child in AllGameObject.transform)
            if (child.gameObject.name.Contains("Line"))
                Destroy(child.gameObject);
    }
    private void CalculateNodePositions(Character root)
    {
        InitializeNodes(root, 0);
        CalculateInitialX(root);
        CheckAllChildrenOnScreen(root);
        CalculateFinalPositions(root, 0);
        UpdateNodePositions(root);
    }
    private void InitializeNodes(Character character, int depth)
    {
        character.CharacterButton.X = 0;
        character.CharacterButton.Y = depth * -DepthDistance;
        character.CharacterButton.Mod = 0;
        character.CharacterButton.Depth = depth;

        foreach (Character child in character.Childrens) InitializeNodes(child, depth + 1);
    }
    private void CalculateInitialX(Character character)
    {
        foreach (Character child in character.Childrens) 
            CalculateInitialX(child);
 
        if (character.IsLeaf())
            character.CharacterButton.X = character.IsLeftMost() ? 0 : character.GetPreviousSibling().CharacterButton.X + NodeSize + SiblingDistance;

        else if (character.Childrens.Count == 1)
        {
            CharacterButton leftChild = character.GetLeftMostChild().CharacterButton;
            
            character.CharacterButton.X = character.IsLeftMost() ? leftChild.X : character.GetPreviousSibling().CharacterButton.X + NodeSize + SiblingDistance;
            character.CharacterButton.Mod = character.CharacterButton.X - leftChild.X;
        }

        else
        {
            CharacterButton leftChild = character.GetLeftMostChild().CharacterButton;
            CharacterButton rightChild = character.GetRightMostChild().CharacterButton;
            
            int desiredX = (leftChild.X + rightChild.X) / 2;
            
            character.CharacterButton.X = character.IsLeftMost() ? desiredX : character.GetPreviousSibling().CharacterButton.X + NodeSize + SiblingDistance;
            character.CharacterButton.Mod = character.CharacterButton.X - desiredX;
        }

        if (character.Childrens.Count > 0 && character.IsLeftMost() == false)
            CheckForConflicts(character);
    }
    private void CheckAllChildrenOnScreen(Character character)
    {
        Dictionary<int, float> nodeContour = new();
        GetLeftContour(character, 0, ref nodeContour);

        float shiftAmount = nodeContour.Values.Min();
        if (shiftAmount < 0)
        {
            character.CharacterButton.X += (int)Math.Abs(shiftAmount);
            character.CharacterButton.Mod += (int)Math.Abs(shiftAmount);
        }
    }
    private void CalculateFinalPositions(Character character, int modSum)
    {
        character.CharacterButton.X += modSum;
        modSum += character.CharacterButton.Mod;

        foreach (Character child in character.Childrens) CalculateFinalPositions(child, modSum);

        character.CharacterButton.Y = character.IsLeaf() ? character.CharacterButton.Depth * -DepthDistance : character.Childrens[0].CharacterButton.Y + DepthDistance;
    }
    private void UpdateNodePositions(Character character)
    {
        character.SetTransformPositionX(character.CharacterButton.X - NodeSize / 2);
        character.SetTransformPositionY(character.CharacterButton.Y - NodeSize / 2);

        foreach (Character child in character.Childrens) UpdateNodePositions(child);
    }

    private void CheckForConflicts(Character character)
    {
        float minDistance = TreeDistance + NodeSize;
        float shiftValue = 0;

        Dictionary<int, float> nodeContour = new();
        GetLeftContour(character, 0, ref nodeContour);

        Character sibling = character.GetLeftMostSibling();
        while (sibling != null && sibling != character)
        {
            Dictionary<int, float> siblingContour = new();
            GetRightContour(sibling, 0, ref siblingContour);

            int minLevel = Math.Max(nodeContour.Keys.Min(), siblingContour.Keys.Min());
            for (int level = character.CharacterButton.Y - DepthDistance; level >= minLevel; level -= DepthDistance)
            {
                float distance = nodeContour[level] - siblingContour[level];
                shiftValue = Mathf.Max(minDistance - distance, shiftValue);
            }

            if (shiftValue > 0)
                CenterNodesBetween(sibling, character, shiftValue);

            sibling = sibling.GetNextSibling();
        }

        if (shiftValue > 0)
        {
            character.CharacterButton.X += (int)shiftValue;
            character.CharacterButton.Mod += (int)shiftValue;        
        }
    }
    private void CenterNodesBetween(Character leftNode, Character rightNode, float shiftValue)
    {
        int leftIndex = leftNode.Parents[0].Childrens.IndexOf(leftNode);
        int rightIndex = leftNode.Parents[0].Childrens.IndexOf(rightNode);

        int numNodesBetween = rightIndex - leftIndex - 1;
        if (numNodesBetween > 0)
        {
            int distanceBetweenNodesbefore = Mathf.Abs(rightNode.CharacterButton.X - leftNode.CharacterButton.X) / (numNodesBetween + 1);
            int distanceBetweenNodesafter = (int)Mathf.Abs(rightNode.CharacterButton.X + shiftValue - leftNode.CharacterButton.X) / (numNodesBetween + 1);
            
            int count = 1;
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                Character middleNode = leftNode.Parents[0].Childrens[i];

                int desiredXafter = leftNode.CharacterButton.X + (distanceBetweenNodesafter * count);
                int desiredX = leftNode.CharacterButton.X + (distanceBetweenNodesbefore * count);
                int offset = desiredXafter - desiredX;
                
                middleNode.CharacterButton.X += offset;
                middleNode.CharacterButton.Mod += offset;
                
                count++;
            }

            CheckForConflicts(leftNode);
        }
    }

    private void GetLeftContour(Character character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        int characterY = character.CharacterButton.Y;
        float characterX = character.CharacterButton.X + modSum;

        if (nodeContour.ContainsKey(characterY) == false) nodeContour.Add(characterY, characterX);
        else nodeContour[characterY] = Math.Min(nodeContour[characterY], characterX);

        modSum += character.CharacterButton.Mod;

        foreach (Character child in character.Childrens) GetLeftContour(child, modSum, ref nodeContour);
    }
    private void GetRightContour(Character character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        int characterY = character.CharacterButton.Y;
        float characterX = character.CharacterButton.X + modSum;

        if (nodeContour.ContainsKey(characterY) == false) nodeContour.Add(characterY, characterX);
        else  nodeContour[characterY] = Math.Max(nodeContour[characterY], characterX);

        modSum += character.CharacterButton.Mod;

        foreach (Character child in character.Childrens) GetRightContour(child, modSum, ref nodeContour);
    }
    
    private void DrawLines(Character character)
    {
        RectTransform rectTransform = character.CharacterButton.Button.GetComponent<RectTransform>();

        if (character.Parents.Count != 0)
        {
            RectTransform parentRectTransform = character.Parents[0].CharacterButton.Button.GetComponent<RectTransform>();
            
            Vector2 nodeTopMiddle = new(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 50 - 8);
            Vector2 nodeAboveMiddle = new(nodeTopMiddle.x, (rectTransform.anchoredPosition.y + parentRectTransform.anchoredPosition.y) / 2);
            
            CreateLine(nodeAboveMiddle, nodeTopMiddle);
        }

        if (character.Childrens.Count > 0)
        {
            RectTransform leftChildrenRectTransform = character.Childrens[0].CharacterButton.Button.GetComponent<RectTransform>();
            
            Vector2 nodeBottomMiddle = new(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - 50 + 4);
            Vector2 nodeBelowMiddle = new(nodeBottomMiddle.x, (rectTransform.anchoredPosition.y + leftChildrenRectTransform.anchoredPosition.y) / 2);

            CreateLine(nodeBottomMiddle, nodeBelowMiddle);
            
            if (character.Childrens.Count > 1)
            {
                RectTransform rightChildrenRectTransform = character.Childrens[^1].CharacterButton.Button.GetComponent<RectTransform>();
                
                Vector2 nodeLeftMiddle = new(leftChildrenRectTransform.anchoredPosition.x - 2.435f, (leftChildrenRectTransform.anchoredPosition.y + rectTransform.anchoredPosition.y) / 2);
                Vector2 nodeRightMiddle = new(rightChildrenRectTransform.anchoredPosition.x + 2.435f, (rightChildrenRectTransform.anchoredPosition.y + rectTransform.anchoredPosition.y) / 2);

                CreateLine(nodeLeftMiddle, nodeRightMiddle);
            }
        }

        foreach (Character child in character.Childrens)
            DrawLines(child);
    }
    private void CreateLine(Vector2 startPoint, Vector2 endPoint)
    {
        GameObject line = new GameObject("Line", typeof(Image));

        Transform transform = line.GetComponent<Transform>();
        transform.SetParent(AllGameObject.transform);
        transform.localScale = new Vector3(1, 1, 1);

        float distance = Vector2.Distance(startPoint, endPoint);
        float angle = Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x) * Mathf.Rad2Deg;
        RectTransform rectTransform = line.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = (startPoint + endPoint) / 2;
        rectTransform.sizeDelta = new Vector2(distance, 5);
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        line.GetComponent<Image>().color = Color.red;
    }

    private void CentrelizeTree(Character root)
    {
        float shiftValue = -root.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.x;
        foreach (Transform child in AllGameObject.transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + shiftValue, rectTransform.anchoredPosition.y);
        }
    }
    private void UpdateContentsSizes(Character root)
    {        
        Character TopNode = root, BottomNode = root, LeftNode = root, RightNode = root;
        Queue<Character> queue = new();
        queue.Enqueue(root);
        
        while (queue.Count > 0)
        {
            Character currentCharacter = queue.Dequeue();

            if (currentCharacter.CharacterButton.Y > TopNode.CharacterButton.Y) TopNode = currentCharacter;
            if (currentCharacter.CharacterButton.Y < BottomNode.CharacterButton.Y) BottomNode = currentCharacter;
            if (currentCharacter.CharacterButton.X < LeftNode.CharacterButton.X) LeftNode = currentCharacter;
            if (currentCharacter.CharacterButton.X > RightNode.CharacterButton.X) RightNode = currentCharacter;

            foreach (Character child in currentCharacter.Childrens) queue.Enqueue(child);
        }

        float contentWidth = Math.Abs(RightNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.x) + Math.Abs(LeftNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.x) + NodeSize;
        float contentHeight = Math.Abs(TopNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.y) + Math.Abs(BottomNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.y) + NodeSize;
        float defaultWidth = 1180;
        float defaultHeight = 674;
        
        contentWidth = Mathf.Max(contentWidth, defaultWidth);
        contentHeight = Mathf.Max(contentHeight, defaultHeight);

        RectTransform allRectTransform = AllGameObject.GetComponent<RectTransform>();
        allRectTransform.sizeDelta = new Vector2(contentWidth, contentHeight);
        allRectTransform.anchoredPosition = new Vector2(contentWidth / 2, contentHeight / 2);
    }
}
