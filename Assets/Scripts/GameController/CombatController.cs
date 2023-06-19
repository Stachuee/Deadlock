using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] float _POISON_DAMAGE_PER_TICK = 10f;
    [SerializeField] float _FIRE_DAMAGE_PER_TICK = 10f;
    [SerializeField] float _FREEZE_DAMAGE_PER_TICK = 10f;
    [SerializeField] float _FREEZE_BASE_STRENGTH = 0.5f;

    [SerializeField] float _BASE_EFFECT_DURATION = 5f;
    [SerializeField] float _BASE_EFFECT_TICK = 0.5f;

    [SerializeField] float _DISINTEGRATING_FALLOFF;

    [SerializeField] float _ARMOR_DAMAGE_REDUCTION = 0.5f;

    [SerializeField] float _PSI_BOOST_DAMAGE_REDUCTION = 0.7f;
    [SerializeField] float _PSI_BOOST_SPEED_INCREASE = 0.1f;

    public static float POISON_DAMAGE_PER_TICK { get; private set; }
    public static float FIRE_DAMAGE_PER_TICK { get; private set; }
    public static float FREEZE_DAMAGE_PER_TICK { get; private set; }
    public static float FREEZE_BASE_STRENGTH { get; private set; }
    public static float BASE_EFFECT_DURATION { get; private set; }
    public static float BASE_EFFECT_TICK { get; private set; }
    public static float DISINTEGRATING_FALLOFF { get; private set; }
    public static float ARMOR_DAMAGE_REDUCTION { get; private set; }
    public static float PSI_BOOST_DAMAGE_REDUCTION { get; private set; }
    public static float PSI_BOOST_SPEED_INCREASE { get; private set; }



    private void Awake()
    {
        DISINTEGRATING_FALLOFF = _DISINTEGRATING_FALLOFF;

        POISON_DAMAGE_PER_TICK = _POISON_DAMAGE_PER_TICK;
        FIRE_DAMAGE_PER_TICK = _FIRE_DAMAGE_PER_TICK;
        FREEZE_DAMAGE_PER_TICK = _FREEZE_DAMAGE_PER_TICK;
        FREEZE_BASE_STRENGTH = _FREEZE_BASE_STRENGTH;
        BASE_EFFECT_DURATION = _BASE_EFFECT_DURATION;
        BASE_EFFECT_TICK = _BASE_EFFECT_TICK;
        ARMOR_DAMAGE_REDUCTION = _ARMOR_DAMAGE_REDUCTION;
        PSI_BOOST_DAMAGE_REDUCTION = _PSI_BOOST_DAMAGE_REDUCTION;
        PSI_BOOST_SPEED_INCREASE = _PSI_BOOST_SPEED_INCREASE;
    }
}
