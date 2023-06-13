using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistRoomDoor : InteractableBase, ITakeDamage
{
    [SerializeField]
    float doorMaxHp;

    [SerializeField]
    float doorHp;

    [SerializeField]
    bool doorSelfRepair;
    [SerializeField]
    float doorRepairSpeed;

    Rooms parrent;
    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        doorHp -= damage;
        parrent.SendWarning(WarningStrength.Strong);
        return damage;
    }

    public void TakeArmorDamage(float damage)
    {
        
    }

    private void Start()
    {
        parrent = transform.GetComponentInParent<Rooms>();
        doorHp = doorMaxHp;
    }

    private void Update()
    {
        if (doorSelfRepair)
        {
            doorHp += doorRepairSpeed * Time.deltaTime;
        }
        if (doorHp <= 0)
        {
            GameController.gameController.DestroyedScientistDoor();
            gameObject.SetActive(false);
        }

    }

    public override ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer() { name = displayName, hp = doorHp, maxHp = doorMaxHp, showHp = true };
    }

    public bool IsImmune()
    {
        return false;
    }

    public override bool IsUsable(PlayerController player)
    {
        return false;
    }

    public void ApplyStatus(Status toApply)
    {
        
    }

    public bool IsArmored()
    {
        return false;
    }

    public float Heal(float ammount)
    {
        doorHp = Mathf.Min(ammount + doorHp, doorMaxHp);
        return ammount;
    }
}
