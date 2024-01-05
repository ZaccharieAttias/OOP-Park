using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeFocus : MonoBehaviour
{
    public ScrollRect scrollView;
    public RectTransform targetItem;
    public float focusSpeed = 1.0f;

    public void Start()
    {
        scrollView = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Tree").GetComponent<ScrollRect>();
    }

    public void SetTargetItem(RectTransform item)
    {
        targetItem = item;
    }

    public void FocusOnTargetItem()
    {
         StartCoroutine(scrollView.FocusOnItemCoroutine(targetItem, focusSpeed));
    }

    IEnumerator StartFocusOnTargetItem()
    {
        yield return StartCoroutine(scrollView.FocusOnItemCoroutine(targetItem, focusSpeed));
    }
}
