using System.Collections.Generic;

[System.Serializable]
public class TileData
{
    public int id;
    public PositionData position;
    public string character;
    public List<int> children;
}

[System.Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class LevelData
{
    public string title;
    public List<TileData> tiles;
}