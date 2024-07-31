using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AiModelData : MonoBehaviour
{
    public List<string> CharactersOrder = new();
    public int AbstractLevelTries = 0;
    public int AppearanceLevelTries = 0;
    public int DeathsCount = 0;
    public int TimeTook = 0;
    public int Score = 0;


    public void AddCharacterOrder()
    {
        if (CharactersOrder.Count == 0 || CharactersOrder.Last() != CharactersData.CharactersManager.CurrentCharacter.Name)
        {
            CharactersOrder.Add(CharactersData.CharactersManager.CurrentCharacter.Name);
        }
    }

    public void SaveJson()
    {
        // Create here the needed json
    }

    public int CalculateScore()
    {
        TimeTook = (int)Time.timeSinceLevelLoad;

        int timeVariable = TimeTook / 90;
        int deathsVariable = DeathsCount > 2 ? DeathsCount - 2 : 0;
        int abstractLevelTriesVariable = AbstractLevelTries > 5 ? AbstractLevelTries - 5 : 0;
        int appearanceLevelTriesVariable = AppearanceLevelTries > 5 ? AppearanceLevelTries - 5 : 0;

        // Add here a timeout factor
        Score = 100 - (deathsVariable * 5) - (abstractLevelTriesVariable * 5) - (appearanceLevelTriesVariable * 5) - (timeVariable * 5);

        Score = Score < 0 ? 0 : Score;
        // SaveJson()...
        return Score;
    }
}