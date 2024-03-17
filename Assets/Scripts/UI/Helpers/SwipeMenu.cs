using System;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject ScrollBar;
    public float Scroll_pos = 0;
    public float[] pos;
    public EncapsulationManager EncapsulationManager;
    public float Scrollposition = 0;

    public void Start()
    {
        EncapsulationManager = GameObject.Find("Canvas/Popups").GetComponent<EncapsulationManager>();
        ScrollBar = EncapsulationManager.Popup.transform.Find("Background/Foreground/Set/ScrollView/ScrollbarVertical").gameObject;
    }
    public void Update()
    {
        pos = new float[transform.childCount];

        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++){
            pos[i] = distance * i;
        }
        Array.Reverse(pos);

        if (Input.GetMouseButton (0))
            Scroll_pos = ScrollBar.GetComponent<Scrollbar>().value;
        else{
            for (int i =0; i < pos.Length; i++){
                if (Scroll_pos < pos[i] + (distance / 2) && Scroll_pos > pos[i] - (distance / 2)){
                    ScrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(ScrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        int z=0;
        for (int i = 0; i < pos.Length; i++){
            if (Scroll_pos < pos[i] + (distance / 2) && Scroll_pos > pos[i] - (distance / 2)){
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                z=i;
                for (int j =0; j<pos.Length;j++){
                    if (j!= i)
                        transform.GetChild(j).localScale = Vector2.Lerp (transform.GetChild(j).localScale, new Vector2(0.8f,0.8f), 0.1f);
                }
            }
        }

        if (Scrollposition != ScrollBar.GetComponent<Scrollbar>().value)
        {
            Scrollposition = ScrollBar.GetComponent<Scrollbar>().value;
            EncapsulationManager.CurrentSet = transform.GetChild(z).gameObject;
            EncapsulationManager.UpdateGetContent(transform.GetChild(z).name.Split(' ')[0]);
        }
    }
}
