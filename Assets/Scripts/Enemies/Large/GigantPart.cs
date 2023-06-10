using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GigantPart : MonoBehaviour, ITakeDamage
{
    [SerializeField] _EnemyBase parrent;
    [SerializeField] float armor;
    public void ApplyStatus(Status toApply)
    {
        
    }

    public float GetArmor()
    {
        return armor;
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

    public void TakeArmorDamage(float damage)
    {
        armor = Mathf.Clamp01(armor - damage);
    }

    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        float damageTaken = damage;

        switch (effects)
        {
            case DamageEffetcts.None:
                damageTaken = (1 - (armor)) * damage;
                break;
            case DamageEffetcts.Disintegrating:
                damageTaken = CombatController.DISINTEGRATING_FALLOFF.Evaluate(armor) * damage;
                break;
        }

        parrent.TakeTrueDamage(damageTaken);

        return damageTaken;
    }
}
