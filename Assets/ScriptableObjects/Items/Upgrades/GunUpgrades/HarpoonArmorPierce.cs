using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Upgrades/Harpoon/PierceUpgrade", order = 5)]
public class HarpoonArmorPierce : UpgradeSO
{
    [SerializeField] float additionalPierce;
    public override void PickUpUpgrade(PlayerController player)
    {
        Harpoon.armorPierce += additionalPierce;
    }
}

