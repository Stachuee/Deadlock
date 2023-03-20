using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTowel : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float shootingAreaRadius = 5f;
    [SerializeField] private float fireRate = 0.1f;

    [SerializeField] protected GameObject bulletPrefab;


    private float shootTimer = 0f;



    void Update()
    {
        

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, shootingAreaRadius, enemyLayer);
        Transform target = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D hitCollider in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = hitCollider.transform;
            }
        }

        if (target != null)
        {
            AimAt(target.position);

            if (Time.time >= shootTimer + fireRate)
            {
                Shoot();
            }
        }
    }

    private void AimAt(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - shootingPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shootingPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        shootTimer = Time.time;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingAreaRadius);
    }
}