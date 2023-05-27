using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : GunBase
{

    readonly int MAX_AMMO = 1;

    public float shootDelay = 0.2f;
    private float shootTimer = 0f; // time elapsed since last shot

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int maxPreciseAmmo;
    [SerializeField] int currentPreciseAmmo;

    int fireMode;
    int targetFireMode;

    [SerializeField] Sprite ammoIcon;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Sprite ammoPreciseIcon;
    [SerializeField] GameObject preciseBulletPrefab;
    

    protected override void Start()
    {
        base.Start();
        currentAmmo = MAX_AMMO;
        currentPreciseAmmo = MAX_AMMO;
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

        if (reloading) return;

        if ((fireMode == 0 && currentAmmo <= 0) || (fireMode == 1 && currentPreciseAmmo <= 0))
        {
            if (fireMode == 0 && maxAmmo > 0 || fireMode == 1 && maxPreciseAmmo > 0)
            {
                Reload();
                return;
            }
            else return;
        }

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
        fireMode = targetFireMode;
        if (input)
        {
            StopReload();
            targetFireMode = (fireMode + 1) % 2;
            Reload(true);
            //bulletPrefab = bullets[currentBulletIndex];
            //bullet = bulletPrefab.GetComponent<Bullet>();
        }
    }

    override public string GetAmmoAmount()
    {
        if (fireMode == 0)
            return currentAmmo.ToString() + "/" + maxAmmo.ToString();
        else if (fireMode == 1)
            return currentPreciseAmmo.ToString() + "/" + maxPreciseAmmo.ToString();

        return "";
    }

    public override string GetBothAmmoString()
    {
        return (currentAmmo + maxAmmo) + " " + (currentPreciseAmmo + maxPreciseAmmo);
    }

    public override void RefillAmmo()
    {
        if (fireMode == 0)
        {
            maxAmmo += currentAmmo;
            currentAmmo = Mathf.Min(MAX_AMMO, maxAmmo);
            maxAmmo -= currentAmmo;
        }
        else if (fireMode == 1)
        {
            maxPreciseAmmo += currentPreciseAmmo;
            currentPreciseAmmo = Mathf.Min(MAX_AMMO, maxPreciseAmmo);
            maxPreciseAmmo -= currentPreciseAmmo;
        }
    }

    private void OnDisable()
    {
        StopReload();
    }
    private void OnEnable()
    {
        targetFireMode = fireMode;
    }

    protected override bool IsFullOnAmmo()
    {
        if (fireMode == 0) return currentAmmo == MAX_AMMO;
        else return currentPreciseAmmo == MAX_AMMO;
    }
    public override Sprite GetAmmoIcon()
    {
        if (fireMode == 0) return ammoIcon;
        else return ammoPreciseIcon;
    }
}
