using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyBooster", menuName = "Items/NonGrabbableItem/Booster/KeyBooster")]
public class KeyBooster : BoosterItem, IBooster
{
    [SerializeField] private ChestItem chestItem;
    private void OnEnable()
    {
        BoosterType = BoosterType.Instant;
    }

    public void SetChestReference(ChestItem chest)
    {
        chestItem = chest;
    }

    public override void Activate()
    {
        if (chestItem != null)
        {
            chestItem.OpenChest();
        }
        else
        {
            Debug.LogWarning("No chest assigned to this key booster!");
        }
    }
}
