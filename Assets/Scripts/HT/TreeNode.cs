using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class TreeNode : MonoBehaviour
{
    public TreeNode parent;
    public List<TreeNode> children = new List<TreeNode>();
    public int depth;
}
