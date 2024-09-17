using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float levelTimeLimit = 60f;
    private float RemainingTime;
    public List<Item> currentItems = new List<Item>();
    public List<Item> grabbableItems = new List<Item>(); 
    
    public void StartLevel(Level levelData, GameObject itemPrefab)
    {
        ClearItems();
        RemainingTime = levelTimeLimit;
        LoadItems(levelData, itemPrefab);
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
        GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
        Item itemComponent = newItem.GetComponent<Item>();

        if (itemComponent != null)
        {
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
            Debug.Log($"Non-grabbable item destroyed! Remaining items: {currentItems.Count}");

            // Check if all items are destroyed
            if (grabbableItems.Count == 0)
            {
                GameManager.Instance.NextLevel();
            }
        }
    }


    public void ClearItems()
    {
        foreach (var item in currentItems)
        {
            Destroy(item.gameObject);
        }
        currentItems.Clear();
        grabbableItems.Clear();
    }
}
