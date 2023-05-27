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

    [SerializeField] AnimationCurve _DISINTEGRATING_FALLOFF;

    public static float POISON_DAMAGE_PER_TICK { get; private set; }
    public static float FIRE_DAMAGE_PER_TICK { get; private set; }
    public static float FREEZE_DAMAGE_PER_TICK { get; private set; }
    public static float FREEZE_BASE_STRENGTH { get; private set; }
    public static float BASE_EFFECT_DURATION { get; private set; }
    public static float BASE_EFFECT_TICK { get; private set; }
    public static AnimationCurve DISINTEGRATING_FALLOFF { get; private set; }

    private void Awake()
    {
        DISINTEGRATING_FALLOFF = _DISINTEGRATING_FALLOFF;

        POISON_DAMAGE_PER_TICK = _POISON_DAMAGE_PER_TICK;
        FIRE_DAMAGE_PER_TICK = _FIRE_DAMAGE_PER_TICK;
        FREEZE_DAMAGE_PER_TICK = _FREEZE_DAMAGE_PER_TICK;
        FREEZE_BASE_STRENGTH = _FREEZE_BASE_STRENGTH;
        BASE_EFFECT_DURATION = _BASE_EFFECT_DURATION;
        BASE_EFFECT_TICK = _BASE_EFFECT_TICK;
    }
}
