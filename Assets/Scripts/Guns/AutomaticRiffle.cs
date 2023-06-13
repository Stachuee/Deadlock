using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutomaticRiffle : GunBase
{
    readonly float TRAIL_LIFE_TIME = 0.05f;
    readonly int MAX_AMMO = 30;


    //[SerializeField] private List<GameObject> bullets;
    //private int currentBulletIndex = 0;

    [SerializeField] float fireRate = 0.1f;
    private float shootTimer = 0f; // time elapsed since last shot
                                   //private Bullet bullet;

    public float aromrPierce = 0;

    [SerializeField] Sprite ammoIcon;
    [SerializeField] float damagePerBullet;
    [SerializeField] int maxAmmo;
    [SerializeField] Sprite ammoIconDesintegrating;
    [SerializeField] float damagePerDisintegratingBullet;
    [SerializeField] int maxDisintegratingAmmo;
    int currentAmmo;
    int currentDisintegratingAmmo;

    [SerializeField] LineRenderer gunTrail;
    [SerializeField] LayerMask toIgnore;

    [SerializeField] Transform bulletImpact;
    [SerializeField] ParticleSystem bulletImpactVFXWall;
    [SerializeField] ParticleSystem bulletImpactVFXFlesh;
    float trailDisapearTimer;
    bool trailShown;


    protected override void Start()
    {
        base.Start();
        //bulletPrefab = bullets[currentBulletIndex];
        //bullet = bulletPrefab.GetComponent<Bullet>();
        currentAmmo = MAX_AMMO;
        currentDisintegratingAmmo = MAX_AMMO;
        Reload();
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


    protected override void Update()
    {
        base.Update();


        if (trailShown && trailDisapearTimer <= Time.time)
        {
            gunTrail.transform.gameObject.SetActive(false);
            trailShown = false;
            barrelFlash.SetActive(false);
        }

        if (reloading) return;

        if ((fireMode == 0 && currentAmmo <= 0) || (fireMode == 1 && currentDisintegratingAmmo <= 0))
        {
            if (fireMode == 0 && maxAmmo > 0 || fireMode == 1 && maxDisintegratingAmmo > 0)
            {
                Reload();
                return;
            }
            else return;
        }

        if (isShooting)
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

                bulletImpact.position = hit.point;
                bulletImpact.rotation = Quaternion.Euler(0,0, rot_z - 180);
                

                trailDisapearTimer = Time.time + TRAIL_LIFE_TIME;
                gunTrail.transform.gameObject.SetActive(true);
                currentRecoilAngle += (Random.Range(0f,1f) > 0.5f ? -1 : 1) * recoilAnglePerShot;
                trailShown = true;
                barrelFlash.SetActive(true);
                shotAudio.Play();

                if (hit.transform.tag == "Enemy")
                {
                    if(fireMode == 0)
                    {
                        hit.transform.GetComponent<ITakeDamage>().TakeDamage(damagePerBullet, DamageSource.Player);
                    }
                    else
                    {
                        hit.transform.GetComponent<ITakeDamage>().TakeDamage(damagePerDisintegratingBullet, DamageSource.Player, DamageEffetcts.Disintegrating);
                    }
                    bulletImpactVFXFlesh.Play();
                }
                else
                {
                    bulletImpactVFXWall.Play();
                }
            }

            if (fireMode == 0) currentAmmo--;
            else currentDisintegratingAmmo--;

            shootTimer = Time.time; // reset timer to current time
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

    //override public DamageType GetBulletType()
    //{
    //    if(fireMode == 0) return DamageType.Bullet;
    //    else return DamageType.Disintegrating;
    //}

    public override string GetBothAmmoString()
    {
        return currentAmmo + maxAmmo + " " + currentDisintegratingAmmo + maxDisintegratingAmmo;
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
            maxDisintegratingAmmo += currentDisintegratingAmmo;
            currentDisintegratingAmmo = Mathf.Min(MAX_AMMO, maxDisintegratingAmmo);
            maxDisintegratingAmmo -= currentDisintegratingAmmo;
        }
    }

    protected override bool IsFullOnAmmo()
    {
        if (fireMode == 0) return currentAmmo == MAX_AMMO;
        else return currentDisintegratingAmmo == MAX_AMMO;
    }

    public override Sprite GetAmmoIcon()
    {
        if (fireMode == 0) return ammoIcon;
        else return ammoIconDesintegrating;
    }
}
