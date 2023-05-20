using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunBase
{
    [SerializeField] private List<GameObject> bullets;
    private int currentBulletIndex = 0;

    public float shootDelay = 0.2f;
    private float shootTimer = 0f; // time elapsed since last shot

    private Bullet bullet;

    [SerializeField] int maxAmmo;
    int currentAmmo;


    protected override void Start()
    {
        base.Start();
        bulletPrefab = bullets[currentBulletIndex];
        bullet = bulletPrefab.GetComponent<Bullet>();
        currentAmmo = 0;
        Reload();
    }
    public override void Reload()
    {
        while (currentAmmo < 12 && maxAmmo > 0)
        {
            currentAmmo++;
            maxAmmo--;
        }
    }

    public override void AddAmmo(AmmoType aT, int amount)
    {
        switch (aT)
        {
            case AmmoType.Bullet:
                currentAmmo += amount;
                break;
            default:
                Debug.LogError($"Wrong AmmoType({aT}) for Pistol!");
                break;
        }
    }

    private void Update()
    {
        if (bullet.GetDamageType() == DamageType.Bullet && currentAmmo <= 0) return;

        if (isShooting >= 0.9f)
        {
            if (Time.time < shootTimer + shootDelay)
            {
                return; // not enough time has passed since last shot
            }

            Vector2 diff = (owner.currentAimDirection).normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            Instantiate(bulletPrefab, barrel.position, Quaternion.Euler(0, 0, rot_z));
            if (bullet.GetDamageType() == DamageType.Bullet)
                currentAmmo--;

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

    override public string GetAmmoAmount()
    {
        if (bullet.GetDamageType() == DamageType.Bullet)
            return currentAmmo.ToString()+"/"+maxAmmo;

        return "";
    }

    override public DamageType GetBulletType()
    {
        return bullet.GetDamageType();
    }
}
