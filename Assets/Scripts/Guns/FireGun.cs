using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : GunBase
{

    [SerializeField] float fireRate = 0.1f;
    private float shootTimer = 0f; // time elapsed since last shot

    [SerializeField] ParticleSystem fireVFX;
    
    public override void Reload()
    {
        currentAmmo = maxAmmo;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && isShooting >= 0.9f)
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

        if(isShooting >= 0.9f)
        {
            ParticleSystem firePS = Instantiate(fireVFX, transform.position, Quaternion.identity);
        }
            
    }

}