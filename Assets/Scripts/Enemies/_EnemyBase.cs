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

    [SerializeField] int patience;
    [SerializeField] float currentDistance;

    [SerializeField]
    bool armored;
    [SerializeField]
    float armorMaxHp;
    float armorHp;
    [SerializeField, Range(0f,1f)] float dropChance;
    [SerializeField] GameObject itemPrefab;

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

    bool psiBoosted;
    float psiBoosterTimer;

    [SerializeField] Marker markerType;
    RectTransform myMarker;
    GameObject myMarkerObject;

    protected ITakeDamage damaging;

    [SerializeField] GameObject deathAnim;
    

    protected NavNode currentTargetNode;
    protected Vector2 target;
    protected bool followNode;

    protected Rigidbody2D rb;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    [SerializeField] bool useBones;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (damaging == null && (collision.transform.tag == "Interactable" || collision.transform.tag == "Player"))
        {
            ITakeDamage temp = collision.transform.GetComponent<ITakeDamage>();
            if ( temp != null && !temp.IsImmune())
            {
                if(temp is ITakeDamageInteractable)
                {
                    if((temp as ITakeDamageInteractable).AlwaysAttack())
                    {
                        damaging = temp;
                    }
                    else if(Mathf.Abs(currentTargetNode.transform.position.x - transform.position.x) < 1.5f) // workaround on attacking doors
                    {
                        damaging = temp;
                    }
                }
                else
                {
                    damaging = temp;
                }
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
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    protected virtual void Start()
    {
        hp = maxHp * PacingController.pacingController.hpModifier;
        armorHp = armorMaxHp;

        patience += Random.Range(-1, 2);
        currentDistance = -1;

        baseSpeed = Random.Range(randomSpeed.x, randomSpeed.y);
        speed = baseSpeed;
        if (ComputerUI.scientistComputer != null)
        {
            myMarker = ComputerUI.scientistComputer.CreateMarker(markerType, out myMarkerObject);
        }

        currentTargetNode = NavController.instance.FindClosestWaypoint(transform.position, true);
        currentDistance = currentTargetNode.distanceToScientist;

        SpawnerController.instance.AddEnemyToMap(this, transform);

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

        if(psiBoosted)
        {
            if(psiBoosterTimer <= Time.time)
            {
                psiBoosted = false;
                speed -= CombatController.PSI_BOOST_SPEED_INCREASE;
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
            if (transform.position.x > damaging.GetTransform().position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            animator.SetTrigger("attack");
            damaging.TakeDamage(damage, DamageSource.Enemy);
            lastAttack = Time.time;
        }
        else
        {
            animator.ResetTrigger("attack");
        }
    }

    public virtual float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {

        float damageTaken = damage;

        switch (effects)
        {
            case DamageEffetcts.None:
                damageTaken = (armored ? CombatController.ARMOR_DAMAGE_REDUCTION : 1) * (psiBoosted ? CombatController.PSI_BOOST_DAMAGE_REDUCTION : 1) * damage * (frozen ? CombatController.FREEZE_DAMAGE_INCREASE : 1);
                break;
            case DamageEffetcts.Disintegrating:
                damageTaken = (armored ? CombatController.DISINTEGRATING_FALLOFF : 1) * (psiBoosted ? CombatController.PSI_BOOST_DAMAGE_REDUCTION : 1) * damage * (frozen ? CombatController.FREEZE_DAMAGE_INCREASE : 1);
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
        armorHp = armorHp - damage;
        if (armorHp <= 0)
        {
            armored = false;
        }
    }

    //private void OnParticleCollision(GameObject other)
    //{
    //    FireGunDamageType damageType = other.GetComponent<FireGunDamageType>();
    //    TakeDamage(10f, damageType.GetDamageType());
    //}

    public virtual void Dead()
    {
        SpawnerController.instance.RemoveFromMap(transform);
        if (ComputerUI.scientistComputer != null) Destroy(myMarkerObject);
        Debug.Log(dropChance);
        if(Random.Range(0f, 1f) <= dropChance)
        {
            ItemSO toDrop = PacingController.pacingController.DropItem();
            Debug.Log("dropped " + toDrop);
            if (toDrop != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity).GetComponent<Item>().Innit(toDrop);
            }
        }
        Destroy(Instantiate(deathAnim, transform.position, Quaternion.identity), 1);
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


        float closestWithoutObs = Mathf.Infinity;
        NavNode closestWoObs = null;

        currentTargetNode.GetConnectedNodes().ForEach(node =>
        {
            float dist = Vector2.Distance(transform.position, node.transform.position);
            if (node.distanceToScientistWithObsticles + dist < closestWithObs)
            {
                closestWithObs = node.distanceToScientistWithObsticles + dist;
                closestObs = node;
            }
            if (node.distanceToScientist + dist < closestWithoutObs)
            {
                closestWithoutObs = node.distanceToScientist + dist;
                closestWoObs = node;
            }
        });

        if((patience > 0 || closestObs == closestWoObs))
        {
            if(currentDistance < closestObs.distanceToScientistWithObsticles && closestObs != closestWoObs) patience--;
            currentDistance = closestObs.distanceToScientistWithObsticles;
            currentTargetNode = closestObs;
            return;
        }  

        //if (closestObs != closestWoObs && closestWithObs - closestWithoutObs <= patience)
        //{
        //    patience = Mathf.Clamp(patience - Mathf.Max(0, closestWithObs - currentTargetNode.distanceToScientist), 0, patience);
        //    currentTargetNode = closestObs;
        //    return;
        //}
        currentDistance = closestWoObs.distanceToScientist;
        currentTargetNode = closestWoObs;
    }
    public bool IsImmune()
    {
        return false;
    }

    public bool IsArmored()
    {
        return armored;
    }

    public void PsiBoost(float duration)
    {
        if (psiBoosted) return;
        psiBoosted = true;
        psiBoosterTimer = Time.time + duration;
        speed += CombatController.PSI_BOOST_SPEED_INCREASE;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, currentTargetNode.transform.position);
    }
}
