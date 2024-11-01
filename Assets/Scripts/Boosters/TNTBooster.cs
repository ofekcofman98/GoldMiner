using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TNTBooster", menuName = "Items/NonGrabbableItem/Booster/TNTBooster")]
public class TNTBooster : BoosterItem
{
    [SerializeField] private AudioClip _explosionSound;
    private void OnEnable()
    {
        BoosterType = BoosterType.Stored;
    }

    public override void Activate()
    {
        Debug.Log("TNT booster is stored!");
        AudioManager.Instance.PlaySound(_explosionSound);
        PlayerController.Instance.DestroyGrabbedItem();
        PlayerController.Instance.StopGrabbing();
        PlayerController.Instance.SetThrustSpeedToInitial();

        // GameObject explosionEffect = Instantiate(explosionPrefab, PlayerController.Instance.transform.position, Quaternion.identity);
        // Destroy(explosionEffect, 2f);  // Destroy effect after 2 seconds

    }
}
