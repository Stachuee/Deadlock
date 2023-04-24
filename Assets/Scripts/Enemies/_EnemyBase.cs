using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _EnemyBase : MonoBehaviour, ITakeDamage
{
    [SerializeField]
    protected float maxHp;
    protected float hp;
    [SerializeField, MinMaxSlider(0, 10)]
    protected Vector2 randomSpeed;
    protected float speed;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float attackSpeed;
    protected float lastAttack;

    [SerializeField]
    DamageTypeResistance resistances;

    protected virtual void Start()
    {
        hp = maxHp;
        speed = Random.Range(randomSpeed.x, randomSpeed.y);
    }

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
