using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    private RectTransform lineRect;

    void Start()
    {
        // Assurez-vous d'avoir un RectTransform attaché à cet objet
        lineRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (startPoint && endPoint)
        {
            // get the achored position pf the pointA and pointB
            Vector3 pointALocal = startPoint.GetComponent<RectTransform>().anchoredPosition;
            Debug.Log("pointALocal" + pointALocal);
            Vector3 pointBLocal = endPoint.GetComponent<RectTransform>().anchoredPosition;
            Debug.Log("pointBLocal" + pointBLocal);
            UpdateLine(pointALocal, pointBLocal);
        }
        else
        {
            // Si l'un des points n'est pas défini, masquer la ligne en ajustant sa taille à (0, 0)
            lineRect.sizeDelta = new Vector2(0, 0);
        }
    }

void UpdateLine(Vector2 anchoredPointA, Vector2 anchoredPointB)
{
    // Calculer le point médian
    Vector2 midpoint = (anchoredPointA + anchoredPointB) / 2;

    // Calculer l'angle entre les deux points
    float angle = Mathf.Atan2(anchoredPointB.y - anchoredPointA.y, anchoredPointB.x - anchoredPointA.x) * Mathf.Rad2Deg;

    // Calculer la distance entre les deux points
    float distance = Vector2.Distance(anchoredPointA, anchoredPointB);

    // Ajuster la position, la taille et la rotation de la ligne
    lineRect.anchoredPosition = midpoint;
    lineRect.sizeDelta = new Vector2(distance, 5); // 5 est l'épaisseur de la ligne
    lineRect.rotation = Quaternion.Euler(0, 0, angle);
}

}
