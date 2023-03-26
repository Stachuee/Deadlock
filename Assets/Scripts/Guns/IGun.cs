using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    public void Shoot(float isShooting);
    public void Reload();

    public Transform GetGunTransform();
    public GunBase GetGunScript();
    public Transform GetBarrelTransform();
    public void EnableGun(bool isActive);


}
