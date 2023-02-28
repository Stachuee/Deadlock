using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : EnemyBase
{
    [SerializeField] private Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private Transform currentWaypoint;

    [SerializeField] int enemyMaxHp;

    private void Start()
    {
        currentWaypoint = waypoints[currentWaypointIndex];
        maxHp = enemyMaxHp;
        currentHp = maxHp;
    }

    private void Update()
    {
        if (base.CanSeeTarget())
        {
            Transform target = base.FindNearestTarget();
            Move(target.position);
        }
        else
        {
            Move(currentWaypoint.position);
            if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                currentWaypoint = waypoints[currentWaypointIndex];
            }
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
