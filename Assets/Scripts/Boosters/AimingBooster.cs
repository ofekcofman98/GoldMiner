using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AimingBooster", menuName = "Items/NonGrabbableItem/Booster/AimingBooster")]
public class AimingBooster : BoosterItem, IBooster
{
    private void OnEnable()
    {
        BoosterType = BoosterType.NextThrust;
    }

    public override void Activate()
    {
        Debug.Log("Aiming is ON!");
        PlayerController.Instance.ActivateAimingBooster();
    }

}
