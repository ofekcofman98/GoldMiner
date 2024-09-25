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
    private bool isGameOver = false;

    [SerializeField] private int _width = 40;
    [SerializeField] private int _height = 12;
    [SerializeField] private GameObject _wallPrefab;

    [Header("GameLoop")]
    private bool cleanHiScore = false;
    private int _currentScore;
    private string _playerName = "Player";

    private int _hiScore;

    [Header("Items")]
    private List<Item> _currentItems = new();
    public GameObject itemPrefab;
    public List<Level> levels;

    private int currentLevelIndex = 0;

    internal int RightBorder => (_width / 2) + 1;
    internal int LeftBorder => -1 * (_width / 2 + 1);
    internal int BottomBorder => -1 * (_height / 2 + 1);

    void Start()
    {
        HiScoreManager.Instance.LoadTopFiveScores();

        StartGame();       
    }


    private void StartGame()
    {
        // SceneManager.LoadScene("MainScene");


        if (cleanHiScore)
        {
            HiScoreManager.Instance.ResetTopFiveScores();
            CanvasManager.Instance.UpdateHiScore(0);
        }

        CreateWalls();
        _hiScore = PlayerPrefs.GetInt(HiScore, 0);
        _currentScore = 0;

        LoadLevel(currentLevelIndex);

        List<HiScoreManager.ScoreEntry> topScores = HiScoreManager.Instance.GetTopScores();
        
        if (topScores.Count > 0)
        {
            CanvasManager.Instance.UpdateHiScore(topScores[0].score);
        }
        CanvasManager.Instance.UpdateScoreText(0);
        // CanvasManager.Instance.UpdateHiScore(_hiScore);
    
        // HiScoreManager.Instance.LoadTopFiveScores();
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < levels.Count)
        {
            Level currentLevel = levels[levelIndex];
            Debug.Log($"Level {levelIndex + 1} starts now!");
            LevelManager.Instance.StartLevel(currentLevel, itemPrefab);
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
        if(currentLevelIndex < levels.Count)
        {
            LevelManager.Instance.ClearItems();
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("No more levels!");
        }
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

    void SpawnItem(ItemData itemData, Vector2 position)
    {
        if(itemPrefab == null)
        {
            Debug.LogError("ItemPrefab is not assigned!");
            return;
        }
        if(itemData == null)
        {
            Debug.LogError("itemData is null!");
            return;
        }

        GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
        newItem.transform.position = position;
        Item itemComponent = newItem.GetComponent<Item>();
        
        if (itemComponent != null)
        {
            itemComponent.itemData = itemData;
            itemComponent.Initialize();
            Debug.Log($"Spawned item: {itemData.itemName} at position {position}"); 
            newItem.SetActive(true);
        }
            else
        {
            Debug.LogError("Item component not found on instantiated prefab!");
        }

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
            Debug.Log($"current score: {_currentScore}");
            
            HiScoreManager.Instance.CheckForTopFive(_currentScore, _playerName);
            // if (_currentScore > _hiScore)
            // {
            //     _hiScore = _currentScore;
            //     PlayerPrefs.SetInt(HiScore, _hiScore);
            //     PlayerPrefs.Save();
            //     CanvasManager.Instance.UpdateHiScore(_hiScore);
            // }

            CanvasManager.Instance.ShowItemScore(itemScore, item.transform.position);
            CanvasManager.Instance.UpdateScoreText(_currentScore);

            List<HiScoreManager.ScoreEntry> topScores = HiScoreManager.Instance.GetTopScores();
            if (topScores.Count > 0)
            {
                CanvasManager.Instance.UpdateHiScore(topScores[0].score);
            }

        }
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }


    public void EndGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");
        Time.timeScale = 0;
    }

    private void ClearCurrentItems()
    {
        foreach (var item in _currentItems)
        {
            DestroyItem(item);
        }
        _currentItems.Clear();
    }

    private void DestroyItem(Item item)
    {
        Destroy(item.gameObject);  
    }
}
