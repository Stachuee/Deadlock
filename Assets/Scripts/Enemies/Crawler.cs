using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : _EnemyBase
{
    [SerializeField]
    Transform target;



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

            if(Mathf.Abs(transform.position.x - currentTarget.transform.position.x) < 0.2f)
            {
                
                if (currentTarget.navNodeType == NavNode.NavNodeType.Horizontal)
                {
                    FindNextTarget();
                }
                else if(currentTarget.navNodeType == NavNode.NavNodeType.Stairs)
                {
                    FindNextTarget();
                    if(currentTarget.navNodeType == NavNode.NavNodeType.Stairs)
                    {
                        transform.position = currentTarget.transform.position;
                    }
                }
            }
        }
    }



    protected override void Update()
    {
        base.Update();
        
        if(damaging != null && lastAttack + attackSpeed < Time.time )
        {
            if (damaging.IsImmune())
            {
                damaging = null;
                return;
            }
            damaging.TakeDamage(damage);
            lastAttack = Time.time;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if(currentTarget != null) Gizmos.DrawLine(transform.position, currentTarget.transform.position);
    }
}
