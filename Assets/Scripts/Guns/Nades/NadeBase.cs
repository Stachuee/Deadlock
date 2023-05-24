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

    [SerializeField] float explosionRadius;
    [SerializeField] float damage;
    [SerializeField] float armorAndResistDamage;
    [SerializeField] DamageType damageType;

    [SerializeField] GameObject explosionVFX;

    [SerializeField] Rigidbody2D myBody;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!explodeOnContact && proximity)
        {
            myBody.bodyType = RigidbodyType2D.Kinematic;
            myBody.velocity = Vector2.zero;
            armed = true;
        }
        else if (explodeOnContact)
        {
            Explode();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (proximity && armed && collision.transform.tag == "Enemy")
        {
            Explode();
        }
    }

    public void Lunch(Vector2 force)
    {
        explosionTime = Time.time + fuse;
        myBody.AddForce(force);
    }

    private void Update()
    {
        if(explosionTime < Time.time)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        // Check for enemies within explosion range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                ITakeDamage target = collider.GetComponent<ITakeDamage>();
                if (target != null)
                {
                    target.TakeArmorDamage(DamageType.Bullet, armorAndResistDamage);
                    target.TakeArmorDamage(DamageType.Ice, armorAndResistDamage);
                    target.TakeArmorDamage(DamageType.Fire, armorAndResistDamage);
                    target.TakeArmorDamage(DamageType.Mele, armorAndResistDamage);
                    target.TakeDamage(damage, damageType);
                }
            }
        }

        Destroy(gameObject);
    }
}
