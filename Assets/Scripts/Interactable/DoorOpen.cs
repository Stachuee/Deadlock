using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour, ITakeDamage
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

    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        return parrent.TakeDamage(damage);
    }

    public void TakeArmorDamage(float damage)
    {

    }

    public void ApplyStatus(Status toApply)
    {

    }

    public bool IsArmored()
    {
        return false;
    }
    public float Heal(float ammount)
    {
        return parrent.Heal(ammount);
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
