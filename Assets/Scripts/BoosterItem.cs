using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "BoosterItem", menuName = "Items/NonGrabbableItem/Booster")]
public abstract class BoosterItem : NonGrabbableItem
{

    public override ItemType itemType => ItemType.Booster;  // Booster type for all boosters

    public override void Collect()
    {
        Activate();
    }
    
    public abstract override void Activate();

}
