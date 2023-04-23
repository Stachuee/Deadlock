using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunDamageType : MonoBehaviour
{
    [SerializeField] DamageType damageType;

    public DamageType GetDamageType()
    {
        return damageType;
    }
}
