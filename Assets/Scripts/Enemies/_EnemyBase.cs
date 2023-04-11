using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _EnemyBase : MonoBehaviour, ITakeDamage
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float maxHp;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float attackSpeed;
    protected float lastAttack;

    [SerializeField]
    DamageTypeResistance resistances;


    public float TakeDamage(float damage, DamageType type)
    {
        float damageTaken = (1 - resistances.GetResistance(type)) * damage;
        hp -= damageTaken;

        if (hp <= 0) Dead();
        return damageTaken;
    }

    public virtual void Dead()
    {
        Destroy(gameObject);
    }
}
