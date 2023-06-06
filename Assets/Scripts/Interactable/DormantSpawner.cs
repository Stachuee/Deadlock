using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DormantSpawner : Spawner, ITakeDamage
{
    bool active;

    [SerializeField] float maxHp;
    float hp;

    bool poisoned = false;
    float poisonStop;
    float nextPoisonTick;
    bool onFire = false;
    float fireStop;
    float nextFireTick;
    bool frozen = false;
    float freezeStop;
    float nextFreezeTick;

    [SerializeField]
    ParticleSystem onFireParticle;

    Animator animator;

    bool Active
    {
        get 
        { 
            return active; 
        }
        set 
        { 
            active = value;
            animator.SetBool("Active", value);
        }
    }


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (poisoned)
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

    public override void GetNewWave()
    {
        WaveSO temp = SpawnerController.instance.SpawnSideWave();
        if (temp != null) AddToSpawn(temp.GetEnemySpawn(), temp.GetNextWaveDelay());
        else nextWave = Time.time + PACING_LOCK;
    }

    public void Activate()
    {
        hp = maxHp;
        Active = true;
    }

    public void ApplyStatus(Status toApply)
    {
        switch (toApply)
        {
            case Status.Poison:
                poisoned = true;
                poisonStop = Time.time + CombatController.BASE_EFFECT_DURATION;
                break;
            case Status.Freeze:
                freezeStop = Time.time + CombatController.BASE_EFFECT_DURATION;
                frozen = true;
                break;
            case Status.Fire:
                if (!onFire)
                {
                    onFireParticle.Play();
                }
                onFire = true;
                fireStop = Time.time + CombatController.BASE_EFFECT_DURATION;
                break;
        }
    }

    public float GetArmor()
    {
        return 0;
    }

    public float Heal(float ammount)
    {
        return 0;
    }

    public bool IsImmune()
    {
        return !Active;
    }

    public void TakeArmorDamage(float damage)
    {

    }

    public float TakeDamage(float damage, DamageEffetcts effects = DamageEffetcts.None, float armor_piercing = 0)
    {
        hp -= damage;
        return damage;
    }
}
