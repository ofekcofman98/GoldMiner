using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterItem", menuName = "Items/NonGrabbableItem/Booster")]
public class BoosterItem : NonGrabbableItem
{
    public override void Collect()
    {
        Debug.Log("Booster activated! Player speed increased.");
    }

}
