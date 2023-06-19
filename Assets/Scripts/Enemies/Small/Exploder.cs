using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : ActiveEnemy
{
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionDamage;

    [SerializeField] float explosionDuration;
    [SerializeField] EffectManager.ScreenShakeRange explosionRange;
    [SerializeField] EffectManager.ScreenShakeStrength explosionStrength;

    [SerializeField] protected GameObject explosionVFX;

    [SerializeField] float explodeTime;
    float explodeTimer;

    bool exploded;

    bool primed;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (damaging == null && ((state == EnemyState.Passive && collision.transform.tag == "Interactable") || (collision.transform.tag == "Player")))
        {
            ITakeDamage temp = collision.transform.GetComponent<ITakeDamage>();
            if (temp != null && !temp.IsImmune())
            {
                damaging = temp;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if(primed && explodeTimer <= Time.time)
        {
            Explode();
        }
    }

    protected override void Enrage()
    {
        base.Enrage();
        explodeTimer = explodeTime + Time.time;
        primed = true;
    }

    public override void AttackNearby()
    {
        if (damaging != null && lastAttack + attackSpeed < Time.time)
        {
            if (damaging.IsImmune())
            {
                damaging = null;
                return;
            }
            if (damaging.GetTransform().tag != "Player")
            {
                damaging.TakeDamage(damage, DamageSource.Enemy);
                lastAttack = Time.time;
            }
            else
            {
                Explode();
            }
        }
    }

    public override void Dead()
    {
        Explode();
        base.Dead();
    }



    void Explode()
    {
        if (exploded) return;
        exploded = true;
        EffectManager.instance.ScreenShake(explosionDuration, explosionRange, explosionStrength, transform.position);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider.transform != transform || collider.CompareTag("Player"))
            {
                ITakeDamage target = collider.GetComponent<ITakeDamage>();
                if (target != null)
                {
                    target.TakeDamage(explosionDamage, DamageSource.Enemy);
                }
            }
        }
        Destroy(Instantiate(explosionVFX, (Vector2)transform.position + Vector2.up * 0.5f, Quaternion.identity), 1);

        Destroy(gameObject);
    }
}
