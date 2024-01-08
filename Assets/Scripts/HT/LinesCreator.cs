using UnityEngine;
using UnityEngine.UI;


public class LinesCreator : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;

    public RectTransform lineRect;


    public void Settings()
    {
        if (StartPoint && EndPoint)
        {
            lineRect = GetComponent<RectTransform>();
            
            Vector3 pointALocal = StartPoint.GetComponent<RectTransform>().anchoredPosition;
            Vector3 pointBLocal = EndPoint.GetComponent<RectTransform>().anchoredPosition;
            
            UpdateLine(pointALocal, pointBLocal);
        }

        else
            lineRect.sizeDelta = new Vector2(0, 0);
    }
    private void UpdateLine(Vector2 startPoint, Vector2 endPoint)
    {
        float distance = Vector2.Distance(startPoint, endPoint);
        float angle = Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x) * Mathf.Rad2Deg;
        
        Vector2 middlePoint = (startPoint + endPoint) / 2;

        lineRect.anchoredPosition = middlePoint;
        lineRect.sizeDelta = new Vector2(distance, 5);
        lineRect.rotation = Quaternion.Euler(0, 0, angle);
        
        GetComponent<Image>().color = Color.red;
    }
    public void SetPoints(Transform startPoint, Transform endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }
}
