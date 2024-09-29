using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BombItem", menuName = "Items/NonGrabbableItem/Bomb")]
public class BombItem : NonGrabbableItem
{
    public override void Collect()
    {
        Activate();
    }

    public override void Activate()
    {
        if (this.sound != null)
        {
            AudioManager.Instance.PlaySound(this.sound);
        }
        Debug.Log("Bomb exploded! Player takes damage.");
        MenuManager.Instance.ShowBombPanel();
    }
}
