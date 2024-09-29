using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeBooster", menuName = "Items/NonGrabbableItem/Booster/TimeBooster")]
public class TimeBoosterItem : BoosterItem
{
    [SerializeField] private float _timeBonus = 5f;

    public override void Activate()
    {
        if (this.sound != null)
        {
            AudioManager.Instance.PlaySound(this.sound);
        }
        Debug.Log($"Time booster Activated! {_timeBonus} seconds added.");
        CanvasManager.Instance.ShowTimeBonusText(_timeBonus);
        LevelManager.Instance.AddTime(_timeBonus);
    }

}
