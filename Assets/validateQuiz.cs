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
    public List<List<string>> errorAnswer = new() { 
            new() { "Inheritance is about sharing and reusing code, not directly executing methods.\n","", "Inheritance does not affect the deletion of classes; it is about establishing relationships between classes.\n", "Inheritance does not prevent object creation; it helps in creating objects with shared characteristics.\n\n" }, 
            new List<string> { "The public access modifier allows access from any other class.\n", "The protected access modifier allows access within its own class and by derived classes.\n", "", "It not a reason for the importance of inheritance.\n\n" }, 
            new List<string> { "Upcasting refers to treating a subclass object as an instance of its superclass, not the other way around.\n", "", "Upcasting is specifically about the relationship between a superclass and its subclass, not any arbitrary type conversion.\n", "Upcasting is about type conversion within an inheritance hierarchy, not about changing method access levels.\n" } };

    public quizFeedback quizFeedback;
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
            if (scores[1][1] == 1) ErrorText.text += errorAnswer[1][1];
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
            quizFeedback.ToggleOn();
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
