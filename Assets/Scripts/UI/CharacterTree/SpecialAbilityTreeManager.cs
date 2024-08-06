using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SpecialAbilityTreeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public ScrollRect ScrollView;
    public GameObject AllGameObject;

    [Header("Node Settings")]
    public int X_NodeSize;
    public int Y_NodeSize;
    public int SiblingDistance;
    public int TreeDistance;
    public int DepthDistance;


    public void Start()
    {
        InitializeUIElements();
        InitializeNodeSettings();
    }
    public void InitializeUIElements()
    {
        ScrollView = GameObject.Find("Canvas/Popups/SpecialAbilityTree/Background/Foreground/Buttons/ScrollView").GetComponent<ScrollRect>();
        AllGameObject = GameObject.Find("Canvas/Popups/SpecialAbilityTree/Background/Foreground/Buttons/ScrollView/ViewPort/All");
    }
    public void InitializeNodeSettings()
    {
        X_NodeSize = 120;
        Y_NodeSize = 50;
        SiblingDistance = 20;
        TreeDistance = 80;
        DepthDistance = 150;
    }

    public void BuildTree(SpecialAbilityObject root)
    {
        ResetLines();
        CalculateNodePositions(root);
        DrawLines(root);
        CentralizeTree(root);
        UpdateContentSizes(root);

        FocusOnItem(root.Button.GetComponent<RectTransform>());
    }
    public void ResetLines()
    {
        foreach (Transform child in AllGameObject.transform)
        {
            if (child.gameObject.name.Contains("Line")) Destroy(child.gameObject);
        }
    }
    public void CalculateNodePositions(SpecialAbilityObject root)
    {
        InitializeNodes(root, 0);
        CalculateInitialX(root);
        EnsureAllChildrenOnScreen(root);
        CalculateFinalPositions(root, 0);
        UpdateNodePositions(root);
    }
    public void InitializeNodes(SpecialAbilityObject speAbility, int depth)
    {
        speAbility.X = 0;
        speAbility.Y = depth * -DepthDistance;
        speAbility.Mod = 0;
        speAbility.Depth = depth;

        foreach (SpecialAbilityObject child in speAbility.Childrens)
        {
            InitializeNodes(child, depth + 1);
        }
    }
    public void CalculateInitialX(SpecialAbilityObject speAbility)
    {
        foreach (SpecialAbilityObject child in speAbility.Childrens)
        {
            CalculateInitialX(child);
        }

        if (speAbility.IsLeaf())
        {
            speAbility.X = speAbility.IsLeftMost() ? 0 : speAbility.GetPreviousSibling().X + X_NodeSize + SiblingDistance;
        }
        else if (speAbility.Childrens.Count == 1)
        {
            SpecialAbilityObject leftChild = speAbility.GetLeftMostChild();
            speAbility.X = speAbility.IsLeftMost() ? leftChild.X : speAbility.GetPreviousSibling().X + X_NodeSize + SiblingDistance;
            speAbility.Mod = speAbility.X - leftChild.X;
        }
        else
        {
            SpecialAbilityObject leftChild = speAbility.GetLeftMostChild();
            SpecialAbilityObject rightChild = speAbility.GetRightMostChild();
            int desiredX = (leftChild.X + rightChild.X) / 2;
            speAbility.X = speAbility.IsLeftMost() ? desiredX : speAbility.GetPreviousSibling().X + X_NodeSize + SiblingDistance;
            speAbility.Mod = speAbility.X - desiredX;
        }

        if (speAbility.Childrens.Count > 0 && !speAbility.IsLeftMost())
        {
            ResolveConflicts(speAbility);
        }
    }
    public void EnsureAllChildrenOnScreen(SpecialAbilityObject speAbility)
    {
        Dictionary<int, float> nodeContour = new();
        GetLeftContour(speAbility, 0, ref nodeContour);

        float shiftAmount = nodeContour.Values.Min();
        if (shiftAmount < 0)
        {
            speAbility.X += (int)Math.Abs(shiftAmount);
            speAbility.Mod += (int)Math.Abs(shiftAmount);
        }
    }
    public void CalculateFinalPositions(SpecialAbilityObject speAbility, int modSum)
    {
        speAbility.X += modSum;
        modSum += speAbility.Mod;

        foreach (SpecialAbilityObject child in speAbility.Childrens)
        {
            CalculateFinalPositions(child, modSum);
        }

        speAbility.Y = speAbility.IsLeaf() ? speAbility.Depth * -DepthDistance : speAbility.Childrens[0].Y + DepthDistance;
    }
    public void UpdateNodePositions(SpecialAbilityObject speAbility)
    {
        speAbility.SetTransformPositionX(speAbility.X - X_NodeSize / 2);
        speAbility.SetTransformPositionY(speAbility.Y - Y_NodeSize / 2);

        foreach (SpecialAbilityObject child in speAbility.Childrens)
        {
            UpdateNodePositions(child);
        }
    }
    public void ResolveConflicts(SpecialAbilityObject speAbility)
    {
        float minDistance = TreeDistance + X_NodeSize;
        float shiftValue = 0;

        Dictionary<int, float> nodeContour = new();
        GetLeftContour(speAbility, 0, ref nodeContour);

        SpecialAbilityObject sibling = speAbility.GetLeftMostSibling();
        while (sibling != null && sibling != speAbility)
        {
            Dictionary<int, float> siblingContour = new();
            GetRightContour(sibling, 0, ref siblingContour);

            int minLevel = Math.Max(nodeContour.Keys.Min(), siblingContour.Keys.Min());
            for (int level = speAbility.Y - DepthDistance; level >= minLevel; level -= DepthDistance)
            {
                float distance = nodeContour[level] - siblingContour[level];
                shiftValue = Mathf.Max(minDistance - distance, shiftValue);
            }

            if (shiftValue > 0)
            {
                CenterNodesBetween(sibling, speAbility, shiftValue);
            }

            sibling = sibling.GetNextSibling();
        }

        if (shiftValue > 0)
        {
            speAbility.X += (int)shiftValue;
            speAbility.Mod += (int)shiftValue;
        }
    }
    public void CenterNodesBetween(SpecialAbilityObject leftNode, SpecialAbilityObject rightNode, float shiftValue)
    {
        int leftIndex = leftNode.Parent.Childrens.IndexOf(leftNode);
        int rightIndex = leftNode.Parent.Childrens.IndexOf(rightNode);

        int numNodesBetween = rightIndex - leftIndex - 1;
        if (numNodesBetween > 0)
        {
            int distanceBetweenNodesBefore = Mathf.Abs(rightNode.X - leftNode.X) / (numNodesBetween + 1);
            int distanceBetweenNodesAfter = (int)Mathf.Abs(rightNode.X + shiftValue - leftNode.X) / (numNodesBetween + 1);

            int count = 1;
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                SpecialAbilityObject middleNode = leftNode.Parent.Childrens[i];
                int desiredXAfter = leftNode.X + (distanceBetweenNodesAfter * count);
                int desiredX = leftNode.X + (distanceBetweenNodesBefore * count);
                int offset = desiredXAfter - desiredX;

                middleNode.X += offset;
                middleNode.Mod += offset;

                count++;
            }

            ResolveConflicts(leftNode);
        }
    }
    public void GetLeftContour(SpecialAbilityObject speAbility, int modSum, ref Dictionary<int, float> nodeContour)
    {
        int characterY = speAbility.Y;
        float characterX = speAbility.X + modSum;

        if (!nodeContour.ContainsKey(characterY)) nodeContour.Add(characterY, characterX);
        else nodeContour[characterY] = Math.Min(nodeContour[characterY], characterX);

        modSum += speAbility.Mod;

        foreach (SpecialAbilityObject child in speAbility.Childrens)
        {
            GetLeftContour(child, modSum, ref nodeContour);
        }
    }
    public void GetRightContour(SpecialAbilityObject speAbility, int modSum, ref Dictionary<int, float> nodeContour)
    {
        int characterY = speAbility.Y;
        float characterX = speAbility.X + modSum;

        if (!nodeContour.ContainsKey(characterY)) nodeContour.Add(characterY, characterX);
        else nodeContour[characterY] = Math.Max(nodeContour[characterY], characterX);

        modSum += speAbility.Mod;

        foreach (SpecialAbilityObject child in speAbility.Childrens)
        {
            GetRightContour(child, modSum, ref nodeContour);
        }
    }
    public void DrawLines(SpecialAbilityObject speAbility)
    {
        RectTransform rectTransform = speAbility.Button.GetComponent<RectTransform>();

        if (speAbility.Parent != null)
        {
            RectTransform parentRectTransform = speAbility.Parent.Button.GetComponent<RectTransform>();
            Vector2 nodeTopMiddle = new(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + Y_NodeSize / 2);
            Vector2 nodeAboveMiddle = new(nodeTopMiddle.x, (rectTransform.anchoredPosition.y + parentRectTransform.anchoredPosition.y) / 2);
            CreateLine(nodeAboveMiddle, nodeTopMiddle);
        }

        if (speAbility.Childrens.Count > 0)
        {
            RectTransform leftChildrenRectTransform = speAbility.Childrens[0].Button.GetComponent<RectTransform>();
            Vector2 nodeBottomMiddle = new(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - Y_NodeSize / 2);
            Vector2 nodeBelowMiddle = new(nodeBottomMiddle.x, (rectTransform.anchoredPosition.y + leftChildrenRectTransform.anchoredPosition.y) / 2);
            CreateLine(nodeBottomMiddle, nodeBelowMiddle);

            if (speAbility.Childrens.Count > 1)
            {
                RectTransform rightChildrenRectTransform = speAbility.Childrens[^1].Button.GetComponent<RectTransform>();
                Vector2 nodeLeftMiddle = new(leftChildrenRectTransform.anchoredPosition.x - 2.435f, (leftChildrenRectTransform.anchoredPosition.y + rectTransform.anchoredPosition.y) / 2);
                Vector2 nodeRightMiddle = new(rightChildrenRectTransform.anchoredPosition.x + 2.435f, (rightChildrenRectTransform.anchoredPosition.y + rectTransform.anchoredPosition.y) / 2);
                CreateLine(nodeLeftMiddle, nodeRightMiddle);
            }
        }

        foreach (SpecialAbilityObject child in speAbility.Childrens)
        {
            DrawLines(child);
        }
    }
    public void CreateLine(Vector2 startPoint, Vector2 endPoint)
    {
        GameObject line = new GameObject("Line", typeof(Image));
        line.transform.SetParent(AllGameObject.transform);
        line.transform.localScale = Vector3.one;

        float distance = Vector2.Distance(startPoint, endPoint);
        float angle = Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x) * Mathf.Rad2Deg;

        RectTransform rectTransform = line.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = (startPoint + endPoint) / 2;
        rectTransform.sizeDelta = new Vector2(distance, 5);
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        line.GetComponent<Image>().color = Color.black;
    }
    public void CentralizeTree(SpecialAbilityObject root)
    {
        float shiftValue = -root.Button.GetComponent<RectTransform>().anchoredPosition.x;
        foreach (Transform child in AllGameObject.transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + shiftValue, rectTransform.anchoredPosition.y);
        }
    }
    public void UpdateContentSizes(SpecialAbilityObject root)
    {
        SpecialAbilityObject topNode = root, bottomNode = root, leftNode = root, rightNode = root;
        Queue<SpecialAbilityObject> queue = new();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            SpecialAbilityObject currentSpeAbility = queue.Dequeue();

            if (currentSpeAbility.Y > topNode.Y) topNode = currentSpeAbility;
            if (currentSpeAbility.Y < bottomNode.Y) bottomNode = currentSpeAbility;
            if (currentSpeAbility.X < leftNode.X) leftNode = currentSpeAbility;
            if (currentSpeAbility.X > rightNode.X) rightNode = currentSpeAbility;

            foreach (SpecialAbilityObject child in currentSpeAbility.Childrens)
            {
                queue.Enqueue(child);
            }
        }

        float contentWidth = Mathf.Abs(rightNode.Button.GetComponent<RectTransform>().anchoredPosition.x) + Mathf.Abs(leftNode.Button.GetComponent<RectTransform>().anchoredPosition.x) + X_NodeSize;
        float contentHeight = Mathf.Abs(topNode.Button.GetComponent<RectTransform>().anchoredPosition.y) + Mathf.Abs(bottomNode.Button.GetComponent<RectTransform>().anchoredPosition.y) + Y_NodeSize;

        float defaultWidth = 1754;
        float defaultHeight = 714;

        contentWidth = Mathf.Max(contentWidth, defaultWidth);
        contentHeight = Mathf.Max(contentHeight, defaultHeight);

        RectTransform allRectTransform = AllGameObject.GetComponent<RectTransform>();
        allRectTransform.sizeDelta = new Vector2(contentWidth, contentHeight);
        allRectTransform.anchoredPosition = new Vector2(contentWidth / 2, contentHeight / 2);
    }
    public void FocusOnItem(RectTransform item)
    {
        StartCoroutine(ScrollView.FocusOnItemCoroutine(item, 1.0f));
    }
}