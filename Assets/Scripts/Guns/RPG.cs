using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : GunBase
{

    //[SerializeField] private List<GameObject> bullets;
    //private int currentBulletIndex = 0;

    public float shootDelay = 0.5f;
    private float shootTimer = 0f; // time elapsed since last shot

    //private RPGRocket rocket;

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int currentProximityAmmo;

    [SerializeField] float lunchStrength;

    [SerializeField] GameObject granade;
    [SerializeField] GameObject proximityGranade;

    int fireMode;

    protected override void Start()
    {
        base.Start();
        //bulletPrefab = bullets[currentBulletIndex];
        //rocket = bulletPrefab.GetComponent<RPGRocket>();
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

        if (fireMode == 1 && currentProximityAmmo <= 0) return;
        else if  (fireMode == 0 && currentAmmo <= 0) return;

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

    override public void ChangeBulletType(bool input)
    {
        if (input)
        {
            fireMode = (fireMode + 1) % 2;
            //bulletPrefab = bullets[currentBulletIndex];
            //rocket = bulletPrefab.GetComponent<RPGRocket>();
        }
    }

    override public string GetAmmoAmount()
    {
        if (fireMode == 0)
            return currentAmmo.ToString();
        else if (fireMode == 1)
            return currentProximityAmmo.ToString();
        return "";
    }

    override public DamageType GetBulletType()
    {
        return fireMode == 0 ? DamageType.Bullet : DamageType.Bullet;
    }

    public override string GetBothAmmoString()
    {
        return currentAmmo + " " + currentProximityAmmo;
    }
}
