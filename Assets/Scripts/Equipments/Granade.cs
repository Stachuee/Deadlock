using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{

    //[SerializeField] float fuseDuration;
    //float explosionTimer;

    //[SerializeField] float damage = 20f;
    //[SerializeField] float explosionRadius;

    //[SerializeField] ParticleSystem explosionVFX;

    //[SerializeField] GameObject inventorySlotPrefab;

    //private void Start()
    //{
    //    explosionTimer = fuseDuration + Time.time;    
    //}

    //private void Update()
    //{
    //    if(explosionTimer < Time.time)
    //    {
    //        Explode();
    //    }
    //}

    //private void Explode()
    //{
    //    Debug.Log("Boom");
    //    Instantiate(explosionVFX, transform.position, Quaternion.identity);
    //    /*Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
    //    foreach (Collider2D collider in colliders)
    //    {
    //        if (collider.CompareTag("Enemy"))
    //        {
    //            ITakeDamage target = collider.GetComponent<ITakeDamage>();
    //            if (target != null) target.TakeDamage(damage, damageType);
    //        }
    //    }*/
    //    Destroy(gameObject);
    //}

    //public GameObject GetInventorySlotPrefab()
    //{
    //    return inventorySlotPrefab;
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    // Draw a wire sphere to show the explosion radius in the editor
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, explosionRadius);
    //}
}
