using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Upgrades/SteelMuscles", order = 2)]
public class Upgrade_SteelMuscles : UpgradeSO
{
    [SerializeField]
    float throwStrengthMultiplier;
    [SerializeField]
    float speedMultiplier;

    public override void PickUpUpgrade(PlayerController player)
    {
        player.playerInfo.throwStrength *= throwStrengthMultiplier;
        player.playerInfo.speed *= speedMultiplier;
    }
}
