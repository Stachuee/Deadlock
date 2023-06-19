using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveEnemy : _EnemyBase
{
    protected enum EnemyState {Passive, Hunting }
    [SerializeField] protected EnemyState state;
    [SerializeField] float huntingTimer;
    float huntingTimeEnd;

    Vector2 huntingTarget;

    protected override void Update()
    {
        base.Update();
        if (state == EnemyState.Hunting && PlayerController.solider.IsImmune() || huntingTimeEnd < Time.time)
        {
            state = EnemyState.Passive;
            return;
        }
    }

    public override float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        if (source == DamageSource.Player)
        {
            if (state == EnemyState.Passive)
            {
                Enrage();
            }
            else
            {
                target = PlayerController.solider.transform.position;
                huntingTimeEnd = Time.time + huntingTimer;
            }
        }

        return base.TakeDamage(damage, source, effects);
    }

    protected virtual void Enrage()
    {
        huntingTarget = PlayerController.solider.transform.position;
        state = EnemyState.Hunting;
        followNode = false;
        target = PlayerController.solider.transform.position;
        if (damaging != null && damaging.GetTransform().tag != "Player") damaging = null;
        huntingTimeEnd = Time.time + huntingTimer;
    }

    protected override void FixedUpdate()
    {
        if (state == EnemyState.Hunting)
        {
            EnrageMovment();
        }
        else if (currentTargetNode != null && damaging == null)
        {
            PassiveMovment();
        }
    }

    protected virtual void EnrageMovment()
    {
        if (huntingTimeEnd <= Time.time)
        {
            state = EnemyState.Passive;
            followNode = false;
            FindNextNodeTarget();
        }
        Vector2 direction = new Vector2((target - (Vector2)transform.position).x, 0);
        rb.velocity = direction.normalized * speed + new Vector2(0, rb.velocity.y);
    }

    protected virtual void PassiveMovment()
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
