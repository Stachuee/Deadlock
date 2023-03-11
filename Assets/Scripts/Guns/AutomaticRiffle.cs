using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutomaticRiffle : GunBase
{

    [SerializeField] float fireRate = 0.1f;
    bool isShooting = false;
    public override void Reload()
    {
        currentAmmo = maxAmmo;
    }

    public override void Shoot()
    {
        Vector2 diff = (owner.currentAimDirection).normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Instantiate(bulletPrefab, barrel.position, Quaternion.Euler(0, 0, rot_z));
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && !isShooting)
        {
            StartCoroutine(AutoShoot());
        }
    }

    IEnumerator AutoShoot()
    {
        isShooting = true;
        while (Mouse.current.leftButton.isPressed)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
        isShooting = false;
    }
}
