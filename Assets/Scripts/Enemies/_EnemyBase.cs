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
        Debug.Log("dam");
        float damageTaken = (1 - resistances.GetResistance(type)) * damage;
        hp -= damageTaken;

        if (hp <= 0) Dead();
        return damageTaken;
    }

    public void TakeArmorDamage(float damage)
    {
        /*resistances.SetResistance(resistances.GetResistance(DamageType.Bullet) - damage);
        Debug.Log(resistances.GetResistance(DamageType.Bullet));*/
    }

    public virtual void Dead()
    {
        //Destroy(gameObject);
    }
}
