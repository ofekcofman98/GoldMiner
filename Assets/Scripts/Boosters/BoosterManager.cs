using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : Singleton<BoosterManager>
{
    [SerializeField] private GameObject storedItemsContainer;
    [SerializeField] private GameObject storedItemsPrefab;

    private IBooster nextThrustBooster;
    private List<IBooster> storedBoosters = new List<IBooster>();

    private bool v_IsDrillActive;
        
    private void Start()
    {
        v_IsDrillActive = false;
    }

    public void ActivateBooster(IBooster booster)
    {
        if (booster is BoosterItem boosterItem)
        {
            switch (boosterItem.BoosterType)
            {
                case BoosterType.Instant:
                    booster.Activate();
                    break;
                
                case BoosterType.NextThrust:
                    QueueBoosterForNextThrust(booster);
                    break;

                case BoosterType.Stored:
                    StoreBoosterForLater(booster);
                    break;
            }
        }
    }

    private void QueueBoosterForNextThrust(IBooster booster)
    {
        Debug.Log("for next thrust!");
        nextThrustBooster = booster;
    }

    public void ApplyingNextThrustBooster()
    {
        if (nextThrustBooster != null)
        {
            PlayerController.Instance.ActivateNextThrustBooster();
            nextThrustBooster.Activate();
            nextThrustBooster = null;
        }
    }


    private void StoreBoosterForLater(IBooster booster)
    {
        if (storedBoosters.Count < 3)
        {
            storedBoosters.Add(booster);
            CanvasManager.Instance.UpdateStoredItemsContainer(storedBoosters);
            Debug.Log("item stored for later!");
        }
    }

    public void UseStoredBooster(BoosterType boosterType)
    {
        for (int i = 0; i < storedBoosters.Count; i++)
        {
            if ((storedBoosters[i] is BoosterItem boosterItem) &&
                (boosterItem.BoosterType == boosterType))
            {
                storedBoosters[i].Activate();
                storedBoosters.RemoveAt(i);
                return;
            }
        }
        CanvasManager.Instance.UpdateStoredItemsContainer(storedBoosters);
    }

    public bool IsTNTStored()
    {
        bool isStored = false;
        foreach (var booster in storedBoosters)
        {
            if (booster is TNTBooster)
            {
                isStored = true;
                Debug.Log("yes tnt is stored");    
            }
        }
        return isStored;
    }

    public void HandleUpKey()
    {
        if (IsTNTStored())
        {
            UseStoredTNTBooster();
        }
    }

    public void ActivateDrill()
    {
        v_IsDrillActive = true;
    }

    public void InactivateDrill()
    {
        v_IsDrillActive = false;
    }

    public bool IsDrillActive()
    {
        return v_IsDrillActive;
    }

    public void UseStoredTNTBooster()
    {
        for (int i = 0; i < storedBoosters.Count; i++)
        {
            if (storedBoosters[i] is TNTBooster)
            {
                storedBoosters[i].Activate();
                storedBoosters.RemoveAt(i);
                CanvasManager.Instance.UpdateStoredItemsContainer(storedBoosters);
                return;
            }
        }
    }

    public void ClearAllStoredBoosters()
    {
        storedBoosters.Clear();
        CanvasManager.Instance.UpdateStoredItemsContainer(storedBoosters);
        Debug.Log("All stored boosters cleared.");
    }


    
}
