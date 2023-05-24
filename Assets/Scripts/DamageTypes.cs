using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Bullet, Fire, Poison, Ice, Mele, Disintegrating }
public enum Status {Fire, Poison, Freeze }

[System.Serializable]
public struct DamageTypeResistance
{
    [Range(0, 1f)]
    public float bulletResistance;
    [Range(0, 1f)]
    public float fireResistance;
    [Range(0, 1f)]
    public float poisonResistance;
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
                return poisonResistance;
            case DamageType.Ice:
                return iceResistance;
            case DamageType.Mele:
                return meleResistance;
        }
        return 1;
    }

    public void SetResistance(DamageType type, float value)
    {
        switch (type)
        {
            case DamageType.Bullet:
                bulletResistance = value;
                if (bulletResistance <= 0) bulletResistance = 0;
                break;
            case DamageType.Fire:
                fireResistance = value;
                if (fireResistance <= 0) fireResistance = 0;
                break;
            case DamageType.Poison:
                poisonResistance = value;
                if (poisonResistance <= 0) poisonResistance = 0;
                break;
            case DamageType.Ice:
                iceResistance = value;
                if (iceResistance <= 0) iceResistance = 0;
                break;
            case DamageType.Mele:
                meleResistance = value;
                if (meleResistance <= 0) meleResistance = 0;
                break;
        }
    }
}
