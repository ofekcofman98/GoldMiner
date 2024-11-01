using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrillBooster", menuName = "Items/NonGrabbableItem/Booster/DrillBooster")]
public class DrillBooster : BoosterItem
{
    [SerializeField] private Sprite _drillSprite;
    [SerializeField] private float _spriteSize = 1.5f;

    private void OnEnable()
    {
        BoosterType = BoosterType.NextThrust;
    }

    public override void Activate()
    {
        Debug.Log("Driller!!!");
        BoosterManager.Instance.ActivateDrill();
        PlayerController.Instance.ChangeClawSprite(_drillSprite);
        PlayerController.Instance.IncreaseColliderSizeForDrill(_spriteSize);
    }

}
