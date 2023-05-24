using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : GunBase
{

    public float shootDelay = 0.2f;
    private float shootTimer = 0f; // time elapsed since last shot

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int currentPreciseAmmo;

    int fireMode;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject preciseBulletPrefab;

    protected override void Start()
    {
        base.Start();
        //bulletPrefab = bullets[currentBulletIndex];
        //bullet = bulletPrefab.GetComponent<Bullet>();
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

    protected override void Update()
    {
        base.Update();

        if ((fireMode == 0) && currentAmmo <= 0) return;
        else if ((fireMode == 1) && currentPreciseAmmo <= 0) return;

        if (isShooting )
        {
            if (Time.time < shootTimer + shootDelay)
            {
                return; // not enough time has passed since last shot
            }
            Instantiate(fireMode == 0 ? bulletPrefab : preciseBulletPrefab, barrel.position, Quaternion.Euler(0, 0, rot_z));

            if (fireMode == 0)
                currentAmmo--;
            else if (fireMode == 1)
                currentPreciseAmmo--;

            shootTimer = Time.time; // reset timer to current time
        }
    }

    override public void ChangeBulletType(bool input)
    {
        if (input)
        {
            fireMode = (fireMode + 1) % 2;
            //bulletPrefab = bullets[currentBulletIndex];
            //bullet = bulletPrefab.GetComponent<Bullet>();
        }
    }

    override public string GetAmmoAmount()
    {
        if (fireMode == 0)
            return currentAmmo.ToString();
        else if (fireMode == 1)
            return currentPreciseAmmo.ToString();

        return "";
    }

    override public DamageType GetBulletType()
    {
        return DamageType.Bullet;
    }

    public override string GetBothAmmoString()
    {
        return currentAmmo + " " + currentPreciseAmmo;
    }
}
