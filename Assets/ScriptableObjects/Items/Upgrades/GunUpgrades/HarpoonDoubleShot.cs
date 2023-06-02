using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Upgrades/Harpoon/DoubleShot", order = 5)]
public class HarpoonDoubleShot : UpgradeSO
{

    public override void PickUpUpgrade(PlayerController player)
    {
        Harpoon.doubleShot = true;
    }
}


