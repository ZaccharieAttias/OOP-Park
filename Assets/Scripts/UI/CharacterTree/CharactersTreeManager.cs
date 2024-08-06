using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class CharactersTreeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public ScrollRect ScrollView;
    public GameObject AllGameObject;

    [Header("Tree Configuration")]
    public int NodeSize;
    public int SiblingDistance;
    public int TreeDistance;
    public int DepthDistance;


    public void Start()
    {
        InitializeUIElements();
        InitializeTreeConfiguration();
    }
    public void InitializeUIElements()
    {
        ScrollView = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView").GetComponent<ScrollRect>();
        AllGameObject = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView/ViewPort/All");
    }
    public void InitializeTreeConfiguration()
    {
        NodeSize = 100;
        SiblingDistance = 100;
        TreeDistance = 5;
        DepthDistance = 150;
    }

    public void BuildTree(CharacterB root, CharacterB latest)
    {
        ResetLines();
        CalculateNodePositions(root);
        DrawLines(root);
        CentralizeTree(root);
        UpdateContentsSizes(root);
        FocusOnLatestCharacter(latest);
    }
    public void ResetLines()
    {
        foreach (Transform child in AllGameObject.transform)
        {
            if (child.gameObject.name.Contains("Line")) Destroy(child.gameObject);
        }
    }
    public void CalculateNodePositions(CharacterB root)
    {
        InitializeNodes(root, 0);
        CalculateInitialX(root);
        AdjustForScreenBounds(root);
        CalculateFinalPositions(root, 0);
        UpdateNodePositions(root);
    }
    public void InitializeNodes(CharacterB character, int depth)
    {
        character.CharacterButton.X = 0;
        character.CharacterButton.Y = depth * -DepthDistance;
        character.CharacterButton.Mod = 0;
        character.CharacterButton.Depth = depth;

        foreach (CharacterB child in character.Childrens)
        {
            InitializeNodes(child, depth + 1);
        }
    }
    public void CalculateInitialX(CharacterB character)
    {
        foreach (CharacterB child in character.Childrens)
        {
            CalculateInitialX(child);
        }

        if (character.IsLeaf())
        {
            character.CharacterButton.X = character.IsLeftMost() ? 0 : character.GetPreviousSibling().CharacterButton.X + NodeSize + SiblingDistance;
        }

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

        if (character.Childrens.Count > 0 && !character.IsLeftMost())
        {
            ResolveConflicts(character);
        }
    }
    public void AdjustForScreenBounds(CharacterB character)
    {
        Dictionary<int, float> leftContour = new();
        GetLeftContour(character, 0, ref leftContour);

        float shiftAmount = leftContour.Values.Min();
        if (shiftAmount < 0)
        {
            character.CharacterButton.X += (int)Math.Abs(shiftAmount);
            character.CharacterButton.Mod += (int)Math.Abs(shiftAmount);
        }
    }
    public void CalculateFinalPositions(CharacterB character, int modSum)
    {
        character.CharacterButton.X += modSum;
        modSum += character.CharacterButton.Mod;

        foreach (CharacterB child in character.Childrens)
        {
            CalculateFinalPositions(child, modSum);
        }

        if (!character.IsLeaf()) character.CharacterButton.Y = character.Childrens[0].CharacterButton.Y + DepthDistance;
    }
    public void UpdateNodePositions(CharacterB character)
    {
        character.SetTransformPositionX(character.CharacterButton.X - NodeSize / 2);
        character.SetTransformPositionY(character.CharacterButton.Y - NodeSize / 2);

        foreach (CharacterB child in character.Childrens)
        {
            UpdateNodePositions(child);
        }
    }
    public void ResolveConflicts(CharacterB character)
    {
        float minDistance = TreeDistance + NodeSize;
        float shiftValue = 0;

        Dictionary<int, float> leftContour = new();
        GetLeftContour(character, 0, ref leftContour);

        CharacterB sibling = character.GetLeftMostSibling();
        while (sibling != null && sibling != character)
        {
            Dictionary<int, float> siblingContour = new();
            GetRightContour(sibling, 0, ref siblingContour);

            int minLevel = Math.Max(leftContour.Keys.Min(), siblingContour.Keys.Min());
            for (int level = character.CharacterButton.Y - DepthDistance; level >= minLevel; level -= DepthDistance)
            {
                float distance = leftContour[level] - siblingContour[level];
                shiftValue = Mathf.Max(minDistance - distance, shiftValue);
            }

            if (shiftValue > 0)
            {
                CenterNodesBetween(sibling, character, shiftValue);
            }

            sibling = sibling.GetNextSibling();
        }

        if (shiftValue > 0)
        {
            character.CharacterButton.X += (int)shiftValue;
            character.CharacterButton.Mod += (int)shiftValue;
        }
    }
    public void CenterNodesBetween(CharacterB leftNode, CharacterB rightNode, float shiftValue)
    {
        int leftIndex = leftNode.Parent.Childrens.IndexOf(leftNode);
        int rightIndex = leftNode.Parent.Childrens.IndexOf(rightNode);

        int numNodesBetween = rightIndex - leftIndex - 1;
        if (numNodesBetween > 0)
        {
            int distanceBetweenNodesBefore = Mathf.Abs(rightNode.CharacterButton.X - leftNode.CharacterButton.X) / (numNodesBetween + 1);
            int distanceBetweenNodesAfter = (int)Mathf.Abs(rightNode.CharacterButton.X + shiftValue - leftNode.CharacterButton.X) / (numNodesBetween + 1);

            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                CharacterB middleNode = leftNode.Parent.Childrens[i];

                int desiredXAfter = leftNode.CharacterButton.X + (distanceBetweenNodesAfter * (i - leftIndex));
                int desiredX = leftNode.CharacterButton.X + (distanceBetweenNodesBefore * (i - leftIndex));
                int offset = desiredXAfter - desiredX;

                middleNode.CharacterButton.X += offset;
                middleNode.CharacterButton.Mod += offset;
            }

            ResolveConflicts(leftNode);
        }
    }
    public void GetLeftContour(CharacterB character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        int characterY = character.CharacterButton.Y;
        float characterX = character.CharacterButton.X + modSum;

        if (!nodeContour.ContainsKey(characterY)) nodeContour.Add(characterY, characterX);
        else nodeContour[characterY] = Math.Min(nodeContour[characterY], characterX);

        modSum += character.CharacterButton.Mod;

        foreach (CharacterB child in character.Childrens)
        {
            GetLeftContour(child, modSum, ref nodeContour);
        }
    }
    public void GetRightContour(CharacterB character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        int characterY = character.CharacterButton.Y;
        float characterX = character.CharacterButton.X + modSum;

        if (!nodeContour.ContainsKey(characterY)) nodeContour.Add(characterY, characterX);
        else nodeContour[characterY] = Math.Max(nodeContour[characterY], characterX);

        modSum += character.CharacterButton.Mod;

        foreach (CharacterB child in character.Childrens)
        {
            GetRightContour(child, modSum, ref nodeContour);
        }
    }
    public void DrawLines(CharacterB character)
    {
        RectTransform rectTransform = character.CharacterButton.Button.GetComponent<RectTransform>();

        if (character.Parent is not null)
        {
            RectTransform parentRectTransform = character.Parent.CharacterButton.Button.GetComponent<RectTransform>();

            Vector2 nodeTopMiddle = new(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 50 - 2);
            Vector2 nodeAboveMiddle = new(nodeTopMiddle.x, (rectTransform.anchoredPosition.y + parentRectTransform.anchoredPosition.y) / 2);

            CreateLine(nodeAboveMiddle, nodeTopMiddle);
        }

        if (character.Childrens.Count > 0)
        {
            RectTransform leftChildrenRectTransform = character.Childrens[0].CharacterButton.Button.GetComponent<RectTransform>();

            Vector2 nodeBottomMiddle = new(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - 50 + 15);
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

        foreach (CharacterB child in character.Childrens)
        {
            DrawLines(child);
        }
    }
    public void CreateLine(Vector2 startPoint, Vector2 endPoint)
    {
        GameObject line = new GameObject("Line", typeof(Image));
        Transform transform = line.GetComponent<Transform>();
        transform.SetParent(AllGameObject.transform);
        transform.localScale = Vector3.one;

        float distance = Vector2.Distance(startPoint, endPoint);
        float angle = Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x) * Mathf.Rad2Deg;
        RectTransform rectTransform = line.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = (startPoint + endPoint) / 2;
        rectTransform.sizeDelta = new Vector2(distance, 5);
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        line.GetComponent<Image>().color = new Color32(129, 176, 60, 255);
    }
    public void CentralizeTree(CharacterB root)
    {
        float shiftValue = -root.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.x;
        foreach (Transform child in AllGameObject.transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + shiftValue, rectTransform.anchoredPosition.y);
        }
    }
    public void UpdateContentsSizes(CharacterB root)
    {
        CharacterB topNode = root, bottomNode = root, leftNode = root, rightNode = root;
        Queue<CharacterB> queue = new();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            CharacterB currentCharacter = queue.Dequeue();

            if (currentCharacter.CharacterButton.Y > topNode.CharacterButton.Y) topNode = currentCharacter;
            if (currentCharacter.CharacterButton.Y < bottomNode.CharacterButton.Y) bottomNode = currentCharacter;
            if (currentCharacter.CharacterButton.X < leftNode.CharacterButton.X) leftNode = currentCharacter;
            if (currentCharacter.CharacterButton.X > rightNode.CharacterButton.X) rightNode = currentCharacter;

            foreach (CharacterB child in currentCharacter.Childrens)
            {
                queue.Enqueue(child);
            }
        }

        float contentWidth = Math.Abs(rightNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.x) + Math.Abs(leftNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.x) + NodeSize;
        float contentHeight = Math.Abs(topNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.y) + Math.Abs(bottomNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.y) + NodeSize;
        float defaultWidth = 1083f;
        float defaultHeight = 542f;

        contentWidth = Mathf.Max(contentWidth, defaultWidth);
        contentHeight = Mathf.Max(contentHeight, defaultHeight);

        RectTransform allRectTransform = AllGameObject.GetComponent<RectTransform>();
        allRectTransform.sizeDelta = new Vector2(contentWidth, contentHeight);
        allRectTransform.anchoredPosition = new Vector2(contentWidth / 2, contentHeight / 2);
    }
    public void FocusOnLatestCharacter(CharacterB latest)
    {
        RectTransform latestRectTransform = latest.CharacterButton.Button.GetComponent<RectTransform>();
        ScrollViewFocus.FocusOnItem(ScrollView, latestRectTransform);
        StartCoroutine(ScrollView.FocusOnItemCoroutine(latest.CharacterButton.Button.GetComponent<RectTransform>(), 1.0f));
    }
}