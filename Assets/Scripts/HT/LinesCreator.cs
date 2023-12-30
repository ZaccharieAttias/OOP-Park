using UnityEngine;

public class LinesCreator : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    private RectTransform lineRect;

    void Start()
    {
        lineRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (startPoint && endPoint)
        {
            Vector3 pointALocal = startPoint.GetComponent<RectTransform>().anchoredPosition;
            Vector3 pointBLocal = endPoint.GetComponent<RectTransform>().anchoredPosition;
            UpdateLine(pointALocal, pointBLocal);
        }
        else
        {
            lineRect.sizeDelta = new Vector2(0, 0);
        }
    }

void UpdateLine(Vector2 anchoredPointA, Vector2 anchoredPointB)
{
    Vector2 midpoint = (anchoredPointA + anchoredPointB) / 2;
    float distance = Vector2.Distance(anchoredPointA, anchoredPointB);
    float angle = Mathf.Atan2(anchoredPointB.y - anchoredPointA.y, anchoredPointB.x - anchoredPointA.x) * Mathf.Rad2Deg;

    lineRect.anchoredPosition = midpoint;
    lineRect.sizeDelta = new Vector2(distance, 5);
    lineRect.rotation = Quaternion.Euler(0, 0, angle);
}

    public void SetPoints(Transform start, Transform end)
    {
        startPoint = start;
        endPoint = end;
    }
}
