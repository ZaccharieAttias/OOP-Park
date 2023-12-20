using UnityEngine;


public class SearchGameObject : MonoBehaviour
{
    public Transform FindGameObject(Transform parent, string childName)
    {
        Transform result = parent.Find(childName);

        if (result != null)
        {
            return result;
        }

        foreach (Transform child in parent)
        {
            result = FindGameObject(child, childName);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
