using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class QuizManager : MonoBehaviour
{
    [Header("Scripts")]
    public AiModelData AiModelData;
    public FeedbackManager FeedbackManager;

    [Header("UI Elements")]
    public GameObject Popup;
    public TMP_Text ErrorText;

    [Header("Buttons")]
    public List<ToggleGroup> ToggleGroup;
    public Button ValidateButton;

    [Header("Quiz Data")]
    public List<int> Answers;
    public List<List<int>> Scores;
    public List<List<string>> ErrorAnswers;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
        InitializeButtons();
        InitializeEventListeners();
        InitializeQuizData();
    }
    public void InitializeScripts()
    {
        AiModelData = GameObject.Find("Scripts/AiModelData").GetComponent<AiModelData>();
        FeedbackManager = GameObject.Find("Canvas/Popups").GetComponent<FeedbackManager>();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/ErrorFeedback");
        ErrorText = Popup.transform.Find("Background/Foreground/Description/Text").GetComponent<TMP_Text>();
    }
    public void InitializeButtons()
    {
        ToggleGroup = new()
        {
            GameObject.Find("Canvas/Menus/CharacterCenter/Quiz/Buttons/Background/ScrollView/ViewPort/Content/AmericanQuestion1/Toggle").GetComponent<ToggleGroup>(),
            GameObject.Find("Canvas/Menus/CharacterCenter/Quiz/Buttons/Background/ScrollView/ViewPort/Content/AmericanQuestion2/Toggle").GetComponent<ToggleGroup>(),
            GameObject.Find("Canvas/Menus/CharacterCenter/Quiz/Buttons/Background/ScrollView/ViewPort/Content/AmericanQuestion3/Toggle").GetComponent<ToggleGroup>()
        };

        ValidateButton = GameObject.Find("Canvas/Menus/CharacterCenter/Quiz/Buttons/Background/ValidateButton").GetComponent<Button>();
        ValidateButton.onClick.AddListener(() => ValidateFactory());
    }
    public void InitializeEventListeners()
    {
        var closeButton = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        closeButton.onClick.AddListener(() => Popup.SetActive(false));

        for (int i = 0; i < ToggleGroup.Count; i++)
        {
            var toggleGroup = ToggleGroup[i];
            var toggles = toggleGroup.GetComponentsInChildren<Toggle>();

            for (int j = 0; j < toggles.Length; j++)
            {
                var toggle = toggles[j];
                int questionIndex = i;
                int subQuestionIndex = j;
                toggle.onValueChanged.AddListener((value) => SetValueQuestion(questionIndex, subQuestionIndex, value ? 1 : 0));
            }
        }
    }
    public void InitializeQuizData()
    {
        var chapterNumber = SceneManager.GetActiveScene().name[1];
        if (chapterNumber == '1')
        {
            Answers = new() { 2, 2, 1 };
            Scores = new() { new() { 0, 0, 0, 0 }, new List<int> { 0, 0, 0, 0 }, new List<int> { 0, 0, 0, 0 } };
            ErrorAnswers = new()
            {
                new() { "Inheritance is about sharing and reusing code, not directly executing methods.\n","", "Inheritance does not affect the deletion of classes; it is about establishing relationships between classes.\n", "Inheritance does not prevent object creation; it helps in creating objects with shared characteristics.\n\n" },
                new List<string> { "The public access modifier allows access from any other class.\n", "The protected access modifier allows access within its own class and by derived classes.\n", "", "It not a reason for the importance of inheritance.\n\n" },
                new List<string> { "Upcasting refers to treating a subclass object as an instance of its superclass, not the other way around.\n", "", "Upcasting is specifically about the relationship between a superclass and its subclass, not any arbitrary type conversion.\n", "Upcasting is about type conversion within an inheritance hierarchy, not about changing method access levels.\n" }
            };
        }

        else
        {
            Answers = new() { 2, 1, 1 };
            Scores = new() { new() { 0, 0, 0, 0 }, new List<int> { 0, 0, 0, 0 }, new List<int> { 0, 0, 0, 0 } };
            ErrorAnswers = new()
            {
                new() { "This describes encapsulation, not method overriding.\n", "This describes static methods, not method overriding.\n", "", "This describes making a class final (or sealed), not method overriding.\n\n" },
                new List<string> { "This is a standard way to implement encapsulation, not a violation of it.\n", "", "This is a controlled way to provide limited access, not a violation of encapsulation.\n", "This is normal use of a class and does not violate encapsulation.\n\n" },
                new List<string> { "This describes making a class final (or sealed), not using an abstract class.\n", "", "Abstract classes cannot be instantiated; this describes concrete classes.\n", "Multiple inheritance is typically achieved through interfaces, not abstract classes.\n" }
            };
        }
    }

    public void ValidateFactory()
    {
        AiModelData.QuizLevelTries++;

        ErrorText.text = "";
        int rightScores = 0;
        int totalQuestions = 3;

        if (Scores[0][Answers[0]] == 1) rightScores++;
        else
        {
            ErrorText.text += "Error in question 1:\n    ";
            if (Scores[0][0] == 1) ErrorText.text += ErrorAnswers[0][0];
            if (Scores[0][1] == 1) ErrorText.text += ErrorAnswers[0][1];
            if (Scores[0][2] == 1) ErrorText.text += ErrorAnswers[0][2];
            if (Scores[0][3] == 1) ErrorText.text += ErrorAnswers[0][3];
        }

        if (Scores[1][Answers[1]] == 1) rightScores++;
        else
        {
            ErrorText.text += "Error in question 2:\n    ";
            if (Scores[1][0] == 1) ErrorText.text += ErrorAnswers[1][0];
            if (Scores[1][1] == 1) ErrorText.text += ErrorAnswers[1][1];
            if (Scores[1][1] == 1) ErrorText.text += ErrorAnswers[1][1];
            if (Scores[1][3] == 1) ErrorText.text += ErrorAnswers[1][3];
        }

        if (Scores[2][Answers[2]] == 1) rightScores++;
        else
        {
            ErrorText.text += "Error in question 3:\n    ";
            if (Scores[2][0] == 1) ErrorText.text += ErrorAnswers[2][0];
            if (Scores[2][1] == 1) ErrorText.text += ErrorAnswers[2][1];
            if (Scores[2][2] == 1) ErrorText.text += ErrorAnswers[2][2];
            if (Scores[2][3] == 1) ErrorText.text += ErrorAnswers[2][3];
        }

        if (rightScores == totalQuestions) FeedbackManager.ToggleOn();
        else Popup.SetActive(true);
    }

    public void SetValueQuestion(int questionIndex, int subQuestionIndex, int value)
    {
        Scores[questionIndex] = new() { 0, 0, 0, 0 };
        Scores[questionIndex][subQuestionIndex] = value;
    }
}