using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : Singleton<GameManager>
{
    private const string HiScore = "HiScore";

    [SerializeField] private int _width = 40;
    [SerializeField] private int _height = 12;
    [SerializeField] private GameObject _wallPrefab;

    [Header("GameLoop")]
    private bool cleanHiScore = false;
    private int _currentScore;
    private int _currentHiScore;
    private string _playerName = "Player";

    private int _hiScore;

    [Header("Items")]
    private List<Item> _currentItems = new();
    public GameObject itemPrefab;
    public List<Level> levels;

    private int currentLevelIndex = 0;
    private int _cumulativeScoreGoal = 0;

    private int currentLifeNumber;

    internal int RightBorder => (_width / 2) + 1;
    internal int LeftBorder => -1 * (_width / 2 + 1);
    internal int BottomBorder => -1 * (_height / 2 + 1);

    void Start()
    {
        HiScoreManager.Instance.LoadTopFiveScores();
    }


    public void StartGame()
    {
        if (cleanHiScore)
        {
            HiScoreManager.Instance.ResetTopFiveScores();
            CanvasManager.Instance.UpdateHiScore(0);
        }

        CreateWalls();
        _hiScore = PlayerPrefs.GetInt(HiScore, 0);
        _currentScore = 0;

        LifeManager.Instance.SetLifeNumberToMax();
        LifeManager.Instance.ShowLifeOnScreen();

        LoadLevel(currentLevelIndex);

        BoosterManager.Instance.ClearAllStoredBoosters();
        List<HiScoreManager.ScoreEntry> topScores = HiScoreManager.Instance.GetTopScores();
        
        if (topScores.Count > 0)
        {
            _currentHiScore = topScores[0].score;
            CanvasManager.Instance.UpdateHiScore(_currentHiScore);
        }

        CanvasManager.Instance.UpdateScoreText(0);
    }

    public void LoadLevel(int levelIndex)
    {
        if (CheckForNextLevel(currentLevelIndex))
        {
            Level currentLevel = levels[levelIndex];
            Debug.Log($"score goal: {currentLevel.scoreGoal}");
            
            _cumulativeScoreGoal += currentLevel.scoreGoal;
            Debug.Log($"Level {levelIndex + 1} starts now!");
            MenuManager.Instance.ShowLevelStartPanel(levelIndex + 1, _cumulativeScoreGoal, currentLevel.comment);
            LevelManager.Instance.PrepareLevel(currentLevel, itemPrefab);
        }
        else
        {
            Debug.LogError("No more levels");
            EndGame();
        }
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        if(CheckForNextLevel(currentLevelIndex))
        {
            LevelManager.Instance.ClearItems();
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("No more levels!");
            EndGame();
        }
    }

    public bool CheckForNextLevel(int currentLevelIndex)
    {
        return (currentLevelIndex < levels.Count);
    }

    public int GetCumulativeScoreGoal()
    {
        return _cumulativeScoreGoal;
    }


    private void CreateWalls()
    {
        CreateWall("RightWall",RightBorder, 0, 1, _height+1);
        CreateWall("LeftWall", LeftBorder, 0, 1, _height+1);
        CreateWall("BottomWall", 0, BottomBorder, _width+3, 1);
    }

    private void CreateWall(string name, int x, int y, int height, int width)
    {
        var wall = Instantiate(_wallPrefab);
        wall.name = name;
        wall.transform.position = new Vector3(x, y, 0);
        wall.transform.localScale = new Vector3(height, width, 0);
    }

    public void OnItemClawCollision(Item item)
    {
        PlayerController.Instance.StopClawMovement();
        PlayerController.Instance.Grab(item);        
    }

    public void AddScore(Item item)
    {
        int itemScore = item.GetScore();
        if (itemScore > 0)
        {
            _currentScore += itemScore;
            Debug.Log($"Current score: {_currentScore}");

            CanvasManager.Instance.ShowItemScore(itemScore, item.transform.position);
            CanvasManager.Instance.UpdateScoreText(_currentScore);
            
            // List<HiScoreManager.ScoreEntry> topScores = HiScoreManager.Instance.GetTopScores();
            if (_currentScore > _currentHiScore)
            {
                _currentHiScore = _currentScore;
                CanvasManager.Instance.UpdateHiScore(_currentHiScore);
            }
        }
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }


    public void EndGame()
    {
        Debug.Log("Game Over!");
        currentLevelIndex = 0;
        _cumulativeScoreGoal = 0;

        // Only now, after the game ends, check if the final score qualifies for the top 5.
        HiScoreManager.Instance.CheckForTopFive(_currentScore, _playerName);

        if (HiScoreManager.Instance.CheckIfPlayerIsInTopFive())
        {
            MenuManager.Instance.ShowNameEntryPanel();
        }
        else
        {
            MenuManager.Instance.ShowGameOverMenu();
        }

        Time.timeScale = 0;
        PlayerController.Instance.SetClawBackToInitial();
    }

}
