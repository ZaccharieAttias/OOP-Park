using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetMission : MonoBehaviour
{
    public Transform gridTransform;
    public TMP_InputField ChallengeQuete;
    public CharacterChallengeManager CharacterChallengeManager;
    public List <string> appearancesCondition;
    public int missionNumber;
    public Button confirmButton;
    public void Start()
    {
        gridTransform = gameObject.transform.Find("Background/Foreground/GridObject");
        ChallengeQuete = gameObject.transform.Find("Background/Foreground/Mssion/InputField").GetComponent<TMP_InputField>();
        CharacterChallengeManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>();
        confirmButton = gameObject.transform.Find("Background/Foreground/ConfirmMission").GetComponent<Button>();
        missionNumber = int.Parse(gameObject.name.Substring(7));
    }
    public void SetChallengeAppearancesConditions()
    {
        foreach (Transform item in gridTransform)
        {
            if (item.GetComponent<Toggle>().isOn)
                appearancesCondition.Add(item.name);
        }
        CharacterChallengeManager.InitializeUIOnlineElements(missionNumber, appearancesCondition);
        ResetToggles();
    }
    public void ResetToggles()
    {
        foreach (Transform item in gridTransform)
        {
            item.GetComponent<Toggle>().isOn = false;
        }
    }
    public void AllowToConfirm()
    {
        bool allow = false;
        if (ChallengeQuete.text != "")
        {
            foreach (Transform item in gridTransform)
            {
                if (item.GetComponent<Toggle>().isOn)
                {
                    allow = true;
                    confirmButton.interactable = true;
                    return;
                }
            }
        }
        confirmButton.interactable = allow;
    }
}
