using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, ITakeDamage
{
    [SerializeField] float hp;
    [SerializeField] float maxHp;

    [SerializeField] float resistance = 0;


    private void Awake()
    {
        hp = maxHp;
    }

    public void TakeArmorDamage(DamageType type, float damage)
    {
        if (type == DamageType.Bullet) resistance -= damage;
    }

    public float TakeDamage(float damage, DamageType type)
    {
        float damageTaken = damage * (-resistance + 1);
        hp -= damageTaken;
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
        return damageTaken;
    }

    public bool IsImmune()
    {
        return false;
    }

    public void ApplyStatus(Status toApply)
    {
    }
}
