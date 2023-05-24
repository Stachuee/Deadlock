using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonAmmo : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] LayerMask maskToIgnore;
    Vector2 prevPos;

    [SerializeField]
    float timeToDespawn;
    float timeToDespawnRemain;

    [SerializeField] DamageType damageType;

    [SerializeField] bool isPrecise = false;

    Dictionary<string, bool> alreadyHit;

    bool stuck;
    private void Start()
    {
        prevPos = transform.position;
        timeToDespawnRemain = timeToDespawn + Time.time;
        if(isPrecise)
        {
            alreadyHit = new Dictionary<string, bool>();
        }
    }

    void Update()
    {
        if (timeToDespawnRemain < Time.time) Destroy(gameObject);
        if (stuck) return;
        transform.position += transform.right * speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Linecast(prevPos, transform.position, ~maskToIgnore);
        
        if(hit)
        {
            if(hit.transform.tag == "Enemy")
            {
                if(!isPrecise)
                {
                    ITakeDamage target = hit.transform.GetComponent<ITakeDamage>();
                    target.TakeDamage(damage, damageType);
                    stuck = true;
                    transform.SetParent(hit.transform);
                }
                else if(!alreadyHit.ContainsKey(hit.transform.name))
                {
                    ITakeDamage target = hit.transform.GetComponent<ITakeDamage>();
                    target.TakeDamage(damage, damageType);
                    alreadyHit.Add(hit.transform.name, true);
                }
            }
            else
            {
                stuck = true;
                return;
            }
        }

        
        //if(hit.collider != null && hit.transform.tag == "Enemy")
        //{
        //    ITakeDamage target = hit.transform.GetComponent<ITakeDamage>();

        //    if (target != null) target.TakeDamage(damage, damageType);
        //    else return;
        //    if (!isPrecise)
        //    {
        //        stuck = true;
        //        //Destroy(gameObject);
        //    }
        //    else StartCoroutine(UnableDamage());
        //}
        prevPos = transform.position;
    }

    public DamageType GetDamageType()
    {
        return damageType;
    }
    private IEnumerator UnableDamage()
    {
        float tmpDamage = damage;
        damage = 0f;
        yield return new WaitForSeconds(0.3f);
        damage = tmpDamage;
    }
}
