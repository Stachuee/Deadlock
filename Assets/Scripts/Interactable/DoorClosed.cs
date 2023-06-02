using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClosed : MonoBehaviour, ITakeDamage
{
    [SerializeField] DoorScript parrent;

    private void Awake()
    {
        parrent = transform.GetComponentInParent<DoorScript>();
    }

    public bool IsImmune()
    {
        return false;
    }

    public float TakeDamage(float damage, DamageEffetcts effects = DamageEffetcts.None, float armor_piercing = 0)
    {
        return parrent.TakeDamage(damage);
    }

    public void TakeArmorDamage(float damage)
    {

    }

    public void ApplyStatus(Status toApply)
    {
        
    }

    public float GetArmor()
    {
        return 0;
    }
    public float Heal(float ammount)
    {
        return parrent.Heal(ammount);
    }
}
