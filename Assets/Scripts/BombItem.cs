using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BombItem", menuName = "Items/NonGrabbableItem/Bomb")]
public class BombItem : NonGrabbableItem
{
    // non-grabbable item
    public override void Collect()
    {
        Activate();
    }

    public override void Activate()
    {
        Debug.Log("Bomb exploded! Player takes damage.");
        MenuManager.Instance.ShowBombPanel();
    }
}
