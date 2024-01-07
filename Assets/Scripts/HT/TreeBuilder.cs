using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;


public class TreeBuilder : MonoBehaviour
{
    public Character Root { get; set; }

    public GameObject LinesGameObject { get; set; }
    public GameObject _tempObject1 { get; set; }
    public GameObject _tempObject2 { get; set; }

    private static int nodeSize = 40;
    private static int siblingDistance = 35;
    private static int treeDistance = 5;


    public void Start() { InitializeProperties(); }

    private void InitializeProperties()
    {
        Root = null;

        LinesGameObject = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Tree/Lines");
        
        _tempObject1 = new GameObject("temp1", typeof(RectTransform));
        _tempObject1.transform.SetParent(LinesGameObject.transform);

        _tempObject2 = new GameObject("temp2", typeof(RectTransform));
        _tempObject2.transform.SetParent(LinesGameObject.transform);
    }

    public void BuildTree()
    {
        ResetLines();
        CalculateNodePositions();
        DrawLines(Root);
    }

    private void ResetLines()
    {
        foreach (Transform child in LinesGameObject.transform)
            if (child.gameObject.name != "temp1" && child.gameObject.name != "temp2")
                Destroy(child.gameObject);
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
        character.X = 0;
        character.Y = depth * -75;
        character.Mod = 0;
        character.Depth = depth;

        foreach (Character child in character.Childrens)
            InitializeNodes(child, depth + 1);
    }

    private void CalculateInitialX(Character character)
    {
        foreach (Character child in character.Childrens)
            CalculateInitialX(child);
 
        if (character.IsLeaf())
        {
            if (character.IsLeftMost() == false) character.X = character.GetPreviousSibling().X + nodeSize + siblingDistance;
            else character.X = 0;
        }

        else if (character.Childrens.Count == 1)
        {
            if (character.IsLeftMost())
                character.X = character.Childrens[0].X;

            else
            {
                character.X = character.GetPreviousSibling().X + nodeSize + siblingDistance;
                character.Mod = character.X - character.Childrens[0].X;
            }
        }

        else
        {
            Character leftChild = character.GetLeftMostChild();
            Character rightChild = character.GetRightMostChild();

            int desiredX =  (leftChild.X + rightChild.X) / 2;

            if (character.IsLeftMost()) character.X = desiredX;
    
            else
            {
                character.X = character.GetPreviousSibling().X + nodeSize + siblingDistance;
                character.Mod = character.X - desiredX;
            }
        }

        if (character.Childrens.Count > 0 && character.IsLeftMost() == false)
            CheckForConflicts(character);
    }

    private void CheckForConflicts(Character character)
    {
        float minDistance = treeDistance + nodeSize;
        float shiftValue = 0;

        Dictionary<int, float> nodeContour = new Dictionary<int, float>();
        GetLeftContour(character, 0, ref nodeContour);

        Character sibling = character.GetLeftMostSibling();
        while (sibling != null && sibling != character)
        {
            Dictionary<int, float> siblingContour = new Dictionary<int, float>();
            GetRightContour(sibling, 0, ref siblingContour);

            for (int level = character.Y - 75; level >= Math.Max(siblingContour.Keys.Min(), nodeContour.Keys.Min()); level -= 75)
            {
                float distance = nodeContour[level] - siblingContour[level];
                if (distance + shiftValue < minDistance)
                    shiftValue = Mathf.Max(minDistance - distance, shiftValue);
            }

            if (shiftValue > 0)
                CenterNodesBetween(sibling, character, shiftValue);

            sibling = sibling.GetNextSibling();
        }

        if (shiftValue > 0)
        {
            character.X += (int)shiftValue;
            character.Mod += (int)shiftValue;
            shiftValue = 0;
        }
    }

    private void CenterNodesBetween(Character leftNode, Character rightNode, float shiftValue)
    {
        int leftIndex = leftNode.Parents[0].Childrens.IndexOf(leftNode);
        int rightIndex = leftNode.Parents[0].Childrens.IndexOf(rightNode);

        int numNodesBetween = rightIndex - leftIndex - 1;

        if (numNodesBetween > 0)
        {
            var distanceBetweenNodesbefore = Mathf.Abs(rightNode.X - leftNode.X) / (numNodesBetween + 1);
            var distanceBetweenNodesafter = Mathf.Abs(rightNode.X + shiftValue - leftNode.X) / (numNodesBetween + 1);
            int count = 1;
            
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                Character middleNode = leftNode.Parents[0].Childrens[i];
                int desiredXafter = leftNode.X + ((int)distanceBetweenNodesafter * count);
                int desiredX = leftNode.X + (distanceBetweenNodesbefore * count);
                int offset = desiredXafter - desiredX;
                middleNode.X += offset;
                middleNode.Mod += offset;
                count++;
            }

            CheckForConflicts(leftNode);
        }
    }


    private void GetLeftContour(Character character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        if (nodeContour.ContainsKey(character.Y) == false) nodeContour.Add(character.Y, character.X + modSum);
        else nodeContour[character.Y] = Math.Min(nodeContour[character.Y], character.X + modSum);

        modSum += character.Mod;

        foreach (Character child in character.Childrens)
            GetLeftContour(child, modSum, ref nodeContour);
    }

    private void GetRightContour(Character character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        if (nodeContour.ContainsKey(character.Y) == false) nodeContour.Add(character.Y, character.X + modSum);
        else nodeContour[character.Y] = Math.Max(nodeContour[character.Y], character.X + modSum);

        modSum += character.Mod;

        foreach (Character child in character.Childrens)
            GetRightContour(child, modSum, ref nodeContour);
    }

    private void CheckAllChildrenOnScreen(Character character)
    {
        Dictionary<int, float> nodeContour = new Dictionary<int, float>();
        GetLeftContour(character, 0, ref nodeContour);

        float shiftAmount = 0;
        foreach (int y in nodeContour.Keys)
        {
            if (nodeContour[y] + shiftAmount < 0)
                shiftAmount = nodeContour[y] * -1;
        }

        if (shiftAmount > 0)
        {
            character.X += (int)shiftAmount;
            character.Mod += character.Mod + (int)shiftAmount;
        }
    }

    private void CalculateFinalPositions(Character character, int modSum)
    {
        character.X += modSum;
        modSum += character.Mod;

        foreach (Character child in character.Childrens)
            CalculateFinalPositions(child, modSum);

        if (character.IsLeaf()) character.Y = character.Depth * -75;
        else character.Y = character.Childrens[0].Y + 75;
    }

    private void UpdateNodePositions(Character character)
    {
        character.SetTransformPositionX(character.X);
        character.SetTransformPositionY(character.Y);

        foreach (Character child in character.Childrens)
            UpdateNodePositions(child);
    }

    private void DrawLines(Character character)
    {
        if (character.Parents.Count != 0)
        {
            Vector2 nodeTopMiddle = new Vector2(character.CharacterButton.GetComponent<RectTransform>().anchoredPosition.x, character.CharacterButton.GetComponent<RectTransform>().anchoredPosition.y + 20 - 4);
            Vector2 nodeAboveMiddle = new Vector2(nodeTopMiddle.x, (character.CharacterButton.GetComponent<RectTransform>().anchoredPosition.y + character.Parents[0].CharacterButton.GetComponent<RectTransform>().anchoredPosition.y) / 2);
            
            GameObject temp1 = new GameObject();
            temp1.transform.SetParent(LinesGameObject.transform);
            GameObject line1 = temp1;
            line1.AddComponent<Image>();
            line1.transform.localScale = new Vector3(1, 1, 1);
            line1.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
            line1.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
            line1.AddComponent<LinesCreator>();
            
            _tempObject1.GetComponent<RectTransform>().anchoredPosition = nodeTopMiddle;
            _tempObject2.GetComponent<RectTransform>().anchoredPosition = nodeAboveMiddle;


            line1.GetComponent<LinesCreator>().SetPoints(_tempObject1.transform, _tempObject2.transform);
            line1.GetComponent<LinesCreator>().Settings();
        }

        if (character.Childrens.Count > 0)
        {
            Vector2 nodeBottomMiddle = new Vector2(character.CharacterButton.GetComponent<RectTransform>().anchoredPosition.x, character.CharacterButton.GetComponent<RectTransform>().anchoredPosition.y - 20 + 2);
            Vector2 nodeBelowMiddle = new Vector2(nodeBottomMiddle.x, (character.CharacterButton.GetComponent<RectTransform>().anchoredPosition.y + character.Childrens[0].CharacterButton.GetComponent<RectTransform>().anchoredPosition.y) / 2);

            GameObject temp2 = new GameObject();
            temp2.transform.SetParent(LinesGameObject.transform);
            GameObject line2 = temp2;
            line2.AddComponent<Image>();
            line2.transform.localScale = new Vector3(1, 1, 1);
            line2.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
            line2.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
            line2.AddComponent<LinesCreator>();
            
            _tempObject1.GetComponent<RectTransform>().anchoredPosition = nodeBottomMiddle;
            _tempObject2.GetComponent<RectTransform>().anchoredPosition = nodeBelowMiddle;


            line2.GetComponent<LinesCreator>().SetPoints(_tempObject1.transform, _tempObject2.transform);
            line2.GetComponent<LinesCreator>().Settings();
            
            if (character.Childrens.Count > 1)
            {
                Vector2 nodeLeftMiddle = new Vector2(character.Childrens[0].CharacterButton.GetComponent<RectTransform>().anchoredPosition.x - 2.435f, (character.Childrens[0].CharacterButton.GetComponent<RectTransform>().anchoredPosition.y + character.CharacterButton.GetComponent<RectTransform>().anchoredPosition.y) / 2);
                Vector2 nodeRightMiddle = new Vector2(character.Childrens[character.Childrens.Count - 1].CharacterButton.GetComponent<RectTransform>().anchoredPosition.x + 2.435f, (character.Childrens[character.Childrens.Count - 1].CharacterButton.GetComponent<RectTransform>().anchoredPosition.y + character.CharacterButton.GetComponent<RectTransform>().anchoredPosition.y) / 2);

                GameObject temp3 = new GameObject();
                temp3.transform.SetParent(LinesGameObject.transform);
                GameObject line3 = temp3;
                line3.AddComponent<Image>();
                line3.transform.localScale = new Vector3(1, 1, 1);
                line3.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
                line3.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
                line3.AddComponent<LinesCreator>();
                
                _tempObject1.GetComponent<RectTransform>().anchoredPosition = nodeLeftMiddle;
                _tempObject2.GetComponent<RectTransform>().anchoredPosition = nodeRightMiddle;
            
                line3.GetComponent<LinesCreator>().SetPoints(_tempObject1.transform, _tempObject2.transform);
                line3.GetComponent<LinesCreator>().Settings();
            }
        }

        foreach (Character child in character.Childrens)
        {
            DrawLines(child);
        }
    }
}

