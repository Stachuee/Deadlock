using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : _EnemyBase
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Interactable")
        {
            ITakeDamage temp = collision.transform.GetComponent<ITakeDamage>();
            if(temp != null)
            {
                damaging = temp;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Interactable")
        {
            ITakeDamage temp = collision.transform.GetComponent<ITakeDamage>();
            if (temp == damaging)
            {
                damaging = null;
            }
        }
    }

    private void Start()
    {
        path = NavController.instance.GetPathToScientist(transform.position);
        if(path.Count > 0)
        {
            currentTarget = path.Dequeue();
        }
    }

    private void Update()
    {
        if(currentTarget != null)
        {
            Vector2 direction = new Vector2((currentTarget.transform.position - transform.position).x, 0);
            rb.velocity = direction.normalized * speed;

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

        if(damaging != null && lastAttack + attackSpeed < Time.time )
        {
            damaging.TakeDamage(damage, DamageType.Mele);
            lastAttack = Time.time;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if(currentTarget != null) Gizmos.DrawLine(transform.position, currentTarget.transform.position);
    }
}
