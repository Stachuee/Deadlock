using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Upgrades/DeepPockets", order = 1)]
public class Upgrade_DeepPockets : UpgradeSO
{
    [SerializeField]
    float bonusAmmo;

    public override void PickUpUpgrade(PlayerController player)
    {
        player.playerInfo.bonusAmmo += bonusAmmo;
    }
}
