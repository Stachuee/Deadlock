using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DormantSpawner : Spawner, ITakeDamage
{
    bool active;

    [SerializeField] float maxHp;
    float hp;

    [SerializeField] float pacingToActivate;

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

    [SerializeField] float newSpawnerShakeDuration;
    [SerializeField] EffectManager.ScreenShakeRange newSpawnerShakeRange;
    [SerializeField] EffectManager.ScreenShakeStrength newSpawnerShakeStrength;

    Animator animator;

    Rooms parrent;

    [SerializeField] Marker markerType;
    RectTransform myMarker;
    GameObject myMarkerObject;

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
        parrent = GetComponentInParent<Rooms>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SpawnerController.instance.AddSpawner(this);
        PacingController.pacingController.AddToNotify(this);
        transform.tag = "Enemy";
    }

    protected virtual void Update()
    {
        if (poisoned)
        {
            if (poisonStop < Time.time) poisoned = false;
            if (nextPoisonTick < Time.time)
            {
                TakeDamage(CombatController.POISON_DAMAGE_PER_TICK, DamageSource.Enemy);
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
                TakeDamage(CombatController.FREEZE_DAMAGE_PER_TICK, DamageSource.Enemy);
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
                TakeDamage(CombatController.FIRE_DAMAGE_PER_TICK, DamageSource.Enemy);
                nextFireTick = Time.time + CombatController.BASE_EFFECT_TICK;
            }
        }
        if(isActive)
        {
            currentPacing -= (pacingFalloff / 60) * Time.deltaTime;
        }
    }

    public override void ActivateSpanwer()
    {
        base.ActivateSpanwer();
        StartCoroutine("Spawn");
        //PacingController.pacingController.IncreasePacing(pacingToActivate);
        hp = maxHp;
        Active = true;
        currentPacing = targetPacing;
        parrent.SendWarning(WarningStrength.Medium);
        EffectManager.instance.ScreenShake(newSpawnerShakeDuration, newSpawnerShakeRange, newSpawnerShakeStrength, Vector2.zero);
        SpawnerController.instance.AddEnemyToMap(this, transform);
        if (ComputerUI.scientistComputer != null)
        {
            myMarker = ComputerUI.scientistComputer.CreateMarker(markerType, out myMarkerObject);
            ComputerUI.scientistComputer.UpdateMarker(transform.position, myMarker);
        }
    }

    public override void GetNewWave()
    {
        //WaveSO temp = SpawnerController.instance.SpawnSideWave();
        //if (temp != null) AddToSpawn(temp.GetEnemySpawn(), temp.GetNextWaveDelay());
        //else nextWave = Time.time + PACING_LOCK;
    }

    protected override IEnumerator Spawn()
    {
        while (isActive)
        {
            if (currentPacing < targetPacing)
            {
                SpawnEnemy(SpawnerController.instance.GetEnemy(false));
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public override void IncreaseLevel(int level)
    {
        pacingFalloff = PacingController.currentDangerLevel.GetPacingFallofSide();
        targetPacing = PacingController.currentDangerLevel.GetTargetPacingSide();
        currentPacing = targetPacing;
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

    public bool IsArmored()
    {
        return false;
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

    public override void DeactivateSpawner()
    {
        base.DeactivateSpawner();
        if (ComputerUI.scientistComputer != null) if (ComputerUI.scientistComputer != null) Destroy(myMarkerObject);
        SpawnerController.instance.RemoveFromMap(transform);
        Active = false;
    }

    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        hp -= damage;
        if(hp <= 0)
        {
            DeactivateSpawner();
        }
        return damage;
    }
}
