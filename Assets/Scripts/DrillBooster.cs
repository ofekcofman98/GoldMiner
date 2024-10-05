using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrillBooster", menuName = "Items/NonGrabbableItem/Booster/DrillBooster")]
public class DrillBooster : BoosterItem
{
    [SerializeField] private Sprite _drillSprite;
    private void OnEnable()
    {
        BoosterType = BoosterType.NextThrust;
    }

    public override void Activate()
    {
        Debug.Log("Driller!!!");
        PlayerController.Instance.SetDrillActive();
        PlayerController.Instance.ChangeClawSprite(_drillSprite);
        PlayerController.Instance.IncreaseColliderSizeForDrill(1.5f);
    }

}
