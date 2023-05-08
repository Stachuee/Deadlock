using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : GunBase
{
    [SerializeField] private List<GameObject> bullets;
    private int currentBulletIndex = 0;

    public float shootDelay = 0.2f;
    private float shootTimer = 0f; // time elapsed since last shot

    private Bullet bullet;

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int currentPreciseAmmo;


    protected override void Start()
    {
        base.Start();
        bulletPrefab = bullets[currentBulletIndex];
        bullet = bulletPrefab.GetComponent<Bullet>();
    }
    public override void Reload()
    {
    }

    public override void AddAmmo(AmmoType aT, int amount)
    {
        switch (aT)
        {
            case AmmoType.Bullet:
                currentAmmo += amount;
                break;
            case AmmoType.Precise:
                currentPreciseAmmo += amount;
                break;
            default:
                Debug.LogError($"Wrong AmmoType({aT}) for Harpoon!");
                break;
        }
    }

    private void Update()
    {
        if ((currentBulletIndex == 0) && currentAmmo <= 0) return;
        else if ((currentBulletIndex == 0) && currentPreciseAmmo <= 0) return;

        if (isShooting >= 0.9f)
        {
            if (Time.time < shootTimer + shootDelay)
            {
                return; // not enough time has passed since last shot
            }

            Vector2 diff = (owner.currentAimDirection).normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            Instantiate(bulletPrefab, barrel.position, Quaternion.Euler(0, 0, rot_z));
            if (currentBulletIndex == 0)
                currentAmmo--;
            else if (currentBulletIndex == 1)
                currentPreciseAmmo--;

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
        if (currentBulletIndex == 0)
            return currentAmmo.ToString();
        else if (currentBulletIndex == 1)
            return currentPreciseAmmo.ToString();

        return "";
    }

    override public DamageType GetBulletType()
    {
        return bullet.GetDamageType();
    }
}
