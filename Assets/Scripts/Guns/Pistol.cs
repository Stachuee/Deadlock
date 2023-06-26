using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunBase
{
    readonly float TRAIL_LIFE_TIME = 0.05f;



    //[SerializeField] private List<GameObject> bullets;
    //private int currentBulletIndex = 0;


    public static int MAX_AMMO = 12;
    public static float shootDelay = 0.2f;
    private float shootTimer = 0f; // time elapsed since last shot

    [SerializeField] Sprite ammoIcon;
    //private Bullet bullet;
    [SerializeField] int damagePerBullet;
    
    int currentAmmo;

    [SerializeField] LineRenderer gunTrail;
    [SerializeField] LayerMask toIgnore;
    float trailDisapearTimer;
    bool trailShown;


    [SerializeField] Transform bulletImpact;
    [SerializeField] ParticleSystem bulletImpactVFXWall;
    [SerializeField] ParticleSystem bulletImpactVFXFlesh;

    protected override void Start()
    {
        base.Start();
        //bulletPrefab = bullets[currentBulletIndex];
        //bullet = bulletPrefab.GetComponent<Bullet>();
        currentAmmo = MAX_AMMO;
        Reload();
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

    override protected void Update()
    {
        base.Update();

        if (trailShown && trailDisapearTimer <= Time.time)
        {
            gunTrail.transform.gameObject.SetActive(false);
            trailShown = false;
            barrelFlash.SetActive(false);
        }

        if (reloading) return;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        } 
        if (isShooting )
        {
            if (Time.time < shootTimer + shootDelay)
            {
                return; // not enough time has passed since last shot
            }

            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(barrel.transform.position, aimVectorWithRecoil, 100, ~toIgnore))
            {
                gunTrail.SetPosition(0, barrel.position);
                gunTrail.SetPosition(1, hit.point);

                bulletImpact.position = hit.point;
                bulletImpact.rotation = Quaternion.Euler(0, 0, rot_z - 180);

                trailDisapearTimer = Time.time + TRAIL_LIFE_TIME;
                gunTrail.transform.gameObject.SetActive(true);
                currentRecoilAngle += (Random.Range(0f, 1f) > 0.5f ? -1 : 1) * recoilAnglePerShot;
                trailShown = true;
                barrelFlash.SetActive(true);
                shotAudio.Play();

                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<ITakeDamage>().TakeDamage(damagePerBullet, DamageSource.Player);
                    bulletImpactVFXFlesh.Play();
                }
                else
                {
                    bulletImpactVFXWall.Play();
                }
            }

            currentAmmo--;

            shootTimer = Time.time; // reset timer to current time
        }
    }

    override public void ChangeBulletType(bool input)
    {
        if (input)
        {

        }
    }

    override public string GetAmmoAmount()
    {
        //if (bullet.GetDamageType() == DamageType.Bullet)
        return currentAmmo.ToString();
        //return "";
    }

    public override string GetBothAmmoString()
    {
        return "";
    }


    public override void RefillAmmo()
    {
        base.RefillAmmo();
        currentAmmo = MAX_AMMO;
    }

    protected override bool IsFullOnAmmo()
    {
        return MAX_AMMO == currentAmmo;
    }

    public override Sprite GetAmmoIcon()
    {
        return ammoIcon;
    }
}
