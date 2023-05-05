using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
    private void Start()
    {
        prevPos = transform.position;
        timeToDespawnRemain = timeToDespawn + Time.time;
    }

    void Update()
    {
        if (timeToDespawnRemain < Time.time) Destroy(gameObject);
        transform.position += transform.right * speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Linecast(prevPos, transform.position, ~maskToIgnore);
        if(hit.collider != null)
        {
            ITakeDamage target = hit.transform.GetComponent<ITakeDamage>();

            if (target != null) target.TakeDamage(damage, damageType);
            else return;
            if (!isPrecise) Destroy(gameObject);
            else StartCoroutine(UnableDamage());
        }
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
