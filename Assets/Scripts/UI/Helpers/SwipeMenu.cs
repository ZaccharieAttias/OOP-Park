using System;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject ScrollBar;
    public float Scroll_pos;
    public float[] pos;
    public EncapsulationManager EncapsulationManager;
    public float Scrollposition;

    public void Start()
    {
        EncapsulationManager = GameObject.Find("Canvas/Popups").GetComponent<EncapsulationManager>();
        ScrollBar = EncapsulationManager.Popup.transform.Find("Background/Foreground/Set/Back/ScrollView/ScrollbarVertical").gameObject;
        Scroll_pos = 1;
        Scrollposition = 1;
    }
    public void Update()
    {
        pos = new float[transform.childCount];

        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++){
            pos[i] = 1- distance * i;
        }

        if (Input.GetMouseButton (0))
            Scroll_pos = ScrollBar.GetComponent<Scrollbar>().value;
        else{
            for (int i = 0; i < pos.Length; i++){
                float invertedScrollPos = 1 - Scroll_pos;
                if (invertedScrollPos < pos[i] + (distance / 2) && invertedScrollPos > pos[i] - (distance / 2))
                {
                    ScrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(ScrollBar.GetComponent<Scrollbar>().value, 1-pos[i], 0.1f);
                }
            }
        }

        int z=0;
        int childCount = transform.childCount;
        for (int i = 0; i < pos.Length; i++){
            float invertedScrollPos = 1 - Scroll_pos;
            if (invertedScrollPos < pos[i] + (distance / 2) && invertedScrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(childCount - i - 1).localScale = Vector2.Lerp(transform.GetChild(childCount - i - 1).localScale, new Vector2(1f, 1f), 0.1f);
                z=i;
                for (int j =0; j<pos.Length;j++){
                    if (j!= i)
                        transform.GetChild(childCount - j - 1).localScale = Vector2.Lerp (transform.GetChild(childCount - j - 1).localScale, new Vector2(0.8f,0.8f), 0.1f);
                }
            }
        }

        if (Scrollposition != ScrollBar.GetComponent<Scrollbar>().value)
        {
            Scrollposition = ScrollBar.GetComponent<Scrollbar>().value;
            EncapsulationManager.CurrentSet = transform.GetChild(childCount - z - 1).gameObject;
            EncapsulationManager.UpdateGetContent(transform.GetChild(childCount - z - 1).name.Split(' ')[0]);
        }
    }
}
