using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrabbableItem", menuName = "Items/Grabbable Item")]
public class GrabbableItemData : ItemData
{
    public int score;
    public float weight = 1f;
    public AudioClip collectSound;

    public override void Collect()
    {
        AudioManager.Instance.PlaySound(collectSound);
        Debug.Log($"Collected {itemName} worth {score} points!");
    }
}
