using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRepairPanel : InteractableBase
{
    StairsScript stairs;
    DoorsBetweenRooms door;


    private void Start()
    {
        AddAction(RepairDoor);
        stairs = GetComponentInParent<StairsScript>();
        door = GetComponentInParent<DoorsBetweenRooms>();
    }

    public void RepairDoor(PlayerController player, UseType type)
    {
        if (player.equipmentController.GetCurrentlyEquiped() == EquipmentType.RepairKit && player.equipmentController.GetCurrentlyEquipedAmmo() > 0)
        {
            if(door != null) door.Repair();
            if (stairs != null) stairs.Repair();
            player.equipmentController.UseCurretnlyEquiped();
            player.RefreshPrompt();
        }
    }

    public override bool IsUsable(PlayerController player)
    {
        return player.equipmentController.GetCurrentlyEquiped() == EquipmentType.RepairKit && player.equipmentController.GetCurrentlyEquipedAmmo() > 0;
    }
}
