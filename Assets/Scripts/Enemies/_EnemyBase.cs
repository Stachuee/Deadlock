using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _EnemyBase : MonoBehaviour, ITakeDamage
{
    public readonly float POISON_DAMAGE_PER_TICK = 10f;
    public readonly float FIRE_DAMAGE_PER_TICK = 10f;
    public readonly float FREEZE_DAMAGE_PER_TICK = 10f;
    public readonly float FREEZE_BASE_STRENGTH = 0.5f;

    public readonly float BASE_EFFECT_DURATION = 5f;
    public readonly float BASE_EFFECT_TICK = 0.5f;

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
    DamageTypeResistance resistances;


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

    protected virtual void Start()
    {
        hp = maxHp;
        baseSpeed = Random.Range(randomSpeed.x, randomSpeed.y);
        speed = baseSpeed;
    }

    protected virtual void Update()
    {
        if(poisoned)
        {
            if (poisonStop < Time.time) poisoned = false;
            if (nextPoisonTick < Time.time) 
            {
                TakeDamage(POISON_DAMAGE_PER_TICK, DamageType.Poison);
                nextPoisonTick = Time.time + BASE_EFFECT_TICK;
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
                TakeDamage(FREEZE_DAMAGE_PER_TICK, DamageType.Ice);
                nextFreezeTick = Time.time + BASE_EFFECT_TICK;
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
                TakeDamage(FIRE_DAMAGE_PER_TICK, DamageType.Fire);
                nextFireTick = Time.time + BASE_EFFECT_TICK;
            }
        }
    }

    public virtual float TakeDamage(float damage, DamageType type)
    {
        float damageTaken = damage;
        switch (type)
        {
            case DamageType.Bullet:
                damageTaken = (1 - resistances.GetResistance(type)) * damage;
                break;
            case DamageType.Poison:
                //if (poisoned) break;
                damageTaken = (1 - resistances.GetResistance(type)) * damage;
                //StartCoroutine(PoisonDamage(3, damageTaken));
                break;
            case DamageType.Fire:
                //if (onFire) break;
                damageTaken = (1 - resistances.GetResistance(type)) * damage;
                //StartCoroutine(FireDamage(3, damageTaken));
                break;
            case DamageType.Ice:
                //if (frozen) break;
                damageTaken = (1 - resistances.GetResistance(type)) * damage;
                //StartCoroutine(Freeze(3, damageTaken));
                break;
            case DamageType.Mele:
                damageTaken = (1 - resistances.GetResistance(type)) * damage;
                break;
            case DamageType.Disintegrating:
                float dResistance = resistances.GetResistance(DamageType.Bullet);
                if (dResistance > 0.5f)
                {
                    damageTaken = damage * (1 - (dResistance - 0.5f));
                }
                else
                {
                    damageTaken = damage;
                }
                break;
            default:
                Debug.LogError($"Invalid DamageType: {type} for Enemy");
                break;
        }
        hp -= damageTaken;

        if (hp <= 0) Dead();
        return damageTaken;
    }

    public void ApplyStatus(Status toApply)
    {
        switch(toApply)
        {
            case Status.Poison:
                poisoned = true;
                poisonStop = Time.time + BASE_EFFECT_DURATION * (1 - resistances.GetResistance(DamageType.Poison));
                break;
            case Status.Freeze:
                freezeStop = Time.time + BASE_EFFECT_DURATION * (1 - resistances.GetResistance(DamageType.Ice));
                if (!frozen)
                {
                    speed *= 1 - FREEZE_BASE_STRENGTH;
                }
                frozen = true;
                break;
            case Status.Fire:
                if(!onFire)
                {
                    onFireParticle.Play();
                }
                onFire = true;
                fireStop = Time.time + BASE_EFFECT_DURATION * (1 - resistances.GetResistance(DamageType.Fire));
                break;
        }
    }

    public void TakeArmorDamage(DamageType type, float damage)
    {
        resistances.SetResistance(type, resistances.GetResistance(type) - damage);
    }

    //private void OnParticleCollision(GameObject other)
    //{
    //    FireGunDamageType damageType = other.GetComponent<FireGunDamageType>();
    //    TakeDamage(10f, damageType.GetDamageType());
    //}

    public virtual void Dead()
    {
        SpawnerController.instance.RemoveFromMap(transform);
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


}
