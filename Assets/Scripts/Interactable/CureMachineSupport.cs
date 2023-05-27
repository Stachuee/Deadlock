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

    [SerializeField] ItemSO toRepair;

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
        ItemSO temp = player.CheckIfHoldingAnyAndDeposit(toRepair);
        if (temp != null)
        {
            hp = maxHp;
        }
    }

    public float TakeDamage(float damage, DamageEffetcts effects = DamageEffetcts.None)
    {
        hp -= damage;
        return damage;
    }

    public void TakeArmorDamage(float damage)
    {
        
    }

    public bool IsImmune()
    {
        return hp <= 0;
    }

    public override bool IsUsable(PlayerController player)
    {
        return player.CheckIfHoldingAny(toRepair);
    }

    public void ApplyStatus(Status toApply)
    {
    
    }

    public float GetArmor()
    {
        return 0;
    }
}
