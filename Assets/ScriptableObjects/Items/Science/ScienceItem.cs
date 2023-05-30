using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Science", menuName = "ScriptableObjects/Items/Science", order = 5)]
public class ScienceItem : ItemSO
{

    [SerializeField] List<CraftingRecipesSO> unlocks;

    public List<CraftingRecipesSO> GetUnlockedRecipies()
    {
        return unlocks;
    }

    public override void Drop(PlayerController player, Item item)
    {
        Debug.LogError("Somehow dropped sciencepack");
    }

    public override bool PickUp(PlayerController player, Item item, out bool destroy)
    {
        destroy = true;
        RecipiesManager.instance.UnlockTech(this);
        return false;
    }
}
