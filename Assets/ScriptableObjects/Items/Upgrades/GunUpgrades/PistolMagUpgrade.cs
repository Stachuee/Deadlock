using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Upgrades/Pistol/MagUpgrade", order = 4)]
public class PistolMagUpgrade : UpgradeSO
{
    [SerializeField] int additionalAmmo;
    [SerializeField] float fireRateMultiplier;
    public override void PickUpUpgrade(PlayerController player)
    {
        Pistol.MAX_AMMO += additionalAmmo;
        Pistol.shootDelay *= fireRateMultiplier;
    }
}
