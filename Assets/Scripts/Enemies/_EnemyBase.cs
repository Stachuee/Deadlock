using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _EnemyBase : MonoBehaviour, ITakeDamage
{
    readonly float MAP_UPDATE_TICK = 0.1f;

    [SerializeField]
    protected float maxHp;
    [SerializeField] protected float hp;
    [SerializeField, MinMaxSlider(0, 10)]
    protected Vector2 randomSpeed;
    protected float baseSpeed;
    protected float speed;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float attackSpeed;
    protected float lastAttack;

    [SerializeField]
    float armor;


    [SerializeField]
    ParticleSystem onFireParticle;


    bool poisoned = false;
    float poisonStop;
    float nextPoisonTick;
    bool onFire = false;
    float fireStop;
    float nextFireTick;
    bool frozen = false;
    float freezeStop;
    float nextFreezeTick;

    [SerializeField] Marker markerType;
    RectTransform myMarker;

    protected virtual void Start()
    {
        hp = maxHp;
        baseSpeed = Random.Range(randomSpeed.x, randomSpeed.y);
        speed = baseSpeed;
        if(ComputerUI.scientistComputer != null) myMarker = ComputerUI.scientistComputer.CreateMarker(markerType);

        StartCoroutine("UpdateMarker");
    }

    IEnumerator UpdateMarker()
    {
        while(true)
        {
            ComputerUI.scientistComputer.UpdateMarker(transform.position, myMarker);
            yield return new WaitForSeconds(MAP_UPDATE_TICK);
        }
    }

    protected virtual void Update()
    {
        if(poisoned)
        {
            if (poisonStop < Time.time) poisoned = false;
            if (nextPoisonTick < Time.time) 
            {
                TakeDamage(CombatController.POISON_DAMAGE_PER_TICK);
                nextPoisonTick = Time.time + CombatController.BASE_EFFECT_TICK;
            }
        }
        if (frozen)
        {
            if (freezeStop < Time.time)
            {
                speed = baseSpeed;
                frozen = false;
            }
            if (nextFreezeTick < Time.time)
            {
                TakeDamage(CombatController.FREEZE_DAMAGE_PER_TICK);
                nextFreezeTick = Time.time + CombatController.BASE_EFFECT_TICK;
            }
        }
        if (onFire)
        {
            if (fireStop < Time.time)
            {
                onFireParticle.Stop();
                onFire = false;
            }
            if (nextFireTick < Time.time)
            {
                TakeDamage(CombatController.FIRE_DAMAGE_PER_TICK);
                nextFireTick = Time.time + CombatController.BASE_EFFECT_TICK;
            }
        }
    }

    public virtual float TakeDamage(float damage, DamageEffetcts effects = DamageEffetcts.None)
    {

        float damageTaken = damage;

        switch (effects)
        {
            case DamageEffetcts.None:
                damageTaken = (1 - armor) * damage;
                break;
            case DamageEffetcts.Disintegrating:
                damageTaken = CombatController.DISINTEGRATING_FALLOFF.Evaluate(armor) * damage;
                break;
        }

        hp -= damageTaken;

        if (hp <= 0) Dead();
        return damageTaken;
        //switch (type)
        //{
        //    case DamageType.Bullet:
        //        damageTaken = (1 - resistances.GetResistance(type)) * damage;
        //        break;
        //    case DamageType.Poison:
        //        //if (poisoned) break;
        //        damageTaken = (1 - resistances.GetResistance(type)) * damage;
        //        //StartCoroutine(PoisonDamage(3, damageTaken));
        //        break;
        //    case DamageType.Fire:
        //        //if (onFire) break;
        //        damageTaken = (1 - resistances.GetResistance(type)) * damage;
        //        //StartCoroutine(FireDamage(3, damageTaken));
        //        break;
        //    case DamageType.Ice:
        //        //if (frozen) break;
        //        damageTaken = (1 - resistances.GetResistance(type)) * damage;
        //        //StartCoroutine(Freeze(3, damageTaken));
        //        break;
        //    case DamageType.Mele:
        //        damageTaken = (1 - resistances.GetResistance(type)) * damage;
        //        break;
        //    case DamageType.Disintegrating:
        //        float dResistance = resistances.GetResistance(DamageType.Bullet);
        //        damageTaken = CombatController.DISINTEGRATING_FALLOFF.Evaluate(dResistance) * damage;
        //        //if (dResistance > 0.5f)
        //        //{
        //        //    damageTaken = damage * (1 - (dResistance - 0.5f));
        //        //}
        //        //else
        //        //{
        //        //    damageTaken = damage;
        //        //}
        //        break;
        //    default:
        //        Debug.LogError($"Invalid DamageType: {type} for Enemy");
        //        break;
        //}

    }

    public void ApplyStatus(Status toApply)
    {
        switch(toApply)
        {
            case Status.Poison:
                poisoned = true;
                poisonStop = Time.time + CombatController.BASE_EFFECT_DURATION;
                break;
            case Status.Freeze:
                freezeStop = Time.time + CombatController.BASE_EFFECT_DURATION;
                if (!frozen)
                {
                    speed *= 1 - CombatController.FREEZE_BASE_STRENGTH;
                }
                frozen = true;
                break;
            case Status.Fire:
                if(!onFire)
                {
                    onFireParticle.Play();
                }
                onFire = true;
                fireStop = Time.time + CombatController.BASE_EFFECT_DURATION;
                break;
        }
    }

    public void TakeArmorDamage(float damage)
    {
        armor = Mathf.Clamp01(armor - damage);
    }

    //private void OnParticleCollision(GameObject other)
    //{
    //    FireGunDamageType damageType = other.GetComponent<FireGunDamageType>();
    //    TakeDamage(10f, damageType.GetDamageType());
    //}

    public virtual void Dead()
    {
        SpawnerController.instance.RemoveFromMap(transform);
        ComputerUI.scientistComputer.DeleteMarker(myMarker);
        Destroy(gameObject);
    }

    private IEnumerator PoisonDamage(int poisonStrenght, float damage)
    {
        poisoned = true;

        while (poisonStrenght > 0)
        {
            yield return new WaitForSeconds(1f);
            hp -= damage;
            poisonStrenght--;
        }
        poisoned = false;
    }

    private IEnumerator FireDamage(int fireStrenght, float damage)
    {
        onFire = true;

        while (fireStrenght > 0)
        {
            yield return new WaitForSeconds(1f);
            hp -= damage;
            fireStrenght--;
        }
        onFire = false;
    }

    private IEnumerator Freeze(int iceStrenght, float damage)
    {
        frozen = true;
        float tmpSpeed = speed;
        speed *= damage;
        yield return new WaitForSeconds(iceStrenght);
        frozen = false;
        speed = tmpSpeed;
    }

    public bool IsImmune()
    {
        return false;
    }

    public float GetArmor()
    {
        return armor;
    }

    public float Heal(float ammount)
    {
        hp = Mathf.Min(ammount + hp, maxHp);
        return ammount;
    }
}
