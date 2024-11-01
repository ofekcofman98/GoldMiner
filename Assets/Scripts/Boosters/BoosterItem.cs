using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BoosterType
{
    Instant,    // happens immediatly
    NextThrust, // applied to the next claw thrust 
    Stored      // stored for later use
}
public abstract class BoosterItem : NonGrabbableItem, IBooster
{
    // public string boosterName;

    private BoosterType boosterType;
    public override ItemType itemType => ItemType.Booster;  // Booster type for all boosters
    public BoosterType BoosterType
    {
        get => boosterType;
        protected set => boosterType = value;
    }

    public override void Collect()
    {
        Activate();
    }
    
    public abstract override void Activate();

}
