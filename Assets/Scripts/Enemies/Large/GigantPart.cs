using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GigantPart : MonoBehaviour, ITakeDamage
{
    [SerializeField] _EnemyBase parrent;
    [SerializeField] bool armored;
    [SerializeField] float armorHp;


    public void ApplyStatus(Status toApply)
    {
        
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public float Heal(float ammount)
    {
        return 0;
    }

    public bool IsImmune()
    {
        return false;
    }
    public bool IsArmored()
    {
        return armored;
    }

    public void TakeArmorDamage(float damage)
    {
        armorHp = armorHp - damage;
        if(armorHp <= 0)
        {
            armored = false;
        }
    }

    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        float damageTaken = damage;

        switch (effects)
        {
            case DamageEffetcts.None:
                damageTaken = (armored ? CombatController.ARMOR_DAMAGE_REDUCTION : 1) * damage;
                break;
            case DamageEffetcts.Disintegrating:
                damageTaken = (armored ? CombatController.DISINTEGRATING_FALLOFF : 1) * damage;
                break;
        }

        parrent.TakeTrueDamage(damageTaken);

        return damageTaken;
    }
}
