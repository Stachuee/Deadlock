using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandFollowEnemy : EnemyBase
{
    private Transform target;

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
            Move(target.position);
        }

        if (currentHp <= 0) Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //todo
        }
    }

    public override void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
