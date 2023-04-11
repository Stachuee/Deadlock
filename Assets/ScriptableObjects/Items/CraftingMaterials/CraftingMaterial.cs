using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/CraftingMaterial", order = 0)]
public class CraftingMaterial : ItemSO
{
    public override void Drop(PlayerController player, Item item)
    {

    }

    public override bool PickUp(PlayerController player, Item item)
    {
        return true;
    }
}
