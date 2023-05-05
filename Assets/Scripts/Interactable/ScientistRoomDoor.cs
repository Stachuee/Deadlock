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

    public float TakeDamage(float damage, DamageType type)
    {
        doorHp -= damage;
        return damage;
    }

    public void TakeArmorDamage(DamageType type, float damage)
    {
        
    }

    private void Start()
    {
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
}
