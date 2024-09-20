using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BombItem", menuName = "Items/NonGrabbableItem/Bomb")]
public class BombItem : NonGrabbableItem
{
    public override void Collect()
    {
        BombActivate();
    }

    private void BombActivate()
    {
        Debug.Log("Bomb exploded! Player takes damage.");
    }
}
