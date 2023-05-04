using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipes", menuName = "ScriptableObjects/CraftingRecipe", order = 2)]
public class CraftingRecipesSO: ScriptableObject
{
    public enum CraftingTab { Hero, AssaultRiffle, Harpoon, Flamethrower, RPG, Laser, Consumable }

    [SerializeField] int id;
    [SerializeField] WorkbenchType workbenchType;
    [SerializeField] float baseCraftTime;
    [SerializeField] ItemSO craftedItem;
    [SerializeField] List<ItemSO> ingredients;
    [SerializeField] CraftingTab tab;


    #region Get/Set
    public int GetId()
    {
        return id;
    }
    public WorkbenchType GetWorkbenchType()
    {
        return workbenchType;
    }

    public float GetBaseCraftTime()
    {
        return baseCraftTime;
    }
    public ItemSO GetCraftedItem()
    {
        return craftedItem;
    }

    public List<ItemSO> GetIngredientsItem()
    {
        return ingredients;
    }

    public CraftingTab GetTab()
    {
        return tab;
    }
    #endregion
}
