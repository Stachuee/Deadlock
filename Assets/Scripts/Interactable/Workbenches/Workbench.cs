using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WorkbenchType { Foundry, Lab }

public abstract class Workbench : ScientistPoweredInteractable //, IControllSubscriberMovment
{
    [SerializeField] protected List<WorkbenchType> workbenchTypes;

    [SerializeField]
    protected GameObject itemPrefab;

    protected Deposit[] itemDeposits;

    protected PlayerController usingPlayer;
    protected int chosenIndex;

    protected override void Awake()
    {
        base.Awake();
        itemDeposits = transform.GetComponentsInChildren<Deposit>();
        AddAction(Craft);
    }
    public abstract void Craft(PlayerController player, UseType type);

    protected CraftingRecipesSO FindRecipie()
    {
        List<ItemSO> itemsInDepostis = new List<ItemSO>();

        for (int i = 0; i < itemDeposits.Length; i++)
        {
            ItemSO deposit = itemDeposits[i].GetStoredIngredient();
            if (deposit != null) itemsInDepostis.Add(deposit);
        }

        int hash = RecipiesManager.GetHashFromItems(itemsInDepostis);
        CraftingRecipesSO toCraft = (CraftingRecipesSO)RecipiesManager.recipes[hash];
        return toCraft;
    }

    //public void SwapRecipe(bool right)
    //{
    //    if (right)
    //    {
    //        if (chosenIndex + 1 >= recipesAvalible.Length) chosenIndex = 0;
    //        else chosenIndex++;
    //    }
    //    else
    //    {
    //        if (chosenIndex - 1 < 0) chosenIndex = recipesAvalible.Length - 1;
    //        else chosenIndex--;
    //    }
    //}


    //public void ForwardCommandMovment(Vector2 context)
    //{
    //    if(nextSwap < Time.time && Mathf.Abs(context.x) > 0.3f)
    //    {
    //        nextSwap = Time.time + swapCooldown;
    //        if (context.x > 0) SwapRecipe(true);    
    //        else SwapRecipe(false);
    //    }
    //}

}
