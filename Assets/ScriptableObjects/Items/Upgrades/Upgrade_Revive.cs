using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Upgrades/Revive", order = 0)]
public class Upgrade_Revive : UpgradeSO
{
    [SerializeField] float reviveHealthMultiplier;
    [SerializeField] float deathTimerMultiplier;
    [SerializeField] float bonusArmor;

    public override void PickUpUpgrade(PlayerController player)
    {
        player.playerInfo.healthRecivedAfterRevive *= reviveHealthMultiplier;
        player.playerInfo.deathTimer *= deathTimerMultiplier;
        player.playerInfo.armor += bonusArmor;
    }
}
