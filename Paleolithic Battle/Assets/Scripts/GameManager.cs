using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private DataManager dataManager;
    private DataOptionsManager dataOptionsManager;
    ProgressData progressData;

    OptionsData optionsData;

    int lastLevel = -1;
    int lastScore = -1;

    public bool[] unlockedLevels = new bool[3];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dataManager = new DataManager();
        progressData = dataManager.LoadProgress();
        InitializeUnlockedLevels();

        dataOptionsManager = new DataOptionsManager();
        optionsData = dataOptionsManager.LoadOptions();
    }

    private void Start()
    {
        SetOptions();
    }

    private void InitializeUnlockedLevels()
    {
        for (int i = 0; i < unlockedLevels.Length; i++)
        {
            LevelData levelData = progressData.levels.Find(l => l.levelNumber == i);
            if (levelData != null)
            {
                unlockedLevels[i] = true;
                Debug.Log($"Level {i} is unlocked with score: {levelData.score}");
            }
            else if (unlockedLevels[i])
            {
                LevelData newLevelData = new LevelData { levelNumber = i, score = 0 };
                progressData.levels.Add(newLevelData);
            }
        }
        SaveProgress();
    }

    public void SetOptions()
    {
        AudioManager.Instance.SetMasterVolume(optionsData.masterVolume);
        AudioManager.Instance.SetMusicVolume(optionsData.musicVolume);
        AudioManager.Instance.SetSFXVolume(optionsData.sfxVolume);
        QualitySettings.SetQualityLevel(optionsData.qualityIndex);
        Screen.fullScreen = optionsData.isFullScreen;
    }

    public void SaveProgress()
    {
        dataManager.SaveProgress(progressData);
    }

    public void SaveOptions()
    {
        optionsData.masterVolume = AudioManager.Instance.GetMasterVolume();
        Debug.Log("Master Volume: " + optionsData.masterVolume);
        optionsData.musicVolume = AudioManager.Instance.GetMusicVolume();
        optionsData.sfxVolume = AudioManager.Instance.GetSFXVolume();
        optionsData.isFullScreen = Screen.fullScreen;
        optionsData.qualityIndex = QualitySettings.GetQualityLevel();
        dataOptionsManager.SaveOptions(optionsData);
    }

    public void SetLastLevel(int levelNumber)
    {
        lastLevel = levelNumber;
    }

    public int GetScore(int levelNumber)
    {
        LevelData levelData = progressData.levels.Find(l => l.levelNumber == levelNumber);
        if (levelData != null)
        {
            return levelData.score;
        }
        return 0; // Return 0 if no score is found for the level
    }


    public void EndGame(int score)
    {
        lastScore = score;
        if (lastLevel != -1)
        {
            LevelData levelData = progressData.levels.Find(l => l.levelNumber == lastLevel);
            if (levelData == null)
            {
                levelData = new LevelData { levelNumber = lastLevel, score = lastScore };
                progressData.levels.Add(levelData);
            }
            else if (levelData.score < lastScore)
            {
                levelData.score = lastScore;
            }
            if (score > 0 && lastLevel < unlockedLevels.Length - 1)
            {
                unlockedLevels[lastLevel + 1] = true;
                LevelData newLevelData = new LevelData { levelNumber = lastLevel + 1, score = 0 };
                progressData.levels.Add(newLevelData);
            }
            SaveProgress();
            SceneManager.LoadScene("EndGameScene");
        }
        else
        {
            Debug.LogWarning("No level set for the game end.");
        }

    }

    public int GetLastScore()
    {
        return lastScore;
    }

    public void ResetGame()
    {
        lastLevel = -1;
        lastScore = -1;
        progressData = new ProgressData();
        for (int i = 1; i < unlockedLevels.Length; i++)
        {
            unlockedLevels[i] = false;
        }
        SaveProgress();
        Debug.Log(progressData.levels.Count + " levels reset.");
    }
    
}
