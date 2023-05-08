using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage 
{
    public float TakeDamage(float damage, DamageType type);
    public void TakeArmorDamage(DamageType type, float damage);
    public bool IsImmune();
}
