using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestItem", menuName = "Items/NonGrabbableItem/Chest")]
public class ChestItem : NonGrabbableItem
{
    [SerializeField] private Sprite _closedChest;
    [SerializeField] private Sprite _openChest;
    [SerializeField] private Sprite _collectedChest;
    private SpriteRenderer chestSpriteRenderer;
    private GameObject chestInstance;

    // private void Awake()
    // {
    //     chestSpriteRenderer = GetComponent<SpriteRenderer>();
    //     if (chestSpriteRenderer == null)
    //     {
    //         Debug.LogError("SpriteRenderer component is missing from the chest object!");
    //     }
    //     else
    //     {
    //         chestSpriteRenderer.sprite = _closedChest;
    //     }
    // }

    public void SetChestInstance(GameObject chest)
    {
        chestInstance = chest;
    }


    public void OpenChest()
    {
        if (chestInstance != null)
        {
            SpriteRenderer chestSpriteRenderer = chestInstance.GetComponent<SpriteRenderer>();
            if (chestSpriteRenderer != null)
            {
                chestSpriteRenderer.sprite = _openChest;
            }
        }
    }

    public void CollectChest()
    {
        if (chestInstance != null)
        {
            SpriteRenderer chestSpriteRenderer = chestInstance.GetComponent<SpriteRenderer>();
            if (chestSpriteRenderer != null)
            {
                chestSpriteRenderer.sprite = _collectedChest;
            }
        }
    }
    public override void Collect()
    {
        Activate();
    }

    public override void Activate()
    {

    }
}
