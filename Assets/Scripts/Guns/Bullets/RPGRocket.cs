using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGRocket : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] LayerMask maskToIgnore;
    [SerializeField] float explosionRadius; // Distance from impact point where enemies take damage

    [SerializeField] ParticleSystem explosionVFX;
    Vector2 prevPos;

    [SerializeField]
    float timeToDespawn;
    float timeToDespawnRemain;

    [SerializeField] DamageType damageType;

    [SerializeField] bool isProximity = false;

    private void Start()
    {
        prevPos = transform.position;
        timeToDespawnRemain = timeToDespawn + Time.time;
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Linecast(prevPos, transform.position, ~maskToIgnore);

        if (hit.collider != null || timeToDespawnRemain < Time.time)
        {
            if (isProximity)
            {
                speed = 0;
                return;
            }
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
                        target.TakeDamage(damage, damageType);
                        target.TakeArmorDamage(DamageType.Bullet, 0.1f);
                        target.TakeArmorDamage(DamageType.Ice, 0.1f);
                        target.TakeArmorDamage(DamageType.Fire, 0.1f);
                        target.TakeArmorDamage(DamageType.Mele, 0.1f);
                    }
                }
             }
            
             Destroy(gameObject);
         }
         prevPos = transform.position;
        
        }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && isProximity)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            ITakeDamage target = collider.GetComponent<ITakeDamage>();
            if (target != null)
            {
                target.TakeDamage(damage, damageType);
                target.TakeArmorDamage(DamageType.Bullet, 0.1f);
            }
            Destroy(gameObject);
        }
    }

    public DamageType GetDamageType()
    {
        return damageType;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to show the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}