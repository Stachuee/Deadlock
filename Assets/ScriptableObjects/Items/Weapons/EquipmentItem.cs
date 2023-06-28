using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Items/Equipment", order = 7)]
public class EquipmentItem : ItemSO
{
    [SerializeField] EquipmentType equipmentType;

    [SerializeField] ScienceItem unlocks;
    public override void Drop(PlayerController player, Item item)
    {

    }

    public override bool PickUp(PlayerController player, Item item, out bool destroy)
    {
        if (!player.isScientist)
        {
            player.equipmentController.UnlockEquipment(equipmentType);
            RecipiesManager.instance.UnlockTech(unlocks);
            destroy = true;
            return false;
        }
        else
        {
            destroy = true;
            return true;
        }
    }
}
