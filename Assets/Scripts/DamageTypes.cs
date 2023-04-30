using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Bullet, Fire, Poison, Ice, Mele }

[System.Serializable]
public struct DamageTypeResistance
{
    [Range(0, 1f)]
    public float bulletResistance;
    [Range(0, 1f)]
    public float fireResistance;
    [Range(0, 1f)]
    public float posionResistance;
    [Range(0, 1f)]
    public float iceResistance;
    [Range(0, 1f)]
    public float meleResistance;

    public float GetResistance(DamageType type)
    {
        switch (type)
        {
            case DamageType.Bullet:
                return bulletResistance;
            case DamageType.Fire:
                return fireResistance;
            case DamageType.Poison:
                return posionResistance;
            case DamageType.Ice:
                return iceResistance;
            case DamageType.Mele:
                return meleResistance;
        }
        return 1;
    }

    public void SetResistance(float value)
    {
        bulletResistance = value;
    }
}
