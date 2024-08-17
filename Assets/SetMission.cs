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
    public int Challenge;
    public void Start()
    {
        gridTransform = gameObject.transform.Find("Background/Foreground/GridObject");
        ChallengeQuete = gameObject.transform.Find("Background/Foreground/Mssion/InputField").GetComponent<TMP_InputField>();
        CharacterChallengeManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>();
        Challenge = int.Parse(gameObject.name.Substring(7));
    }
    public void SetChallengeAppearancesConditions()
    {
        foreach (Transform item in gridTransform)
        {
            if (item.GetComponent<Toggle>().isOn)
                appearancesCondition.Add(item.name);
        }
        CharacterChallengeManager.InitializeUIOnlineElements(Challenge, appearancesCondition);
        ResetToggles();
    }
    //reset all toggles
    public void ResetToggles()
    {
        foreach (Transform item in gridTransform)
        {
            item.GetComponent<Toggle>().isOn = false;
        }
    }
}
