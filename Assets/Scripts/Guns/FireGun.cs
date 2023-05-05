using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : GunBase
{
    [SerializeField] private List<ParticleSystem> fireVFXsList;
    private int currentFireIndex = 0;

    [SerializeField] float fireRate = 0.1f;
    private float shootTimer = 0f; // time elapsed since last shot

    ParticleSystem fireVFX;
    FireGunDamageType fireDT;

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int currentIceAmmo;

    ParticleSystem firePS;

    GunController gunController;


    protected override void Start()
    {
        gunController = FindObjectOfType<GunController>();
        base.Start();
        fireVFX = fireVFXsList[currentFireIndex];
        fireDT = fireVFX.GetComponent<FireGunDamageType>();

        firePS = Instantiate(fireVFX, barrel.position, transform.rotation);
        firePS.enableEmission = false;
        gunController.SetEffectToDeactivate(firePS);
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
            case AmmoType.Ice:
                currentIceAmmo += amount;
                break;
            default:
                Debug.LogError($"Wrong AmmoType({aT}) for Firegun!");
                break;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && isShooting >= 0.9f && currentAmmo > 0)
        {
            
            if (Time.time < shootTimer + fireRate)
            {
                return; // not enough time has passed since last shot
            }
            ITakeDamage enemy = collision.GetComponent<ITakeDamage>();
            enemy.TakeDamage(10f, fireDT.GetDamageType());

            shootTimer = Time.time; // reset timer to current time
        }
    }
    void Update()
    {
        Vector2 diff = (owner.currentAimDirection).normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        firePS.transform.rotation = transform.rotation;
        firePS.transform.position = transform.position;

        if (isShooting >= 0.9f && currentAmmo > 0)
        {
            firePS.enableEmission = true;
            if (Time.time < shootTimer + fireRate)
            {
                return; // not enough time has passed since last shot
            }
            if (fireDT.GetDamageType() == DamageType.Bullet)
                currentAmmo--;
            else if (fireDT.GetDamageType() == DamageType.Ice)
                currentIceAmmo--;
        }
        else
        {
            firePS.enableEmission = false;
        }

        if(!gameObject.activeSelf) Destroy(firePS);

    }

    override public void ChangeBulletType(float input)
    {
        if (input >= 1)
        {
            currentFireIndex = (currentFireIndex + 1) % fireVFXsList.Count;
            fireVFX = fireVFXsList[currentFireIndex];
            Destroy(firePS);
            firePS = Instantiate(fireVFX, barrel.position, transform.rotation);
            firePS.enableEmission = false;
            fireDT = fireVFX.GetComponent<FireGunDamageType>();
            gunController.SetEffectToDeactivate(firePS);
        }
    }

    override public int GetAmmoAmount()
    {
        if (fireDT.GetDamageType() == DamageType.Bullet)
            return currentAmmo;
        else if (fireDT.GetDamageType() == DamageType.Ice)
            return currentIceAmmo;

        return 0;
    }

    override public DamageType GetBulletType()
    {
        return fireDT.GetDamageType();
    }

}