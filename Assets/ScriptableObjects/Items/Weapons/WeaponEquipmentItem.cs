using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Items/Weapons", order = 6)]
public class WeaponEquipmentItem : ItemSO
{
    [SerializeField] WeaponType weaponType;

    [SerializeField] ScienceItem unlocks;
    public override void Drop(PlayerController player, Item item)
    {
        
    }

    public override bool PickUp(PlayerController player, Item item, out bool destroy)
    {
        if (!player.isScientist)
        {
            player.gunController.UnlockWeapon(weaponType);
            RecipiesManager.instance.UnlockTech(unlocks);
            destroy = true;
            return false;
        }
        else
        {
            destroy = true;
            return true;
        }
    }
}
