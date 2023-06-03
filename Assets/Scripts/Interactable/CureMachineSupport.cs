using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CureMachineSupportType {Water, Heat, Samples }

public class CureMachineSupport : PoweredInteractable, ITakeDamage
{
    [SerializeField] float maxHp;
    [SerializeField] float hp;

    bool broken;
    [SerializeField] CureMachineSupportType type;
    [SerializeField] float baseOutput;
    [SerializeField] float addDelay;

    [SerializeField] ItemSO toRepair;

    Animator anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

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

    public void Fix(PlayerController player, UseType type)
    {
        if (player.equipmentController.GetCurrentlyEquiped() == EquipmentType.RepairKit && player.equipmentController.GetCurrentlyEquipedAmmo() > 0)
        {
            player.equipmentController.UseCurretnlyEquiped();
            player.RefreshPrompt();
            hp = maxHp;
        }
    }

    public float TakeDamage(float damage, DamageEffetcts effects = DamageEffetcts.None, float armor_piercing = 0)
    {
        hp -= damage;
        if (!broken && hp <= 0)
        {
            broken = true;
            anim.SetBool("Broken", true);
        }
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
        return player.equipmentController.GetCurrentlyEquiped() == EquipmentType.RepairKit && player.equipmentController.GetCurrentlyEquipedAmmo() > 0;
    }

    public void ApplyStatus(Status toApply)
    {
    
    }

    public float GetArmor()
    {
        return 0;
    }
    public float Heal(float ammount)
    {
        hp = Mathf.Min(ammount + hp, maxHp);
        if(broken && hp > 0)
        {
            broken = false;
            anim.SetBool("Broken", false);
        }
        return ammount;
    }
}
