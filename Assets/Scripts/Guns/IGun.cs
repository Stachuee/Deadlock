using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    public void Shoot(bool isShooting);
    public void Reload(bool forceReload = false);
    public void AddAmmo(AmmoType aT, int amount);

    public void ChangeBulletType(bool input);

    public Transform GetGunTransform();
    public GunBase GetGunScript();
    public Transform GetBarrelTransform();
    public void EnableGun(bool isActive);
    public GameObject GetInventorySlotPrefab();
    public string GetAmmoAmount();
    public string GetBothAmmoString();

}
