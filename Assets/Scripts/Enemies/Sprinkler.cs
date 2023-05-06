using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : Crawler
{
    [SerializeField] float throwSporesCooldown;
    [SerializeField] float throwCooldownRemain;

    [SerializeField] GameObject throwPrefab;
    [SerializeField] float throwStrength;

    protected override void Update()
    {
        base.Update();
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), 1).normalized;    
        if(throwCooldownRemain < 0)
        {
            Instantiate(throwPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(direction * throwStrength);
            throwCooldownRemain = throwSporesCooldown;
        }
        throwCooldownRemain -= Time.deltaTime;
    }
}
