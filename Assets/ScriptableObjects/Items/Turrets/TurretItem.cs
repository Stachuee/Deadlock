using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Turret", order = 2)]
public class TurretItem : ItemSO
{
    public GameObject turretPrefab;

    public override void Drop(PlayerController player, Item item)
    {
        if(player != null && !player.isScientist)
        {
            Instantiate(turretPrefab, item.gameObject.transform.position, Quaternion.identity);
            Destroy(item.gameObject);
        }
    }

    public override bool PickUp(PlayerController player, Item item, out bool destroy)
    {
        destroy = true;
        return true;
    }
}
