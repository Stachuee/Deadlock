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

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int currentFireAmmo;
    [SerializeField] int currentPoisonAmmo;


    protected override void Start()
    {
        base.Start();
        fireVFX = fireVFXsList[currentFireIndex];
    }

    public override void Reload()
    {
        currentAmmo = maxAmmo;
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
            enemy.TakeDamage(10f, DamageType.Bullet);

            shootTimer = Time.time; // reset timer to current time
        }
    }
    void Update()
    {
        Vector2 diff = (owner.currentAimDirection).normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        if(isShooting >= 0.9f && currentAmmo > 0)
        {
            ParticleSystem firePS = Instantiate(fireVFX, barrel.position, transform.rotation);
            if (Time.time < shootTimer + fireRate)
            {
                return; // not enough time has passed since last shot
            }
            currentAmmo -= 5;
        }
            
    }

    override public void ChangeBulletType(float input)
    {
        if (input >= 1)
        {
            
        }
    }

    override public int GetAmmoAmount()
    {
        return currentAmmo;
    }

    override public DamageType GetBulletType()
    {
        return DamageType.Bullet;
    }

}