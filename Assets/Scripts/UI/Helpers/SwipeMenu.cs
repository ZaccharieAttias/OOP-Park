using UnityEngine;
using UnityEngine.UI;


public class SwipeMenu : MonoBehaviour
{
    [Header("Scripts")]
    public EncapsulationManager EncapsulationManager;

    [Header("UI Elements")]
    public GameObject ScrollBar;
    public float CurrentScrollPosition;
    public float PreviousScrollPosition;
    public float[] ItemPosition;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
    }
    public void Update()
    {
        UpdatePositions();
        HandleScrolling();
        ScaleMenuItems();
    }
    public void InitializeScripts()
    {
        EncapsulationManager = GameObject.Find("Canvas/Popups").GetComponent<EncapsulationManager>();
    }
    public void InitializeUIElements()
    {
        ScrollBar = EncapsulationManager.Popup.transform.Find("Background/Foreground/Set/Back/ScrollView/ScrollbarVertical").gameObject;
        CurrentScrollPosition = 1;
        PreviousScrollPosition = 1;
        ItemPosition = new float[transform.childCount];
    }

    public void UpdatePositions()
    {
        int childCount = transform.childCount;
        ItemPosition = new float[childCount];
        float distance = 1f / (childCount - 1f);

        for (int i = 0; i < childCount; i++)
        {
            ItemPosition[i] = 1 - distance * i;
        }
    }
    public void HandleScrolling()
    {
        if (Input.GetMouseButton(0)) CurrentScrollPosition = ScrollBar.GetComponent<Scrollbar>().value;
        else AdjustScrollPosition();
    }
    public void AdjustScrollPosition()
    {
        float distance = 1f / (ItemPosition.Length - 1f);

        for (int i = 0; i < ItemPosition.Length; i++)
        {
            float invertedScrollPos = 1 - CurrentScrollPosition;
            if (invertedScrollPos < ItemPosition[i] + (distance / 2) && invertedScrollPos > ItemPosition[i] - (distance / 2)) ScrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(ScrollBar.GetComponent<Scrollbar>().value, 1 - ItemPosition[i], 0.1f);
        }
    }
    public void ScaleMenuItems()
    {
        int childCount = transform.childCount;
        float distance = 1f / (ItemPosition.Length - 1f);
        int activeItemIndex = 0;

        for (int i = 0; i < ItemPosition.Length; i++)
        {
            float invertedScrollPos = 1 - CurrentScrollPosition;

            if (invertedScrollPos < ItemPosition[i] + (distance / 2) && invertedScrollPos > ItemPosition[i] - (distance / 2))
            {
                Transform childTransform = transform.GetChild(childCount - i - 1);
                childTransform.localScale = Vector2.Lerp(childTransform.localScale, new Vector2(1f, 1f), 0.1f);
                activeItemIndex = i;

                for (int j = 0; j < ItemPosition.Length; j++)
                {
                    if (j != i)
                    {
                        Transform otherChildTransform = transform.GetChild(childCount - j - 1);
                        otherChildTransform.localScale = Vector2.Lerp(otherChildTransform.localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }

        UpdateEncapsulationManager(activeItemIndex);
    }

    public void UpdateEncapsulationManager(int activeItemIndex)
    {
        if (PreviousScrollPosition != ScrollBar.GetComponent<Scrollbar>().value)
        {
            PreviousScrollPosition = ScrollBar.GetComponent<Scrollbar>().value;
            int childCount = transform.childCount;
            GameObject activeItem = transform.GetChild(childCount - activeItemIndex - 1).gameObject;

            EncapsulationManager.CurrentSet = activeItem;
            EncapsulationManager.UpdateGetContent(activeItem.name.Split(' ')[0]);
        }
    }
}