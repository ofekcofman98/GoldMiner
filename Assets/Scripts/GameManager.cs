using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private const string HiScore = "HiScore";

    [Header("GameLoop")]
    // [SerializeField] private int _initialLive = 3;
    // private int _currentLives;
    private int _currentScore;
    private int _hiScore;
    
    [Header("Items")]
    [SerializeField] private Item _itemPrefab;
    private List<Item> _currentItems = new();



    // Start is called before the first frame update
    void Start()
    {
        StartGame();        
    }

    private void StartGame()
    {
        _hiScore = PlayerPrefs.GetInt(HiScore, 0);
        _currentScore = 0;

        // PlayerController.Instance.InitializePlayer();
    }

    public void OnItemClawCollision(/*Claw claw,*/ Item item)
    {
        PlayerController.Instance.StopClawMovement();
        PlayerController.Instance.Grab(item);
        //item.Collect();
        _currentScore += item.score;
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

    private void DestroyItem(Item item)
    {
        Destroy(item.gameObject);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
