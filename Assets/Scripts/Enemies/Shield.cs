using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, ITakeDamage
{
    [SerializeField] float hp;
    [SerializeField] float maxHp;

    [SerializeField] float armor = 0;


    private void Awake()
    {
        hp = maxHp;
    }

    public void TakeArmorDamage(float damage)
    {
         armor -= damage;
    }

    public float TakeDamage(float damage, DamageEffetcts effects = DamageEffetcts.None, float armor_piercing = 0)
    {
        float damageTaken = damage * ( 1 - (armor - armor_piercing));
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

    public float GetArmor()
    {
        return armor;
    }
    public float Heal(float ammount)
    {
        hp = Mathf.Min(ammount + hp, maxHp);
        return ammount;
    }
}
