using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Upgrades/StatusMonitor", order = 4)]
public class Upgrade_StatusMonitor : UpgradeSO
{
    public override void PickUpUpgrade(PlayerController player)
    {
        GameController.playersConnected = true;
    }
}
