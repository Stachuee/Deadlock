using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WorkbenchType { Foundry, Lab }

public abstract class Workbench : InteractableBase //, IControllSubscriberMovment
{
    [SerializeField] protected List<WorkbenchType> workbenchTypes;
    protected List<CraftingRecipesSO> recipesAvalible;

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
    private void Start()
    {
        recipesAvalible = RecipiesManager.recipies.FindAll(x => workbenchTypes.FindIndex(y => y == x.GetWorkbenchType()) != -1);
    }
    public abstract void Craft(PlayerController player);

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
