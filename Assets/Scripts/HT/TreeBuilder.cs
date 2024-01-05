using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using UnityEngine.UI;

public class TreeBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _lines;

    GameObject _tempObject1;
    GameObject _tempObject2;



    private Character _rootCharacter;

    private static int nodeSize = 40;
    private static int siblingDistance = 35;
    private static int treeDistance = 5;

    public void SetRoot(Character rootCharacter)
    {
        _rootCharacter = rootCharacter;
        _lines = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Tree/Lines");
        _tempObject1 = new GameObject("temp1");
        _tempObject1.transform.SetParent(_lines.transform);
        _tempObject1.AddComponent<RectTransform>();
        _tempObject2 = new GameObject("temp2");
        _tempObject2.transform.SetParent(_lines.transform);
        _tempObject2.AddComponent<RectTransform>();
    }

    public void BuildTree()
    {
        ResetLines();
        CalculateNodePositions();
        DrawLines(_rootCharacter);
    }

    private void ResetLines()
    {
        foreach (Transform child in _lines.transform)
        {
            if (child.gameObject.name != "temp1" && child.gameObject.name != "temp2")
                Destroy(child.gameObject);
        }
    }

    private void CalculateNodePositions()
    {
        // initialize node x, y, and mod values
        InitializeNodes(_rootCharacter, 0);

        // assign initial X and Mod values for nodes
        CalculateInitialX(_rootCharacter);

        // ensure no node is being drawn off screen
        CheckAllChildrenOnScreen(_rootCharacter);

        // assign final X values to nodes
        CalculateFinalPositions(_rootCharacter, 0);

        // update the transform positions of the nodes
        UpdateNodePositions(_rootCharacter);
    }

    private void InitializeNodes(Character character, int depth)
    {
        character.SetX(0);
        character.SetY(depth * -75);
        character.SetDepth(depth);
        character.SetMod(0);

        foreach (Character child in character.childrens)
            InitializeNodes(child, depth + 1);
    }

    private void CalculateInitialX(Character character)
    {
        foreach (Character child in character.childrens)
            CalculateInitialX(child);
 
        // if there is a previous sibling in this set, set X to prevous sibling + designated distance
        if (character.IsLeaf()){
            if (!character.IsLeftMost())
                character.SetX(character.GetPreviousSibling().GetX() + nodeSize + siblingDistance);
            else
                // if this is the first character in a set, set X to 0
                character.SetX(0);
        }
        // if there is only one child
        else if (character.childrens.Count == 1){
                if (character.IsLeftMost())
                    character.SetX(character.childrens[0].GetX());
                else
                {
                    character.SetX(character.GetPreviousSibling().GetX() + nodeSize + siblingDistance);
                    character.SetMod(character.GetX() - character.childrens[0].GetX());
                }
        }
        // if there are more than one child
        else
        {
            // Find the first child's X and the last child's X
            Character leftChild = character.GetLeftMostChild();
            Character rightChild = character.GetRightMostChild();

            // Find the midway point between the first and last child's X
            var desiredX =  (leftChild.GetX() + rightChild.GetX()) / 2;

            // Check if this node has any siblings to the left of it
            if (character.IsLeftMost())
            {
                // Set the X value equal to the desired X value to center the parent over the children
                character.SetX(desiredX);
            }
            else
            {
                // Assign the Mod value of the node to node.X - desiredX to shift children under the parent
                character.SetX(character.GetPreviousSibling().GetX() + nodeSize + siblingDistance);
                character.SetMod(character.GetX() - desiredX);
            }
        }

        // Since subtrees can overlap, check for conflicts and shift tree right if needed
        if (character.childrens.Count > 0 && !character.IsLeftMost())
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

            for (int level = character.GetY() -75; level >= Math.Max(siblingContour.Keys.Min(), nodeContour.Keys.Min()); level -= 75)
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
            character.SetX(character.GetX() + (int)shiftValue);
            character.SetMod(character.GetMod() + (int)shiftValue);
            shiftValue = 0;
        }
    }

    private void CenterNodesBetween(Character leftNode, Character rightNode, float shiftValue)
    {
        int leftIndex = leftNode.parents[0].childrens.IndexOf(leftNode);
        int rightIndex = leftNode.parents[0].childrens.IndexOf(rightNode);

        int numNodesBetween = (rightIndex - leftIndex) - 1;

        if (numNodesBetween > 0)
        {
            Debug.Log("CenterNodesBetween > 0");
            var distanceBetweenNodesbefore = Mathf.Abs(rightNode.GetX() - leftNode.GetX()) / (numNodesBetween + 1);
            var distanceBetweenNodesafter = Mathf.Abs(rightNode.GetX() + shiftValue - leftNode.GetX()) / (numNodesBetween + 1);
            int count = 1;
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                Character middleNode = leftNode.parents[0].childrens[i];
                int desiredXafter = leftNode.GetX() + ((int)distanceBetweenNodesafter * count);
                int desiredX = leftNode.GetX() + (distanceBetweenNodesbefore * count);
                int offset = desiredXafter - desiredX;
                middleNode.SetX(middleNode.GetX() + offset);
                middleNode.SetMod(middleNode.GetMod() + offset);
                count++;
            }

            CheckForConflicts(leftNode);
        }
    }


    private void GetLeftContour(Character character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        if (!nodeContour.ContainsKey(character.GetY()))
            nodeContour.Add(character.GetY(), character.GetX() + modSum);
        else
            nodeContour[character.GetY()] = Math.Min(nodeContour[character.GetY()], character.GetX() + modSum);

        modSum += character.GetMod();

        foreach (Character child in character.childrens)
            GetLeftContour(child, modSum, ref nodeContour);
    }

    private void GetRightContour(Character character, int modSum, ref Dictionary<int, float> nodeContour)
    {
        if (!nodeContour.ContainsKey(character.GetY()))
            nodeContour.Add(character.GetY(), character.GetX() + modSum);
        else
            nodeContour[character.GetY()] = Math.Max(nodeContour[character.GetY()], character.GetX() + modSum);

        modSum += character.GetMod();

        foreach (Character child in character.childrens)
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
                shiftAmount = (nodeContour[y] * -1);
        }

        if (shiftAmount > 0)
        {
            character.SetX(character.GetX() + (int)shiftAmount);
            character.SetMod(character.GetMod() + (int)shiftAmount);

        }
    }

    private void CalculateFinalPositions(Character character, int modSum)
    {
        character.SetX(character.GetX() + modSum);
        modSum += character.GetMod();

        foreach (Character child in character.childrens)
            CalculateFinalPositions(child, modSum);

        if (character.IsLeaf())
            character.SetY(character.GetDepth() * -75);
        else
            character.SetY(character.childrens[0].GetY() + 75);
    }

    private void UpdateNodePositions(Character character)
    {
        character.SetTransformPositionX(character.GetX());
        character.SetTransformPositionY(character.GetY());

        foreach (Character child in character.childrens)
            UpdateNodePositions(child);
    }

    private void DrawLines(Character character)
    {
        if (character.parents.Count != 0)
        {
            Vector2 nodeTopMiddle = new Vector2(character.GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.x, character.GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y + 20 - 4);
            Vector2 nodeAboveMiddle = new Vector2(nodeTopMiddle.x, (character.GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y + character.parents[0].GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y) / 2);
            
            GameObject temp1 = new GameObject();
            temp1.transform.SetParent(_lines.transform);
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

        if (character.childrens.Count > 0)
        {
            Vector2 nodeBottomMiddle = new Vector2(character.GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.x, character.GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y - 20 + 2);
            Vector2 nodeBelowMiddle = new Vector2(nodeBottomMiddle.x, (character.GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y + character.childrens[0].GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y) / 2);

            GameObject temp2 = new GameObject();
            temp2.transform.SetParent(_lines.transform);
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
            
            if (character.childrens.Count > 1)
            {
                Vector2 nodeLeftMiddle = new Vector2(character.childrens[0].GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.x - 2.435f, (character.childrens[0].GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y + character.GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y) / 2);
                Vector2 nodeRightMiddle = new Vector2(character.childrens[character.childrens.Count - 1].GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.x + 2.435f, (character.childrens[character.childrens.Count - 1].GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y + character.GetCharacterButton().GetComponent<RectTransform>().anchoredPosition.y) / 2);

                GameObject temp3 = new GameObject();
                temp3.transform.SetParent(_lines.transform);
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

        foreach (Character child in character.childrens)
        {
            DrawLines(child);
        }
    }

    public GameObject GetRootCharacterButton()
    {
        return _rootCharacter.GetCharacterButton();
    }
}

