using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelManager : MonoBehaviour
{
    [Header("Items")]
    public List<Item> currentItems = new List<Item>();
    public List<Item> grabbableItems = new List<Item>(); 
    
    [Header("Time")]
    public float levelTimeLimit = 60f;
    private float timeRemaining;
    private bool timerRunning = false;
    
    public void StartLevel(Level levelData, GameObject itemPrefab)
    {
        ClearItems();
        timeRemaining = levelTimeLimit;
        LoadItems(levelData, itemPrefab);
        StartTimer();
    }

    private void StartTimer()
    {
        timeRemaining = levelTimeLimit;
        timerRunning = true;
    }

    private void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                CanvasManager.Instance.UpdateTimerText(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                OnTimeUp();
            }
        }
    }


    private void LoadItems(Level levelData, GameObject itemPrefab)
    {
        foreach(var itemPositions in levelData.itemPositions)
        {
            SpawnItem(itemPositions.itemData, itemPositions.position, itemPrefab);
        }
    }

private void SpawnItem(ItemData itemData, Vector2 position, GameObject itemPrefab)
{
    Debug.Log($"Spawning item: {itemData?.itemName} at position {position}");
    GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
    Item itemComponent = newItem.GetComponent<Item>();
    if (itemComponent != null)
    {
        if (itemData == null)
        {
            Debug.LogError("ItemData is null when trying to assign to the item!");
        }
        itemComponent.itemData = itemData;
        itemComponent.Initialize();
        currentItems.Add(itemComponent);

        if (itemData is GrabbableItemData)
        {
            grabbableItems.Add(itemComponent);
        }
    }
}

    public void OnItemDestroyed(Item item)
    {
        if (currentItems.Contains(item))
        {
            currentItems.Remove(item);
            Debug.Log($"Remaining items: {currentItems.Count}");

            if (grabbableItems.Contains(item))
            {
                grabbableItems.Remove(item);
                Debug.Log($"Remaining grabbable items: {grabbableItems.Count}");
            }

            if (grabbableItems.Count == 0)
            {
                Debug.Log("No more grabbable Items!");
                FinishLevel();
            }
        }
    }

    public void AddTime(float seconds)
    {
        timeRemaining += seconds;
        CanvasManager.Instance.UpdateTimerText(timeRemaining);
        Debug.Log($"{seconds} seconds added! Time remaining: {timeRemaining}");
    }

    public void OnTimeUp()
    {
        Debug.Log("Time is up!");
        FinishLevel();
    }

    private void FinishLevel()
    {
        GameManager.Instance.NextLevel();
    }


public void ClearItems()
{
    for (int i = currentItems.Count - 1; i >= 0; i--)
    {
        if (currentItems[i] != null)
        {
            Destroy(currentItems[i].gameObject);  
        }
        currentItems.RemoveAt(i);
    }
    grabbableItems.Clear();
}
}
