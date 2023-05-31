using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Upgrades/PneumaticImplants", order = 3)]
public class Upgrade_PneumaticImplants : UpgradeSO
{
    [SerializeField]
    float kickArmorShred;
    [SerializeField]
    float kickCooldownMultiplier;
    [SerializeField]
    float kickBonusDamage;

    public override void PickUpUpgrade(PlayerController player)
    {
        player.playerInfo.kickArmorShred += kickArmorShred;
        player.playerInfo.kickCooldown *= kickCooldownMultiplier;
        player.playerInfo.meleeDamage += kickBonusDamage;
    }
}
