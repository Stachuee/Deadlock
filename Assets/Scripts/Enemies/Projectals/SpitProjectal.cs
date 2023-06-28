using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitProjectal : MonoBehaviour, ITakeDamage
{
    [SerializeField] float damage;
    [SerializeField] float lifeTime;

    [SerializeField] LayerMask maskToIgnore;

    [SerializeField] Rigidbody2D myRigidbody;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite splat;
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
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        myRigidbody.bodyType = RigidbodyType2D.Kinematic;
        //spriteRenderer.sprite = splat;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerController>().TakeDamage(damage, DamageSource.Enemy);
        }
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


}
