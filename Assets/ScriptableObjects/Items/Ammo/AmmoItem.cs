using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Ammo", order = 1)]
public class AmmoItem : ItemSO
{
    [SerializeField]
    int ammoCount;

    [SerializeField] WeaponType weapon;
    [SerializeField] AmmoType ammoType;

    public override void Drop(PlayerController player, Item item)
    { 
         
    }

    public override bool PickUp(PlayerController player, Item item, out bool destroy)
    {
        player.gunController.AddAmmo(weapon, ammoType, ammoCount);
        destroy = true;
        if (player.isScientist) return true;
        else return false;
    }

}
