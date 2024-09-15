using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private const string HiScore = "HiScore";

    [SerializeField] private int _width = 20;
    [SerializeField] private int _height = 12;
    [SerializeField] private GameObject _wallPrefab;

    [Header("GameLoop")]
    // [SerializeField] private int _initialLive = 3;
    // private int _currentLives;
    private int _currentScore;
    private int _hiScore;
    
    [Header("Items")]
    // [SerializeField] private Item _itemPrefab;
    private List<Item> _currentItems = new();
    public GameObject itemPrefab;
    public List<Level> levels;
    private int currentLevelIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        StartGame();        
    }

    private void StartGame()
    {
        CreateWalls();
        _hiScore = PlayerPrefs.GetInt(HiScore, 0);
        _currentScore = 0;

        // PlayerController.Instance.InitializePlayer();
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < levels.Count)
        {
            Level currentLevel = levels[levelIndex];
            Debug.Log($"Level {levelIndex + 1} starts now!");
            foreach (var itemPositions in currentLevel.itemPositions)
            {
                SpawnItem(itemPositions.itemData, itemPositions.position);
            }
        }
        else
        {
            Debug.LogError("Level index is out oh bounds");
        }
    }

    public void NewLevel()
    {
        currentLevelIndex++;
        if(currentLevelIndex < levels.Count)
        {
            ClearCurrentItems();
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("No more levels!");
        }
    }

    private void ClearCurrentItems()
    {
        foreach (var item in _currentItems)
        {
            DestroyItem(item);
        }
        _currentItems.Clear();
    }

    internal int RightBorder => (_width / 2) + 1;
    internal int LeftBorder => -1 * (_width / 2 + 1);
    // public int TopBorder => (_height / 2) + 1;
    internal int BottomBorder => -1 * (_height / 2 + 1);

    private void CreateWalls()
    {
        CreateWall("RightWall",RightBorder, 0, 1, _height+1);
        CreateWall("LeftWall", LeftBorder, 0, 1, _height+1);
        // CreateWall("TopWall", 0, TopBorder, _width+3, 1);
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
        GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
        newItem.transform.position = position;
        newItem.SetActive(true);

        Item itemComponent = newItem.GetComponent<Item>();
        itemComponent.itemData = itemData;

        itemComponent.Initialize();
    }

    public void OnItemClawCollision(/*Claw claw,*/ Item item)
    {
        PlayerController.Instance.StopClawMovement();
        PlayerController.Instance.Grab(item);
        
        int itemScore = item.GetScore();
        if (itemScore > 0)
        {
            _currentScore += itemScore;
            Debug.Log($"current score: {_currentScore}");
            
            if (_currentScore > _hiScore)
            {
                _hiScore = _currentScore;
                PlayerPrefs.SetInt(HiScore, _hiScore);
                PlayerPrefs.Save();

                // CanvasManager.Instance.UpdateHiScore(_hiScore);
            }   
            // CanvasManager.Instance.UpdateCurrentScore(_currentScore);
        }
    }

    private void DestroyItem(Item item)
    {
        Destroy(item.gameObject);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
