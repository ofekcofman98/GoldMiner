using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeBooster", menuName = "Items/NonGrabbableItem/Booster/TimeBooster")]
public class TimeBoosterItem : BoosterItem
{
    [SerializeField] private float _timeBonus = 5f;

    public override void Activate()
    {
        Debug.Log($"Time booster Activated! {_timeBonus} seconds added.");
        LevelManager.Instance.AddTime(_timeBonus);
    }

}
