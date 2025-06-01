using UnityEngine;

public class DataManager
{
    private string filePath => Application.persistentDataPath + "/progress.json";
    public void SaveProgress(ProgressData progressData)
    {
        string json = JsonUtility.ToJson(progressData, true);
        System.IO.File.WriteAllText(filePath, json);
    }
    public ProgressData LoadProgress()
    {
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            return JsonUtility.FromJson<ProgressData>(json);
        }
        return new ProgressData();
    }

    public void UpdateLevelData(int levelNumber, int score)
    {
        ProgressData progressData = LoadProgress();
        LevelData levelData = progressData.levels.Find(l => l.levelNumber == levelNumber);
        if (levelData == null)
        {
            levelData = new LevelData { levelNumber = levelNumber, score = score };
            progressData.levels.Add(levelData);
        }
        else
        {
            levelData.score = score;
        }
        SaveProgress(progressData);
    }
}