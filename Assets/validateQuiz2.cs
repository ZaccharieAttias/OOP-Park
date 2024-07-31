using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ValidateQuiz2 : MonoBehaviour
{
    public Button ValidateButton;
    public ToggleGroup toggleGroup1;
    public ToggleGroup toggleGroup2;
    public ToggleGroup toggleGroup3;

    public List<int> answer = new() { 2, 1, 1 };
    public List<List<int>> scores = new() { new() { 0, 0, 0, 0 }, new List<int> { 0, 0, 0, 0 }, new List<int> { 0, 0, 0, 0 } };
    public List<List<string>> errorAnswer = new() { 
            new() { "This describes encapsulation, not method overriding.\n", "This describes static methods, not method overriding.\n", "", "This describes making a class final (or sealed), not method overriding.\n\n" }, 
            new List<string> { "This is a standard way to implement encapsulation, not a violation of it.\n", "", "This is a controlled way to provide limited access, not a violation of encapsulation.\n", "This is normal use of a class and does not violate encapsulation.\n\n" }, 
            new List<string> { "This describes making a class final (or sealed), not using an abstract class.\n", "", "Abstract classes cannot be instantiated; this describes concrete classes.\n", "Multiple inheritance is typically achieved through interfaces, not abstract classes.\n" } };

    public QuizFeedback QuizFeedback;
    public GameObject ErrorPanel;
    public TMP_Text ErrorText;
    
    public void Validate()
    {
        ErrorText.text = "";
        int score = 0;
        int total = 3;

        if (scores[0][answer[0]] == 1) score++;
        else {
            ErrorText.text += "Error in question 1:\n    ";
            if (scores[0][0] == 1) ErrorText.text += errorAnswer[0][0];
            if (scores[0][1] == 1) ErrorText.text += errorAnswer[0][1];
            if (scores[0][3] == 1) ErrorText.text += errorAnswer[0][3];
        }
        if (scores[1][answer[1]] == 1) score++;
        else {
            ErrorText.text += "Error in question 2:\n    ";
            if (scores[1][0] == 1) ErrorText.text += errorAnswer[1][0];
            if (scores[1][2] == 1) ErrorText.text += errorAnswer[1][2];
            if (scores[1][3] == 1) ErrorText.text += errorAnswer[1][3];
        }
        if (scores[2][answer[2]] == 1) score++;
        else {
            ErrorText.text += "Error in question 3:\n    ";
            if (scores[2][0] == 1) ErrorText.text += errorAnswer[2][0];
            if (scores[2][2] == 1) ErrorText.text += errorAnswer[2][2];
            if (scores[2][3] == 1) ErrorText.text += errorAnswer[2][3];
        }

        if (score == total)
            QuizFeedback.ToggleOn();
        else
        {
            ErrorPanel.SetActive(true);
        }

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
