using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : GunBase
{
    readonly int MAX_AMMO = 6;
    //[SerializeField] private List<GameObject> bullets;
    //private int currentBulletIndex = 0;

    public float shootDelay = 0.5f;
    private float shootTimer = 0f; // time elapsed since last shot

    //private RPGRocket rocket;

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int maxProximityAmmo;
    [SerializeField] int currentProximityAmmo;

    [SerializeField] float lunchStrength;

    [SerializeField] Sprite ammoIcon;
    [SerializeField] GameObject granade;
    [SerializeField] Sprite ammoProximityIcon;
    [SerializeField] GameObject proximityGranade;


    protected override void Start()
    {
        base.Start();
        currentAmmo = MAX_AMMO;
        currentProximityAmmo = MAX_AMMO;
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

    protected override void Update()
    {
        base.Update();


        if (reloading) return;

        if ((fireMode == 0 && currentAmmo <= 0) || (fireMode == 1 && currentProximityAmmo <= 0))
        {
            if (fireMode == 0 && maxAmmo > 0 || fireMode == 1 && maxProximityAmmo > 0)
            {
                Reload();
                return;
            }
            else return;
        }

        if (isShooting)
        {
            if (Time.time < shootTimer + shootDelay)
            {
                return; // not enough time has passed since last shot
            }

            NadeBase temp = Instantiate(fireMode == 0 ? granade : proximityGranade, barrel.position, Quaternion.Euler(0, 0, currentBarrelAngle)).GetComponent<NadeBase>();
            temp.Lunch(aimVectorWithRecoil.normalized * lunchStrength);

            if (fireMode == 0)
                currentAmmo--;
            else if (fireMode == 1)
                currentProximityAmmo--;

            shootTimer = Time.time; // reset timer to current time
        }
    }

    override public string GetAmmoAmount()
    {
        if (fireMode == 0)
            return currentAmmo.ToString() + "/" + maxAmmo.ToString();
        else if (fireMode == 1)
            return currentProximityAmmo.ToString() + "/" + maxProximityAmmo.ToString();
        return "";
    }

    //override public DamageType GetBulletType()
    //{
    //    return fireMode == 0 ? DamageType.Bullet : DamageType.Bullet;
    //}

    public override string GetBothAmmoString()
    {
        return (currentAmmo + maxAmmo) + " " + (currentProximityAmmo + maxProximityAmmo);
    }

    public override void RefillAmmo()
    {
        base.RefillAmmo();
        if (fireMode == 0)
        {
            maxAmmo += currentAmmo;
            currentAmmo = Mathf.Min(MAX_AMMO, maxAmmo);
            maxAmmo -= currentAmmo;
        }
        else if (fireMode == 1)
        {
            maxProximityAmmo += currentProximityAmmo;
            currentProximityAmmo = Mathf.Min(MAX_AMMO, maxProximityAmmo);
            maxProximityAmmo -= currentProximityAmmo;
        }
    }

    protected override bool IsFullOnAmmo()
    {
        if (fireMode == 0) return currentAmmo == MAX_AMMO;
        else return currentProximityAmmo == MAX_AMMO;
    }

    public override Sprite GetAmmoIcon()
    {
        if (fireMode == 0) return ammoIcon;
        else return ammoProximityIcon;
    }
}
