using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatedScientist : _EnemyBase
{
    [SerializeField]
    Transform target;

    Queue<NavNode> path;
    NavNode currentTarget;

    Rigidbody2D rb;

    ITakeDamage damaging;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

    private void OnTriggerExit2D(Collider2D collision)
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.transform.tag == "Interactable")
    //    {
    //        ITakeDamage temp = collision.transform.GetComponent<ITakeDamage>();
    //        if (temp != null && !temp.IsImmune())
    //        {
    //            damaging = temp;
    //        }
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if(collision.transform.tag == "Interactable" && damaging == collision.transform.GetComponent<ITakeDamage>())
    //    {
    //        damaging = null;
    //    }
    //}

    protected override void Start()
    {
        base.Start();
        path = NavController.instance.GetPathToScientist(transform.position);
        if (path.Count > 0)
        {
            currentTarget = path.Dequeue();
        }
    }

    private void FixedUpdate()
    {
        if (currentTarget != null && damaging == null)
        {
            Vector2 direction = new Vector2((currentTarget.transform.position - transform.position).x, 0);
            rb.velocity = direction.normalized * speed + new Vector2(0, rb.velocity.y);
            if (direction.x > currentTarget.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }


            if (direction.magnitude < 1f && path.Count > 0)
            {
                if (currentTarget.navNodeType == NavNode.NavNodeType.Stairs)
                {
                    currentTarget = path.Dequeue();
                    transform.position = currentTarget.transform.position;
                    currentTarget = path.Dequeue();
                }
                else
                {
                    currentTarget = path.Dequeue();
                }
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (damaging != null && lastAttack + attackSpeed < Time.time)
        {
            if (damaging.IsImmune()) damaging = null;
            damaging.TakeDamage(damage);
            lastAttack = Time.time;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (currentTarget != null) Gizmos.DrawLine(transform.position, currentTarget.transform.position);
    }
}
