using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRepairField : InteractableBase
{

    [SerializeField] ItemSO toRepair;
    [SerializeField] DoorScript door;
    private void Start()
    {
        AddAction(RepairDoor);
    }

    public void RepairDoor(PlayerController player)
    {
        ItemSO temp = player.CheckIfHoldingAnyAndDeposit(toRepair);
        if (temp != null)
        {
            door.Repair();
        }
    }
}
