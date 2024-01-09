using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class TreeBuilder : MonoBehaviour
{
    public Character Root;
    
    public GameObject AllGameObject;

    public GameObject _tempObject1;
    public GameObject _tempObject2;

    public static int NodeSize = 40;
    public static int SiblingDistance = 35;
    public static int TreeDistance = 5;

    public ScrollRect ScrollView;

    public void Start() { InitializeProperties(); }

    private void InitializeProperties()
    {
        Root = null;

        AllGameObject = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Scroll View/Viewport/All");
        ScrollView = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Scroll View").GetComponent<ScrollRect>();
        
        _tempObject1 = new GameObject("temp1", typeof(RectTransform));
        _tempObject1.transform.SetParent(AllGameObject.transform);

        _tempObject2 = new GameObject("temp2", typeof(RectTransform));
        _tempObject2.transform.SetParent(AllGameObject.transform);
    }

    public void BuildTree()
    {
        ResetLines();
        CalculateNodePositions();
        DrawLines(Root);
        CentrelizeTree(-Root.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.x);
        UpdateContentsSizes();
        //ScrollView.FocusOnItem(Root.CharacterButton.Button.GetComponent<RectTransform>());
        //StartCoroutine(ScrollView.FocusOnItemCoroutine(Root.CharacterButton.Button.GetComponent<RectTransform>(), 1.0f));
    }

    private void ResetLines()
    {
        foreach (Transform child in AllGameObject.transform)
        {
            string childName = child.gameObject.name;
            if (childName != "temp1" && childName != "temp2" && !childName.Contains("Character"))
                Destroy(child.gameObject);
        }
    }

    private void CalculateNodePositions()
    {
        InitializeNodes(Root, 0);
        CalculateInitialX(Root);
        CheckAllChildrenOnScreen(Root);
        CalculateFinalPositions(Root, 0);
        UpdateNodePositions(Root);
    }
    private void InitializeNodes(Character character, int depth)
    {
        character.CharacterButton.X = 0;
        character.CharacterButton.Y = depth * -75;
        character.CharacterButton.Mod = 0;
        character.CharacterButton.Depth = depth;

        foreach (Character child in character.Childrens)
            InitializeNodes(child, depth + 1);
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

        foreach (Character child in character.Childrens)
            CalculateFinalPositions(child, modSum);

        character.CharacterButton.Y = character.IsLeaf() ? character.CharacterButton.Depth * -75 : character.Childrens[0].CharacterButton.Y + 75;
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
            for (int level = character.CharacterButton.Y - 75; level >= minLevel; level -= 75)
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
            var distanceBetweenNodesbefore = Mathf.Abs(rightNode.CharacterButton.X - leftNode.CharacterButton.X) / (numNodesBetween + 1);
            var distanceBetweenNodesafter = Mathf.Abs(rightNode.CharacterButton.X + shiftValue - leftNode.CharacterButton.X) / (numNodesBetween + 1);
            
            int count = 1;
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                Character middleNode = leftNode.Parents[0].Childrens[i];

                int desiredXafter = leftNode.CharacterButton.X + ((int)distanceBetweenNodesafter * count);
                int desiredX = leftNode.CharacterButton.X + (distanceBetweenNodesbefore * count);
                int offset = desiredXafter - desiredX;
                
                middleNode.CharacterButton.X += offset;
                middleNode.CharacterButton.Mod += offset;
                
                count++;
            }

            CheckForConflicts(leftNode);
        }
    }
    private void UpdateNodePositions(Character character)
    {
        character.SetTransformPositionX(character.CharacterButton.X - NodeSize / 2);
        character.SetTransformPositionY(character.CharacterButton.Y - NodeSize / 2);

        foreach (Character child in character.Childrens)
            UpdateNodePositions(child);
    }

    private void GetLeftContour(Character character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        int characterY = character.CharacterButton.Y;
        float characterX = character.CharacterButton.X + modSum;

        if (nodeContour.ContainsKey(characterY) == false) nodeContour.Add(characterY, characterX);
        else nodeContour[characterY] = Math.Min(nodeContour[characterY], characterX);

        modSum += character.CharacterButton.Mod;

        foreach (Character child in character.Childrens)
            GetLeftContour(child, modSum, ref nodeContour);
    }
    private void GetRightContour(Character character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        int characterY = character.CharacterButton.Y;
        float characterX = character.CharacterButton.X + modSum;

        if (nodeContour.ContainsKey(characterY) == false) nodeContour.Add(characterY, characterX);
        else  nodeContour[characterY] = Math.Max(nodeContour[characterY], characterX);

        modSum += character.CharacterButton.Mod;

        foreach (Character child in character.Childrens)
            GetRightContour(child, modSum, ref nodeContour);
    }
    
    private void DrawLines(Character character)
    {
        RectTransform rectTransform = character.CharacterButton.Button.GetComponent<RectTransform>();

        if (character.Parents.Count != 0)
        {
            RectTransform parentRectTransform = character.Parents[0].CharacterButton.Button.GetComponent<RectTransform>();

            Vector2 nodeTopMiddle = new(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 16);
            Vector2 nodeAboveMiddle = new(nodeTopMiddle.x, (rectTransform.anchoredPosition.y + parentRectTransform.anchoredPosition.y) / 2);
            
            CreateLine(nodeAboveMiddle, nodeTopMiddle);
        }

        if (character.Childrens.Count > 0)
        {
            RectTransform leftChildrenRectTransform = character.Childrens[0].CharacterButton.Button.GetComponent<RectTransform>();

            Vector2 nodeBottomMiddle = new(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - 18);
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
    private void CreateLine(Vector2 x, Vector2 y)
    {
        GameObject line = new("Line", typeof(Image), typeof(LinesCreator));
        
        Transform transform = line.GetComponent<Transform>();
        transform.SetParent(AllGameObject.transform);
        transform.localScale = new Vector3(1, 1, 1);

        RectTransform rectTransform = line.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        
        _tempObject1.GetComponent<RectTransform>().anchoredPosition = x;
        _tempObject2.GetComponent<RectTransform>().anchoredPosition = y;

        LinesCreator linesCreator = line.GetComponent<LinesCreator>();
        linesCreator.SetPoints(_tempObject1.transform, _tempObject2.transform);
        linesCreator.Settings();
    }

    private void CentrelizeTree(float shiftValue)
    {
        foreach (Transform child in AllGameObject.transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + shiftValue, rectTransform.anchoredPosition.y);
        }
    }

    private void UpdateContentsSizes()
    {
        //if all the nodes are on the screen, then set the size of the content to 383x250 and the position to 1.525879e-05x-125
        
        Character TopNode = Root, BottomNode = Root, LeftNode = Root, RightNode = Root;
        Queue<Character> queue = new();
        queue.Enqueue(Root);
        while (queue.Count > 0)
        {
            Character currentCharacter = queue.Dequeue();

            if (currentCharacter.CharacterButton.Y > TopNode.CharacterButton.Y)
                TopNode = currentCharacter;
            if (currentCharacter.CharacterButton.Y < BottomNode.CharacterButton.Y)
                BottomNode = currentCharacter;
            if (currentCharacter.CharacterButton.X < LeftNode.CharacterButton.X)
                LeftNode = currentCharacter;
            if (currentCharacter.CharacterButton.X > RightNode.CharacterButton.X)
                RightNode = currentCharacter;

            foreach (Character child in currentCharacter.Childrens)
                queue.Enqueue(child);
        }

        RectTransform allRectTransform = AllGameObject.GetComponent<RectTransform>();

        float contentWidth = Math.Abs(RightNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.x) + Math.Abs(LeftNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.x) + NodeSize;
        float contentHeight = Math.Abs(TopNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.y) + Math.Abs(BottomNode.CharacterButton.Button.GetComponent<RectTransform>().anchoredPosition.y) + NodeSize;

        if (contentWidth < 380)
            contentWidth = 380;
        if (contentHeight < 250)
            contentHeight = 250;

        // //search for scrollbars in the scrollview
        // float originalVerticalSizeScrollbars = ScrollView.verticalScrollbar.size;
        // float originalHorizontalSizeScrollbars = ScrollView.horizontalScrollbar.size;
        
        allRectTransform.sizeDelta = new Vector2(contentWidth, contentHeight);

        allRectTransform.anchoredPosition = new Vector2(contentWidth / 2, contentHeight / 2);
    
        // ScrollView.verticalScrollbar.size = originalVerticalSizeScrollbars;
        // ScrollView.horizontalScrollbar.size = originalHorizontalSizeScrollbars;
    }
}
