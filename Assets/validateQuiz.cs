using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class validateQuiz : MonoBehaviour
{
    public Button ValidateButton;
    public ToggleGroup toggleGroup1;
    public ToggleGroup toggleGroup2;
    public ToggleGroup toggleGroup3;

    public List<int> answer = new() { 2, 2, 1 };
    public List<List<int>> scores = new() { new() { 0, 0, 0, 0 }, new List<int> { 0, 0, 0, 0 }, new List<int> { 0, 0, 0, 0 } };

    public void Start()
    {
        ValidateButton.onClick.AddListener(Validate);
    }

    public void Validate()
    {
        int score = 0;
        int total = 3;

        if (scores[0][answer[0]] == 1) score++;
        if (scores[1][answer[1]] == 1) score++;
        if (scores[2][answer[2]] == 1) score++;

        Debug.Log("Score: " + score + "/" + total);

    }

    public void SetValueQuestion1(int value)
    {
        scores[0] = new() { 0, 0, 0, 0 };
        scores[0][value - 1] = 1;
    }
    public void SetValueQuestion2(int value)
    {
        scores[1] = new() { 0, 0, 0, 0 };
        scores[1][value - 1] = 1;
    }
    public void SetValueQuestion3(int value)
    {
        scores[2] = new() { 0, 0, 0, 0 };
        scores[2][value - 1] = 1;
    }
}
