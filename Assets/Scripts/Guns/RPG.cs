using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : GunBase
{

    [SerializeField] private List<GameObject> bullets;
    private int currentBulletIndex = 0;

    public float shootDelay = 0.5f;
    private float shootTimer = 0f; // time elapsed since last shot

    private RPGRocket rocket;

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int currentProximityAmmo;

    protected override void Start()
    {
        base.Start();
        bulletPrefab = bullets[currentBulletIndex];
        rocket = bulletPrefab.GetComponent<RPGRocket>();
    }

    public override void Reload()
    {
        currentAmmo = maxAmmo;
    }

    public override void AddAmmo(AmmoType aT, int amount)
    {
        switch (aT)
        {
            case AmmoType.Bullet:
                currentAmmo += amount;
                break;
            case AmmoType.Proximity:
                currentProximityAmmo += amount;
                break;
            default:
                Debug.LogError($"Wrong AmmoType({aT}) for RPG!");
                break;
        }
    }

    private void Update()
    {
        if (currentBulletIndex == 1 && currentProximityAmmo <= 0) return;
        else if  (rocket.GetDamageType() == DamageType.Bullet && currentAmmo <= 0) return;

        if (isShooting >= 0.9f)
        {
            if (Time.time < shootTimer + shootDelay)
            {
                return; // not enough time has passed since last shot
            }

            Vector2 diff = (owner.currentAimDirection).normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            Instantiate(bulletPrefab, barrel.position, Quaternion.Euler(0, 0, rot_z));
            if (currentBulletIndex == 1)
                currentProximityAmmo--;
            else if (rocket.GetDamageType() == DamageType.Bullet)
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
            rocket = bulletPrefab.GetComponent<RPGRocket>();
        }
    }

    override public int GetAmmoAmount()
    {
        if (currentBulletIndex == 1)
            return currentProximityAmmo;
        else if (rocket.GetDamageType() == DamageType.Bullet)
            return currentAmmo;

        return 0;
    }

    override public DamageType GetBulletType()
    {
        return rocket.GetDamageType();
    }
}
