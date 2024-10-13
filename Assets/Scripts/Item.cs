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
    
    // public delegate void ItemCollectedHandler(Item item);
    // public static event ItemCollectedHandler OnItemCollected;
    
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

        // SetColliderBasedOnSprite();
    }

    // public void SetColliderBasedOnSprite()
    // {
    //     // Try to get the CircleCollider2D component
    //     CircleCollider2D collider = GetComponent<CircleCollider2D>();

    //     // If no collider exists, add one
    //     if (collider == null)
    //     {
    //         Debug.Log("No CircleCollider2D found on the item. Adding a new one.");
    //         collider = gameObject.AddComponent<CircleCollider2D>();
    //         collider.isTrigger = true;
    //     }

    //     // Ensure the itemData and spriteRenderer are initialized
    //     if (itemData == null || spriteRenderer == null)
    //     {
    //         Debug.LogError("ItemData or SpriteRenderer is not assigned properly.");
    //         return;
    //     }

    //     // Get the bounds of the sprite and calculate the radius
    //     float spriteWidth = spriteRenderer.bounds.size.x;
    //     float spriteHeight = spriteRenderer.bounds.size.y;
    //     float maxDimension = Mathf.Max(spriteWidth, spriteHeight);

    //     // Use the colliderScaleFactor from itemData if it's set properly
    //     float scaleFactor = itemData.colliderScaleFactor;

    //     // Set the radius based on the max dimension of the sprite, adjusting with the scale factor
    //     collider.radius = maxDimension * scaleFactor * 0.3f;

    //     // Optionally, set the offset based on itemData if necessary
    //     collider.offset = new Vector2(0, -0.14f); // Example offset, adjust as needed

    //     // Log for debugging
    //     Debug.Log($"Collider set with radius: {collider.radius} for item: {itemData.itemName}");
    // }




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

            if (BoosterManager.Instance.IsDrillActive())
            {
                CollectItem();
            }
            else
            {
                HandleItemCollision(other.transform);
            }


        }
    }

    public void HandleItemCollision(Transform clawTransform)
    {
        if (itemData is GrabbableItemData)
        {
            GameManager.Instance.OnItemClawCollision(this);
            this.clawTransform = clawTransform;
            transform.SetParent(clawTransform);
        }
        else if (itemData is NonGrabbableItem)
        {
            PlayerController.Instance.StopClawMovement();
            
            if (itemData is ChestItem chestItem)
            {
                return;
            }

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

    public void CollectItem()
    {
        if (itemData != null)
        {
            if (itemData is GrabbableItemData)
            {
                GameManager.Instance.AddScore(this);
                itemData.Collect();
            }
            else if (itemData is NonGrabbableItem)
            {
                if (itemData is BoosterItem boosterItem)
                {
                    BoosterManager.Instance.ActivateBooster(boosterItem);
                }

                if (itemData is BombItem bombItem)
                {
                    bombItem.Activate();
                }
            }

            Destroy(gameObject);

            // OnItemCollected?.Invoke(this);
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


}
