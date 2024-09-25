using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "NonGrabbableItem", menuName = "Items/NonGrabbable Item")]
public abstract class NonGrabbableItem : ItemData
{
    public override abstract void Collect();
    public abstract void Activate();
}
