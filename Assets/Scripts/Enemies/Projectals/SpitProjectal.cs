using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitProjectal : MonoBehaviour, ITakeDamage
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float lifeTime;

    [SerializeField] LayerMask maskToIgnore;
    Vector2 prevPos;


    Vector2 startingPos;
    Vector2 endPosition;
    [SerializeField] float arcHeight;
    public void ApplyStatus(Status toApply)
    {
        
    }

    public bool IsArmored()
    {
        return false;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public float Heal(float ammount)
    {
        return 0;  
    }

    public bool IsImmune()
    {
        return false;
    }

    public void TakeArmorDamage(float damage)
    {
        
    }

    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        Destroy(gameObject);
        return 0;
    }

    private void Start()
    {
        prevPos = transform.position;
        startingPos = transform.position;
        endPosition = (Vector2)PlayerController.solider.transform.position + Vector2.down;
        Destroy(gameObject, lifeTime);
    }

    //private void Update()
    //{
    //    //transform.position += transform.right * speed * Time.deltaTime;
    //    RaycastHit2D hit = Physics2D.Linecast(prevPos, transform.position, ~maskToIgnore);

    //    if (hit)
    //    {
    //        if (hit.transform.tag == "Player")
    //        {
    //            ITakeDamage temp = hit.transform.GetComponent<ITakeDamage>();
    //            temp.TakeDamage(damage, DamageSource.Enemy);
    //            Destroy(gameObject);
    //        }
    //        else
    //        {
    //            Destroy(gameObject);
    //        }
    //    }


    //    prevPos = transform.position;
    //}

    private void Update()
    {
        float x0 = startingPos.x;
        float x1 = endPosition.x;
        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
        float baseY = Mathf.Lerp(startingPos.y, endPosition.y, (nextX - x0) / dist);
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
        transform.position = new Vector3(nextX, baseY + arc, transform.position.z);

        RaycastHit2D hit = Physics2D.Linecast(prevPos, transform.position, ~maskToIgnore);

        if (hit)
        {
            if (hit.transform.tag == "Player")
            {
                ITakeDamage temp = hit.transform.GetComponent<ITakeDamage>();
                temp.TakeDamage(damage, DamageSource.Enemy);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        prevPos = transform.position;
    }
}
