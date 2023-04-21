using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemy, ITakeDamage
{
    // Fields
    protected float moveSpeed = 3f;
    protected float viewDistance = 5f;
    protected float maxHp;
    protected float currentHp;



    // Methods
    public abstract void Move(Vector3 targetPosition);
    //public abstract void Attack();

    public float TakeDamage(float damage, DamageType type)
    {

        switch (type)
        {
            case DamageType.Bullet:
                Damage(damage);
                break;
            case DamageType.Poison:
                StartCoroutine(PoisonDamage(3, damage));
                break;
            case DamageType.Fire:
                Damage(damage * 2);
                break;
        }

        if (currentHp <= 0) Die();

        return damage;
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

            if (directionToTarget.magnitude < viewDistance /*&& Vector3.Dot(transform.right, directionToTarget) > 0*/)
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


    void Damage(float damage)
    {
        currentHp -= damage;
    }


    private IEnumerator PoisonDamage(int poisonStrenght, float damage)
    {
        Damage(damage);
        while (poisonStrenght > 0)
        {
            yield return new WaitForSeconds(1f);
            Damage(damage);
            poisonStrenght--;
        }
    }


}
