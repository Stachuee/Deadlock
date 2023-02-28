using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    // Fields
    protected float moveSpeed = 3f;
    protected float viewDistance = 5f;
    protected int maxHp;
    protected int currentHp;

    // Methods
    public abstract void Move(Vector3 targetPosition);
    //public abstract void Attack();

    public void TakeDamage(int damageAmount)
    {
        currentHp -= damageAmount;
    }

    public void Die()
    {
        Destroy(gameObject);
    }


    protected bool CanSeeTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            Vector3 directionToTarget = player.transform.position - transform.position;

            if (directionToTarget.magnitude < viewDistance && Vector3.Dot(transform.right, directionToTarget) > 0)
            {
                return true;
            }
        }

        return false;
    }

    protected Transform FindNearestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < nearestDistance)
            {
                nearestTarget = player.transform;
                nearestDistance = distance;
            }
        }

        return nearestTarget;
    }
}
