using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunBase
{
    public override void Reload()
    {
        currentAmmo = maxAmmo;
    }

    public override void Shoot()
    {
        Vector2 diff = (owner.currentAimDirection).normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Instantiate(bulletPrefab, barrel.position, Quaternion.Euler(0,0, rot_z));
    }
}
