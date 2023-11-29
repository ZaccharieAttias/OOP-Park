using UnityEngine;
using UnityEngine.UI;

public class Create : MonoBehaviour
{
    public GameObject objToCreate;

    private void Start()
    {
        // Vérifiez si le composant Button est attaché à cet objet
        Button button = GetComponent<Button>();

        // Si le composant Button est trouvé, ajoutez un écouteur de clic
        if (button != null)
        {
            button.onClick.AddListener(CreateObject);
        }
        else
        {
            Debug.LogError("Aucun composant Button trouvé sur cet objet.");
        }
    }

    public void CreateObject()
    {
        Instantiate(objToCreate, transform.position, transform.rotation);
    }
}
