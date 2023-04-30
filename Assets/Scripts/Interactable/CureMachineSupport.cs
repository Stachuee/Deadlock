using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CureMachineSupportType {Water, Heat, Samples }

public class CureMachineSupport : PoweredInteractable, ITakeDamage
{
    [SerializeField] float maxHp;
    [SerializeField] float hp;
    [SerializeField] CureMachineSupportType type;
    [SerializeField] float baseOutput;
    [SerializeField] float addDelay;
    private void Start()
    {
        AddAction(Fix);
        StartCoroutine("AddSupport");
    }

    IEnumerator AddSupport()
    {
        while(true)
        {
            yield return new WaitForSeconds(addDelay);
            if (hp > 0 && powered) CureMachine.Instance.AddSupport((int)type, baseOutput * addDelay);
        }
    }

    public void Fix(PlayerController player)
    {
        hp = maxHp;
    }

    public float TakeDamage(float damage, DamageType type)
    {
        hp -= damage;
        return damage;
    }
}
