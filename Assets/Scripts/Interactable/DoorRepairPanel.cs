using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRepairPanel : InteractableBase, ITakeDamage
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

    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        return 0;
    }

    public float Heal(float ammount)
    {
        return door.Heal(ammount);
    }

    public void ApplyStatus(Status toApply)
    {
        
    }

    public void TakeArmorDamage(float damage)
    {
        
    }

    public bool IsImmune()
    {
        return true;
    }

    public bool IsArmored()
    {
        return false;
    }

}
