using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunBase
{
    readonly float TRAIL_LIFE_TIME = 0.1f;

    //[SerializeField] private List<GameObject> bullets;
    //private int currentBulletIndex = 0;

    public float shootDelay = 0.2f;
    private float shootTimer = 0f; // time elapsed since last shot

    //private Bullet bullet;
    [SerializeField] int damagePerBullet;
    int currentAmmo;

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
        Reload();
    }
    public override void Reload()
    {
        currentAmmo = 12;
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
        }

        if (currentAmmo <= 0) return;

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
                trailDisapearTimer = Time.time + TRAIL_LIFE_TIME;
                gunTrail.transform.gameObject.SetActive(true);
                currentRecoilAngle += (Random.Range(0f, 1f) > 0.5f ? -1 : 1) * recoilAnglePerShot;
                trailShown = true;

                if(hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<ITakeDamage>().TakeDamage(damagePerBullet, DamageType.Bullet);
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
            //currentBulletIndex = (currentBulletIndex + 1) % bullets.Count;
            //bulletPrefab = bullets[currentBulletIndex];
            //bullet = bulletPrefab.GetComponent<Bullet>();
        }
    }

    override public string GetAmmoAmount()
    {
        //if (bullet.GetDamageType() == DamageType.Bullet)
        return currentAmmo.ToString();
        //return "";
    }

    override public DamageType GetBulletType()
    {
        return DamageType.Bullet;
        //return bullet.GetDamageType();
    }

    public override string GetBothAmmoString()
    {
        return "";
    }
}
