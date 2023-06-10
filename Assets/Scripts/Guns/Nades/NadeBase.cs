using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NadeBase : MonoBehaviour
{
    [SerializeField] bool explodeOnContact;
    [SerializeField] bool proximity;
    [SerializeField] float fuse;
    float explosionTime;
    bool armed;
    Vector2 stickedWallNormal;

    [SerializeField] float explosionRadius;
    [SerializeField] float damage;
    [SerializeField] float armorAndResistDamage;

    [SerializeField] float explosionDuration;
    [SerializeField] EffectManager.ScreenShakeRange explosionRange;
    [SerializeField] EffectManager.ScreenShakeStrength explosionStrength;

    [SerializeField] protected GameObject explosionVFX;

    [SerializeField] Rigidbody2D myBody;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!explodeOnContact && proximity)
        {
            myBody.bodyType = RigidbodyType2D.Kinematic;
            myBody.velocity = Vector2.zero;
            armed = true;
            stickedWallNormal = collision.GetContact(0).normal;
        }
        else if (explodeOnContact)
        {
            Explode(true, collision.GetContact(0).normal);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (proximity && armed && collision.transform.tag == "Enemy")
        {
            Explode(false, stickedWallNormal);
        }
    }

    public void Lunch(Vector2 force)
    {
        explosionTime = Time.time + fuse;
        myBody.AddForce(force);
    }

    protected virtual void Update()
    {
        if(explosionTime < Time.time)
        {
            Explode(false, Vector2.up);
        }
    }

    protected virtual void Explode(bool onContact = false, Vector2? normals = null)
    {
        //Destroy(Instantiate(explosionVFX, (Vector2)transform.position, Quaternion.identity), 1);
        // Check for enemies within explosion range
        EffectManager.instance.ScreenShake(explosionDuration, explosionRange, explosionStrength, transform.position);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                ITakeDamage target = collider.GetComponent<ITakeDamage>();
                if (target != null)
                {
                    target.TakeArmorDamage(armorAndResistDamage);
                    target.TakeDamage(damage, DamageSource.Player);
                }
            }
        }
        
        Destroy(gameObject);
    }
}
