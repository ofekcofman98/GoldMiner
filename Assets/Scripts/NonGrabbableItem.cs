using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "NonGrabbableItem", menuName = "Items/NonGrabbable Item")]
public abstract class NonGrabbableItem : ItemData
{
    // son of Item, the claw knows NOT to grab them, they have special effect 
    public override abstract void Collect();
    public abstract void Activate();
}
