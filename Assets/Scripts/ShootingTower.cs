using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTower : MonoBehaviour
{

    readonly float TRAIL_LIFE_TIME = 0.05f;

    [SerializeField] private Transform shootingPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float shootingAreaRadius = 5f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] int damagePerBullet;

    [SerializeField] float LifeTime;
    float lifeRimeRemain;

    [SerializeField] Transform barrel;
    [SerializeField] LineRenderer gunTrail;
    float trailDisapearTimer;
    bool trailShown;

    private float shootTimer = 0f;

    [SerializeField] GameObject inventorySlotPrefab;

    Transform target;

    private void Start()
    {
        lifeRimeRemain = LifeTime;
    }

    void Update()
    {
        if (trailShown && trailDisapearTimer <= Time.time)
        {
            gunTrail.transform.gameObject.SetActive(false);
            trailShown = false;
        }


        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, shootingAreaRadius, enemyLayer);
        target = null;
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
        if (lifeRimeRemain <= 0) Destroy(gameObject);
        lifeRimeRemain -= Time.deltaTime;
    }

    private void AimAt(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - shootingPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shootingPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Shoot()
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(barrel.transform.position, target.position - barrel.position, 100, enemyLayer))
        {
            gunTrail.SetPosition(0, barrel.position);
            gunTrail.SetPosition(1, hit.point);
            trailDisapearTimer = Time.time + TRAIL_LIFE_TIME;
            gunTrail.transform.gameObject.SetActive(true);
            trailShown = true;

            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<ITakeDamage>().TakeDamage(damagePerBullet, DamageSource.Turret);
            }
        }

        shootTimer = Time.time;
    }

    public GameObject GetInventorySlotPrefab()
    {
        return inventorySlotPrefab;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingAreaRadius);
    }
}