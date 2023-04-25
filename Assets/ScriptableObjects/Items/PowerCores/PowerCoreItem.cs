using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/PowerCore", order = 3)]
public class PowerCoreItem : CraftingMaterialItem
{
    [SerializeField] int powerLevel;

    public int GetPowerLevel()
    {
        return powerLevel;
    }
}
