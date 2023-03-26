using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : EnemyBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTip;
    [SerializeField] private float shootingCooldown = 1f;

    private Transform target;
    private float lastShotTime;

    [SerializeField] int enemyMaxHp;

    private void Start()
    {
        target = FindNearestTarget();
        maxHp = enemyMaxHp;
        currentHp = maxHp;
    }

    private void Update()
    {
        if (CanSeeTarget())
        {
            target = FindNearestTarget();
            Vector3 direction = target.position - transform.position;

            if (Time.time - lastShotTime > shootingCooldown)
            {
                Shoot(direction);
                lastShotTime = Time.time;
            }
        }
    }

 

    public override void Move(Vector3 targetPosition)
    {
        // Shooting enemy doesn't move
    }

    private void Shoot(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, gunTip.position, Quaternion.identity);
        bullet.transform.right = direction;
    }
}
