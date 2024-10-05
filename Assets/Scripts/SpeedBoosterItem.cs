using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBooster", menuName = "Items/NonGrabbableItem/Booster/SpeedBooster")]
public class SpeedBoosterItem : BoosterItem, IBooster
{
    [SerializeField] private float _speed = 12f;

    private void OnEnable()
    {
        BoosterType = BoosterType.NextThrust;
    }

    public override void Activate()
    {
        Debug.Log("Speed Booster is Activated!");
        PlayerController.Instance.CollectedBoosterForNextThrust(this);
    }

    public float GetSpeedBoost()
    {
        return _speed;
    }

    
}
