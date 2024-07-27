using System.Collections.Generic;

public class LevelB
{
    public List<string> TileTable;
    public int[,,] GroundMap;
    public int[,,] CoverMap;
    public int[,,] PropsMap;
    public int[,,] WallMap;
    public float characterX;
    public float characterY;

    public LevelB(int width, int height, int depth, float characterX, float characterY)
    {
        TileTable = new List<string>();
        GroundMap = new int[width, height, depth];
        CoverMap = new int[width, height, depth];
        PropsMap = new int[width, height, depth];
        WallMap = new int[width, height, depth];
        this.characterX = characterX;
        this.characterY = characterY;
    }

    public int AddTexture(string textureName)
    {
        var index = TileTable.IndexOf(textureName);

        if (index != -1) return index;

        TileTable.Add(textureName);
            
        return TileTable.Count - 1;
    }
}