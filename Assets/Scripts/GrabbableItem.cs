using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrabbableItem", menuName = "Items/Grabbable Item")]
public class GrabbableItemData : ItemData
{
    // son of Item - the claw knows to grab them, they has a score value and weight 
    public int score;
    public float weight = 1f;
    public AudioClip collectSound;

    public override void Collect()
    {
        AudioManager.Instance.PlaySound(collectSound);
        Debug.Log($"Collected {itemName} worth {score} points!");
    }
}
