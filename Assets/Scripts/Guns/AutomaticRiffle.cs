using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutomaticRiffle : GunBase
{
    [SerializeField] private List<GameObject> bullets;
    private int currentBulletIndex = 0;

    [SerializeField] float fireRate = 0.1f;
    private float shootTimer = 0f; // time elapsed since last shot
    private Bullet bullet;

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int currentFireAmmo;
    [SerializeField] int currentPoisonAmmo;

    protected override void Start()
    {
        base.Start();
        bulletPrefab = bullets[currentBulletIndex];
        bullet = bulletPrefab.GetComponent<Bullet>();
    }

    public override void Reload()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (bullet.GetDamageType() == DamageType.Bullet && currentAmmo <= 0) return;
        else if (bullet.GetDamageType() == DamageType.Fire && currentFireAmmo <= 0) return;
        else if (bullet.GetDamageType() == DamageType.Poison && currentPoisonAmmo <= 0) return;

        if (isShooting >= 0.9f)
        {
            if (Time.time < shootTimer + fireRate)
            {
                return; // not enough time has passed since last shot
            }
            Vector2 diff = (owner.currentAimDirection).normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            Instantiate(bulletPrefab, barrel.position, Quaternion.Euler(0, 0, rot_z));
            if (bullet.GetDamageType() == DamageType.Bullet)
                currentAmmo--;
            else if (bullet.GetDamageType() == DamageType.Fire)
                currentFireAmmo--;
            else if (bullet.GetDamageType() == DamageType.Poison)
                currentPoisonAmmo--;

            shootTimer = Time.time; // reset timer to current time
        }
    }

    override public void ChangeBulletType(float input)
    {
        if (input >= 1)
        {
            currentBulletIndex = (currentBulletIndex + 1) % bullets.Count;
            bulletPrefab = bullets[currentBulletIndex];
            bullet = bulletPrefab.GetComponent<Bullet>();
        }
    }

    override public int GetAmmoAmount()
    {
        if (bullet.GetDamageType() == DamageType.Bullet)
            return currentAmmo;
        else if (bullet.GetDamageType() == DamageType.Fire)
            return currentFireAmmo;
        else if (bullet.GetDamageType() == DamageType.Poison)
            return currentPoisonAmmo;

        return 0;
    }

    override public DamageType GetBulletType()
    {
        return bullet.GetDamageType();
    }

}
