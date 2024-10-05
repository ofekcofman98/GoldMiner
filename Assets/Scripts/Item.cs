using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{

    public ItemData itemData;
    private SpriteRenderer spriteRenderer;
    private Transform clawTransform; 

    public bool AlreadyDestroyed {get; set;}

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize()
    {
        if (itemData == null)
        {
            Debug.LogError($"itemData is null in Initialize for {gameObject.name}!");
            return;
        }

        if (spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer is missing on {gameObject.name}!");
            return;
        }

        Debug.Log($"Initializing {itemData.itemName} with size {itemData.size}");

        // Setting sprite and size based on ItemData
        spriteRenderer.sprite = itemData.sprite;
        transform.localScale = new Vector3(itemData.size, itemData.size, 1);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        // when the claw collides with items
        // grabbable item: the claw grabs them
        // non-grabbable item: the claw doesnt grab them but activate the special effect
        if(other.CompareTag("Claw"))
        {

            if (itemData == null)
            {
                Debug.LogError($"{gameObject.name}: itemData is null during collision with claw!");
                return; 
            }

            AudioManager.Instance.PlaySound(itemData.sound);
            if (PlayerController.Instance.IsDrillActive())
            {
                CollectAndDestroyItem();
            }
            else
            {
                if (itemData is GrabbableItemData)
                {
                    GameManager.Instance.OnItemClawCollision(this);
                    clawTransform = other.transform;
                    transform.SetParent(clawTransform);
                }
                else if (itemData is NonGrabbableItem nonGrabbableItem)
                {
                    PlayerController.Instance.StopClawMovement();
                    if (itemData is BoosterItem boosterItem)
                    {
                        BoosterManager.Instance.ActivateBooster(boosterItem);
                    }
                    else
                    {
                        itemData.Collect();
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    public int GetScore()
    {
        int score = 0;
        if (itemData is GrabbableItemData grabbableItem)
        {
            score = grabbableItem.score;
        }
        return score; // Non-grabbable items don't have a score
    }

    private void CollectAndDestroyItem()
    {
        if (itemData != null)
        {
            if (itemData is GrabbableItemData)
            {
                GameManager.Instance.AddScore(this);
            }
            else if (itemData is NonGrabbableItem nonGrabbableItem)
            {
                if (itemData is BoosterItem boosterItem)
                {
                    BoosterManager.Instance.ActivateBooster(boosterItem);
                }
            } 
            else
            {
                itemData.Collect();
            }
            Destroy(gameObject);
            Debug.Log($"{itemData.itemName} collected by drill.");
        }
    }

}
