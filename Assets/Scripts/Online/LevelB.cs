using System.Collections.Generic;

public class LevelB
{
    public List<string> TileTable;
    public int[,,] GroundMap;
    public int[,,] CoverMap;
    public int[,,] PropsMap;
    public int[,,] WallMap;
    public int[,,] GameplayMap;
    public float characterX;
    public float characterY;
    public Dictionary<int, List<string>> ChallengeAppearancesConditions;
    public Dictionary<int, List<string>> ChallengeAppearancesTexts;

    public LevelB(int width, int height, int depth, float characterX, float characterY, Dictionary<int, List<string>> ChallengeAppearancesConditions, Dictionary<int, List<string>> ChallengeAppearancesTexts)
    {
        TileTable = new List<string>();
        GroundMap = new int[width, height, depth];
        CoverMap = new int[width, height, depth];
        PropsMap = new int[width, height, depth];
        GameplayMap = new int[width, height, depth];
        WallMap = new int[width, height, depth];
        this.characterX = characterX;
        this.characterY = characterY;
        this.ChallengeAppearancesConditions = ChallengeAppearancesConditions;
        this.ChallengeAppearancesTexts = ChallengeAppearancesTexts;
    }

    public int AddTexture(string textureName)
    {
        var index = TileTable.IndexOf(textureName);

        if (index != -1) return index;

        TileTable.Add(textureName);

        return TileTable.Count - 1;
    }
}