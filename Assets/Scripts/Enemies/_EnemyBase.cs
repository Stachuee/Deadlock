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

    [SerializeField] float patience;

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

    protected ITakeDamage damaging;

    

    protected NavNode currentTargetNode;
    protected Vector2 target;
    protected bool followNode;

    protected Rigidbody2D rb;


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (damaging == null && (collision.transform.tag == "Interactable" || collision.transform.tag == "Player"))
        {
            ITakeDamage temp = collision.transform.GetComponent<ITakeDamage>();
            if (temp != null && !temp.IsImmune())
            {
                damaging = temp;
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Interactable" || collision.transform.tag == "Player")
        {
            ITakeDamage temp = collision.transform.GetComponent<ITakeDamage>();
            if (temp == damaging)
            {
                damaging = null;
            }
        }
    }


    protected virtual void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }
    
    protected virtual void Start()
    {
        hp = maxHp;
        baseSpeed = Random.Range(randomSpeed.x, randomSpeed.y);
        speed = baseSpeed;
        if(ComputerUI.scientistComputer != null) myMarker = ComputerUI.scientistComputer.CreateMarker(markerType);
        currentTargetNode = NavController.instance.FindClosestWaypoint(transform.position, true, true);

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
                TakeDamage(CombatController.POISON_DAMAGE_PER_TICK, DamageSource.Enemy);
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


        AttackNearby();
    }

    protected virtual void FixedUpdate()
    {
        if (currentTargetNode != null && damaging == null)
        {
            Vector2 direction = new Vector2((currentTargetNode.transform.position - transform.position).x, 0);
            rb.velocity = direction.normalized * speed + new Vector2(0, rb.velocity.y);
            if (direction.x > currentTargetNode.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            if (Mathf.Abs(transform.position.x - currentTargetNode.transform.position.x) < 0.2f)
            {

                if (currentTargetNode.navNodeType == NavNode.NavNodeType.Horizontal)
                {
                    FindNextNodeTarget();
                }
                else if (currentTargetNode.navNodeType == NavNode.NavNodeType.Stairs)
                {
                    FindNextNodeTarget();
                    if (currentTargetNode.navNodeType == NavNode.NavNodeType.Stairs)
                    {
                        transform.position = currentTargetNode.transform.position;
                    }
                }
            }
        }
    }

    public virtual void AttackNearby()
    {
        if (damaging != null && lastAttack + attackSpeed < Time.time)
        {
            if (damaging.IsImmune())
            {
                damaging = null;
                return;
            }
            damaging.TakeDamage(damage, DamageSource.Enemy);
            lastAttack = Time.time;
        }
    }

    public virtual float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {

        float damageTaken = damage;

        switch (effects)
        {
            case DamageEffetcts.None:
                damageTaken = (1 - (armor)) * damage;
                break;
            case DamageEffetcts.Disintegrating:
                damageTaken = CombatController.DISINTEGRATING_FALLOFF.Evaluate(armor) * damage;
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

    public virtual float TakeTrueDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0) Dead();
        return damage;
    }
    protected void FindNextNodeTarget()
    {
        float closestWithObs = Mathf.Infinity;
        NavNode closestObs = null;

        currentTargetNode.GetConnectedNodes().ForEach(node =>
        {
            if (node.distanceToScientist + node.obstaclesWeigths < closestWithObs)
            {
                closestWithObs = node.distanceToScientist + node.obstaclesWeigths + Vector2.Distance(transform.position, node.transform.position);
                closestObs = node;
            }
        });


        if (closestWithObs < currentTargetNode.distanceToScientist + patience)
        {
            patience = Mathf.Clamp(patience - Mathf.Max(0, closestWithObs - currentTargetNode.distanceToScientist), 0, patience);
            currentTargetNode = closestObs;
            return;
        }

        float closestWithoutObs = Mathf.Infinity;
        NavNode closestWoObs = null;
        currentTargetNode.GetConnectedNodes().ForEach(node =>
        {
            if (node.distanceToScientist < closestWithoutObs)
            {
                closestWithoutObs = node.distanceToScientist + node.obstaclesWeigths + Vector2.Distance(transform.position, node.transform.position);
                closestWoObs = node;
            }
        });

        currentTargetNode = closestWoObs;
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
    public Transform GetTransform()
    {
        return transform;
    }
}
