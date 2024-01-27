using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonHandler : MonoBehaviour {
    [SerializeField] BuildingObjectBase item;
    Button button;

    BuildingCreator buildingCreator;

    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        buildingCreator = BuildingCreator.GetInstance();
    }

    public BuildingObjectBase Item {
        set {
            item = value;
        }
    }

    private void ButtonClicked() {
        buildingCreator.ObjectSelected(item);
    }

}