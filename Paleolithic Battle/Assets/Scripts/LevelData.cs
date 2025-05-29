using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public int score;
}

[System.Serializable]
public class ProgressData
{
    public List<LevelData> levels = new List<LevelData>();
}