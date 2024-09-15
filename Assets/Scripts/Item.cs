using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public ItemData itemData;
    // [SerializeField] public int score;
    // [SerializeField] public float size;
    private SpriteRenderer spriteRenderer;
    private Transform clawTransform; 

    public bool AlreadyDestroyed {get; set;}

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize()
    {
        if (itemData != null && spriteRenderer != null)
        {
            Debug.Log("Setting sprite and size based on ItemData");
            spriteRenderer.sprite = itemData.sprite;

            // Set the item's size based on ItemData
            transform.localScale = new Vector3(itemData.size, itemData.size, 1);

            // Set the score from ItemData
            // score = itemData.score;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Claw"))
        {
            Collect();
            GameManager.Instance.OnItemClawCollision(/*other.GetComponent<Claw>(),*/ this);
            
            clawTransform = other.transform;
            transform.SetParent(clawTransform);

        }

    }

    public virtual void Collect()
    {
        Debug.Log("Item collected!");
        // if gold / diamond / rock:
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
