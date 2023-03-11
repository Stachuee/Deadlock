using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : GunBase
{

    public float shootDelay = 0.5f;
    private float shootTimer = 0f; // time elapsed since last shot
    public override void Reload()
    {
        currentAmmo = maxAmmo;
    }

    public override void Shoot()
    {
        if (Time.time < shootTimer + shootDelay)
        {
            return; // not enough time has passed since last shot
        }

        Vector2 diff = (owner.currentAimDirection).normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Instantiate(bulletPrefab, barrel.position, Quaternion.Euler(0, 0, rot_z));

        shootTimer = Time.time; // reset timer to current time
    }
}
