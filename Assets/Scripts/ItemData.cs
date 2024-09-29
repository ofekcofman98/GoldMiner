using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public float size;
    public AudioClip sound;

    public virtual ItemType itemType {get;}

    public abstract void Collect();
 

    public enum ItemType
    {
        Gold,
        Diamond,
        Rock,
        Bomb,
        Booster
    }

}
