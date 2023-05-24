using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutomaticRiffle : GunBase
{
    readonly float TRAIL_LIFE_TIME = 0.1f;

    //[SerializeField] private List<GameObject> bullets;
    //private int currentBulletIndex = 0;

    [SerializeField] float fireRate = 0.1f;
    private float shootTimer = 0f; // time elapsed since last shot
    //private Bullet bullet;

    [SerializeField] float damagePerBullet;
    [SerializeField] int maxAmmo;
    [SerializeField] float damagePerDisintegratingBullet;
    [SerializeField] int maxDisintegratingAmmo;
    int currentAmmo;
    int currentDisintegratingAmmo;

    [SerializeField] LineRenderer gunTrail;
    [SerializeField] LayerMask toIgnore;
    float trailDisapearTimer;
    bool trailShown;
    int fireMode;


    protected override void Start()
    {
        base.Start();
        //bulletPrefab = bullets[currentBulletIndex];
        //bullet = bulletPrefab.GetComponent<Bullet>();
        currentAmmo = 0;
        currentDisintegratingAmmo = 0;
        Reload();
    }

    public override void Reload()
    {
        if (fireMode == 0)
        {
            maxAmmo += currentAmmo;
            currentAmmo = Mathf.Min(30, maxAmmo);
            maxAmmo -= currentAmmo;
        }
        else if (fireMode == 1)
        {
            maxDisintegratingAmmo += currentDisintegratingAmmo;
            currentDisintegratingAmmo = Mathf.Min(30, maxDisintegratingAmmo);
            maxDisintegratingAmmo -= currentDisintegratingAmmo;
        }
    }

    public override void AddAmmo(AmmoType aT, int amount)
    {
        switch (aT)
        {
            case AmmoType.Bullet:
                maxAmmo += amount;
                break;
            case AmmoType.Disintegrating:
                maxDisintegratingAmmo += amount;
                break;
            default:
                Debug.LogError($"Wrong AmmoType({aT}) for ARiffle!");
                break;
        }
    }

    //void Update()
    //{
    //    if (bullet.GetDamageType() == DamageType.Bullet && currentAmmo <= 0) return;
    //    else if (bullet.GetDamageType() == DamageType.Disintegrating && currentDisintegratingAmmo <= 0) return;

    //    if (isShooting)
    //    {
    //        if (Time.time < shootTimer + fireRate)
    //        {
    //            return; // not enough time has passed since last shot
    //        }

    //        Vector2 diff = (owner.currentAimDirection).normalized;
    //        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
    //        Instantiate(bulletPrefab, barrel.position, Quaternion.Euler(0, 0, rot_z));

    //        if (bullet.GetDamageType() == DamageType.Bullet)
    //            currentAmmo--;
    //        else if (bullet.GetDamageType() == DamageType.Disintegrating)
    //            currentDisintegratingAmmo--;

    //        shootTimer = Time.time; // reset timer to current time
    //    }
    //}


    private void Update()
    {
        base.Update();


        if (trailShown && trailDisapearTimer <= Time.time)
        {
            gunTrail.transform.gameObject.SetActive(false);
            trailShown = false;
        }

        if ((fireMode == 0 && currentAmmo <= 0) || (fireMode == 1 && currentDisintegratingAmmo <= 0)) return;

        if(isShooting)
        {
            if (Time.time < shootTimer + fireRate)
            {
                return; // not enough time has passed since last shot
            }

            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(barrel.transform.position, aimVectorWithRecoil, 100, ~toIgnore))
            {
                gunTrail.SetPosition(0, barrel.position);
                gunTrail.SetPosition(1, hit.point);
                trailDisapearTimer = Time.time + TRAIL_LIFE_TIME;
                gunTrail.transform.gameObject.SetActive(true);
                currentRecoilAngle += (Random.Range(0f,1f) > 0.5f ? -1 : 1) * recoilAnglePerShot;
                trailShown = true;

                if (hit.transform.tag == "Enemy")
                {
                    if(fireMode == 0)
                    {
                        hit.transform.GetComponent<ITakeDamage>().TakeDamage(damagePerBullet, DamageType.Bullet);
                    }
                    else
                    {
                        hit.transform.GetComponent<ITakeDamage>().TakeDamage(damagePerDisintegratingBullet, DamageType.Disintegrating);
                    }
                }
            }

            if (fireMode == 0) currentAmmo--;
            else currentDisintegratingAmmo--;

            shootTimer = Time.time; // reset timer to current time
        }
    }

    override public void ChangeBulletType(bool input)
    {
        if (input )
        {
            fireMode = (fireMode + 1) % 2;
            //currentBulletIndex = (currentBulletIndex + 1) % bullets.Count;
            //bulletPrefab = bullets[currentBulletIndex];
            //bullet = bulletPrefab.GetComponent<Bullet>();
        }
    }

    override public string GetAmmoAmount()
    {
        if (fireMode == 0)
            return currentAmmo.ToString() + "/" + maxAmmo.ToString();
        else if (fireMode == 1)
            return currentDisintegratingAmmo.ToString() + "/" + maxDisintegratingAmmo.ToString();

        return "";
    }

    override public DamageType GetBulletType()
    {
        if(fireMode == 0) return DamageType.Bullet;
        else return DamageType.Disintegrating;
    }

    public override string GetBothAmmoString()
    {
        return currentAmmo + maxAmmo + " " + currentDisintegratingAmmo + maxDisintegratingAmmo;
    }
}
