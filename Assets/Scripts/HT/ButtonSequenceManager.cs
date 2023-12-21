using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonSequenceManager : MonoBehaviour
{
    public GameObject initialButton; // Reference to the initial button

    private int clickCount = 0; // Keep track of the number of clicks
    private List<GameObject> clickedObjects = new List<GameObject>(); // Keep track of the clicked objects

    void Start()
    {
        // Add onClick listeners to the buttons
        initialButton.GetComponent<Button>().onClick.AddListener(OnInitialButtonClick);
    }

    void OnInitialButtonClick()
    {
        // Logic for the initial button click
        clickCount++;
        clickedObjects.Add(EventSystem.current.currentSelectedGameObject);

        Debug.Log("name of clicked object: " + EventSystem.current.currentSelectedGameObject.name);

        // You can optionally change button interactivity here if needed
    }


    // void OnFirstTargetButtonClick()
    // {
    //     // Logic for the first target button click
    //     clickCount++;
    //     firstClickedObject = EventSystem.current.currentSelectedGameObject;

    //     Debug.Log("First target button clicked.");

    //     // You can optionally change button interactivity here if needed
    // }

    // void OnSecondTargetButtonClick()
    // {
    //     // Logic for the second target button click
    //     clickCount++;
    //     secondClickedObject = EventSystem.current.currentSelectedGameObject;

    //     Debug.Log("Second target button clicked.");

    //     // Check if the sequence is complete
    //     if (clickCount == 3)
    //     {
    //         // Call a function or method with the specified GameObjects
    //         YourFunction(firstClickedObject, secondClickedObject);

    //         // Reset the click count for the next sequence
    //         clickCount = 0;
    //     }
    // }

    // void YourFunction(GameObject firstObject, GameObject secondObject)
    // {
    //     // Implement your desired functionality here
    //     Debug.Log("YourFunction called with objects: " + firstObject.name + " and " + secondObject.name);
    // }
}













