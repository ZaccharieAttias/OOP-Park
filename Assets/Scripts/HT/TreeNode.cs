using UnityEngine;
using System.Collections.Generic;


public class TreeNode : MonoBehaviour
{
    public Character character;
    public List<TreeNode> parent;
    public List<TreeNode> children;
    public int depth;


    public TreeNode()
    {
        parent = new List<TreeNode>();
        children = new List<TreeNode>();
    }
    
    public TreeNode(Character character, List<TreeNode> parent, List<TreeNode> children, int depth)
    {
        this.character = character;
        this.parent = parent;
        this.children = children;
        this.depth = depth;
    }
}
