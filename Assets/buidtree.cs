using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class buidtree : MonoBehaviour
{
    public CharacterZac _rootCharacter;

    private static int nodeSize = 40;
    private static int siblingDistance = 0;
    private static int treeDistance = 0;

    public void CalculateNodePositions()
    {
        // initialize node x, y, and mod values
        InitializeNodes(_rootCharacter, 0);

        // assign initial X and Mod values for nodes
        CalculateInitialX(_rootCharacter);

        // ensure no node is being drawn off screen
        CheckAllChildrenOnScreen(_rootCharacter);

        // assign final X values to nodes
        CalculateFinalPositions(_rootCharacter, 0);
    }

    private void InitializeNodes(CharacterZac character, int depth)
    {
        Debug.Log("InitializeNodes");

        character.x = -75;
        character.y = depth * -75;
        character.SetDepth(depth);
        character.SetMod(0);

        foreach (CharacterZac child in character.childrens)
            InitializeNodes(child, depth + 1);
    }

    private void CalculateInitialX(CharacterZac character)
    {
        Debug.Log("CalculateInitialX");
        foreach (CharacterZac child in character.childrens)
            CalculateInitialX(child);
 
        // if there is a previous sibling in this set, set X to prevous sibling + designated distance
        if (character.IsLeaf()){
            if (!character.IsLeftMost())
                character.x = character.GetPreviousSibling().x + nodeSize + siblingDistance;
            else
                // if this is the first character in a set, set X to 0
                character.x = 0;
        }
        // if there is only one child
        else if (character.childrens.Count == 1){
                if (character.IsLeftMost())
                    character.x = character.childrens[0].x;
                else
                {
                    character.x = character.GetPreviousSibling().x + nodeSize + siblingDistance;
                    character.SetMod(character.x - character.childrens[0].x);
                }
        }
        // if there are more than one child
        else
        {
            // Find the first child's X and the last child's X
            CharacterZac leftChild = character.GetLeftMostChild();
            CharacterZac rightChild = character.GetRightMostChild();

            // Find the midway point between the first and last child's X
            var desiredX =  (leftChild.x + rightChild.x) / 2;

            // Check if this node has any siblings to the left of it
            if (character.IsLeftMost())
            {
                // Set the X value equal to the desired X value to center the parent over the children
                character.x = desiredX;
            }
            else
            {
                // Assign the Mod value of the node to node.X - desiredX to shift children under the parent
                character.x = character.GetPreviousSibling().x + nodeSize + siblingDistance;
                character.SetMod(character.x - desiredX);
            }
        }

        // Since subtrees can overlap, check for conflicts and shift tree right if needed
        if (character.childrens.Count > 0 && !character.IsLeftMost())
            CheckForConflicts(character);
    }

    private void CheckForConflicts(CharacterZac character)
    {
        Debug.Log("CheckForConflicts");
        float minDistance = treeDistance + nodeSize;
        float shiftValue = 0;

        Dictionary<int, float> nodeContour = new Dictionary<int, float>();
        GetLeftContour(character, 0, nodeContour);

        CharacterZac sibling = character.GetLeftMostSibling();
        while (sibling != null && sibling != character)
        {
            Dictionary<int, float> siblingContour = new Dictionary<int, float>();
            GetRightContour(sibling, 0, siblingContour);

            for (int level = character.y + 1; level <= Math.Min(siblingContour.Keys.Max(), nodeContour.Keys.Max()); level++)
            {
                float distance = nodeContour[level] - siblingContour[level];
                if (distance + shiftValue < minDistance)
                    shiftValue = Mathf.Max(minDistance - distance, shiftValue);
            }

            if (shiftValue > 0)
                CenterNodesBetween(character, sibling);

            sibling = sibling.GetNextSibling();
        }

        if (shiftValue > 0)
        {
            character.x += (int)shiftValue;
            character.Mod += (int)shiftValue;
            shiftValue = 0;
        }
    }

    private void CenterNodesBetween(CharacterZac leftNode, CharacterZac rightNode)
    {
        Debug.Log("CenterNodesBetween");
        int leftIndex = leftNode.parents[0].childrens.IndexOf(leftNode);
        int rightIndex = leftNode.parents[0].childrens.IndexOf(rightNode);

        int numNodesBetween = (rightIndex - leftIndex) - 1;

        if (numNodesBetween > 0)
        {
            //  INVERSSEZ
            var distanceBetweenNodes = (leftNode.x - rightNode.x) / (numNodesBetween + 1);

            int count = 1;
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                CharacterZac middleNode = leftNode.parents[0].childrens[i];
                int desiredX = rightNode.x + (distanceBetweenNodes * count);
                int offset = desiredX - middleNode.x;
                middleNode.x += offset;
                middleNode.Mod += offset;
                count++;
            }

            CheckForConflicts(leftNode);
        }
    }






    private void GetLeftContour(CharacterZac character, int modSum, Dictionary<int, float> nodeContour)
    {
        Debug.Log("GetLeftContour");
        if (!nodeContour.ContainsKey(character.y))
            nodeContour.Add(character.y, character.x + modSum);
        else
            nodeContour[character.y] = Math.Min(nodeContour[character.y], character.x + modSum);

        modSum += character.GetMod();

        foreach (CharacterZac child in character.childrens)
            GetLeftContour(child, modSum, nodeContour);
    }

    private void GetRightContour(CharacterZac character, int modSum, Dictionary<int, float> nodeContour)
    {
        Debug.Log("GetRightContour");
        if (!nodeContour.ContainsKey(character.y))
            nodeContour.Add(character.y, character.x + modSum);
        else
            nodeContour[character.y] = Math.Max(nodeContour[character.y], character.x + modSum);

        modSum += character.GetMod();

        foreach (CharacterZac child in character.childrens)
            GetRightContour(child, modSum, nodeContour);
    }

    private void CheckAllChildrenOnScreen(CharacterZac character)
    {
        Debug.Log("CheckAllChildrenOnScreen");

        Dictionary<int, float> nodeContour = new Dictionary<int, float>();
        GetLeftContour(character, 0, nodeContour);

        float shiftAmount = 0;
        foreach (int y in nodeContour.Keys)
        {
            if (nodeContour[y] + shiftAmount < 0)
                shiftAmount = (nodeContour[y] * -1);
        }

        if (shiftAmount > 0)
        {
            character.x += (int)shiftAmount;
            character.Mod += (int)shiftAmount;
        }
    }

    private void CalculateFinalPositions(CharacterZac character, int modSum)
    {
        Debug.Log("CalculateFinalPositions");
        character.x += modSum;
        modSum += character.Mod;

        foreach (CharacterZac child in character.childrens)
            CalculateFinalPositions(child, modSum);

        if (character.IsLeaf())
            character.y = character.GetDepth() * -75;
        else
            character.y = character.childrens[0].y + 75;
    }
}

