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

    public float TakeDamage(float damage, DamageType type)
    {
        return parrent.TakeDamage(damage, type);
    }

    public void TakeArmorDamage(DamageType type, float damage)
    {

    }
}
