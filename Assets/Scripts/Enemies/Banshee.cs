using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banshee : Crawler
{
    [SerializeField] bool enraged = false;

    [SerializeField] float enrageingSpeed;

    [SerializeField] float rageSpeedBoost;
    [SerializeField] float rageDamageResistance;
    [SerializeField] float rageTimer;

    [SerializeField] float rageCd;
    float rageCdRemain;

    float rageRemain;

    protected override void Update()
    {

        base.Update();
        rageCdRemain -= Time.deltaTime;
    }

    public override float TakeDamage(float damage, DamageType type)
    {
        if (!enraged && rageCdRemain <= 0)
        {
            speed = enrageingSpeed;
            enraged = true;
            StartCoroutine("Enraged");
        }
        if(enraged) return base.TakeDamage(damage * (1 - rageDamageResistance), type);
        else return base.TakeDamage(damage, type);
    }

    IEnumerator Enraged()
    {
        yield return new WaitForSeconds(2);
        speed = rageSpeedBoost;
        StartCoroutine("EndOfRage");
    }

    IEnumerator EndOfRage()
    {
        yield return new WaitForSeconds(rageTimer);
        enraged = false;
        speed = baseSpeed;
        rageCdRemain = rageCd;
    }
}
