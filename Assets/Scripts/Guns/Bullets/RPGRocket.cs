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
            ParticleSystem boomVFX = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            // Check for enemies within explosion range
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
             foreach (Collider2D collider in colliders)
             {
                if (collider.CompareTag("Enemy"))
                {
                    ITakeDamage target = collider.GetComponent<ITakeDamage>();
                    if (target != null) target.TakeDamage(damage, DamageType.Bullet);
                }
             }
            
             Destroy(gameObject);
             Destroy(boomVFX);
         }
         prevPos = transform.position;
        
        }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to show the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}