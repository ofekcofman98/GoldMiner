using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrabbableItem", menuName = "Items/Grabbable Item")]
public class GrabbableItemData : ItemData
{
    public int score;
    public float weight = 1f;

    public override void Collect()
    {
        // Common collection behavior for grabbable items
        Debug.Log($"Collected {itemName} worth {score} points!");
    }
}
