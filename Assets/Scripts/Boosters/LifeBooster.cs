using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeBooster", menuName = "Items/NonGrabbableItem/Booster/LifeBooster")]
public class LifeBooster : BoosterItem
{
    [SerializeField] private int _lifeToAdd = 1;

    private void OnEnable()
    {
        BoosterType = BoosterType.Instant;
        sprite = LifeManager.Instance.GetSprite();
    }
    
    public override void Activate()
    {
        Debug.Log($"Life booster Activated! {_lifeToAdd} lifes added.");
        LifeManager.Instance.IncreaseLife(_lifeToAdd);
    }
}
