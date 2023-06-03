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

    public void RepairDoor(PlayerController player, UseType type)
    {
        if (player.equipmentController.GetCurrentlyEquiped() == EquipmentType.RepairKit && player.equipmentController.GetCurrentlyEquipedAmmo() > 0)
        {
            door.Repair();
            player.equipmentController.UseCurretnlyEquiped();
            player.RefreshPrompt();
        }
    }

    public override bool IsUsable(PlayerController player)
    {
        return player.equipmentController.GetCurrentlyEquiped() == EquipmentType.RepairKit && player.equipmentController.GetCurrentlyEquipedAmmo() > 0;
    }
}
