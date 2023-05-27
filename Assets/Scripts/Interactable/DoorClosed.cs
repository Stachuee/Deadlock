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

    public float TakeDamage(float damage, DamageEffetcts effects = DamageEffetcts.None)
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
}
