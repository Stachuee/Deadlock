using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{

    [SerializeField] float fuseDuration;
    float explosionTimer;

    [SerializeField] float damage = 20f;
    [SerializeField] float explosionRadius;

    private void Start()
    {
        explosionTimer = fuseDuration + Time.time;    
    }

    private void Update()
    {
        if(explosionTimer < Time.time)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Debug.Log("Boom");
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
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to show the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
