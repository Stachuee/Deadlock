using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DamageEffetcts { None, Disintegrating }
public interface ITakeDamage 
{
    public float TakeDamage(float damage, DamageEffetcts effects = DamageEffetcts.None);
    public void ApplyStatus(Status toApply);
    public void TakeArmorDamage(float damage);
    public bool IsImmune();
    public float GetArmor();
}
