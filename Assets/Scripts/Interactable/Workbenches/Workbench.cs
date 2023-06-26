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

    [SerializeField] SpriteRenderer toCraft;

    protected Deposit[] itemDeposits;

    protected PlayerController usingPlayer;
    protected int chosenIndex;

    bool validCrafting;

    protected override void Awake()
    {
        base.Awake();
        itemDeposits = transform.GetComponentsInChildren<Deposit>();
        AddAction(Craft);
    }

    private void Start()
    {
        PowerOn(1);    
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

    public void ChangedIngredient()
    {
        CraftingRecipesSO temp = FindRecipie();
        if (temp != null)
        {
            toCraft.sprite = temp.GetCraftedItem().GetIconSprite();
            toCraft.enabled = true;
            validCrafting = true;
        }
        else
        {
            validCrafting = false;
            toCraft.enabled = false;
        }
    }

    public override bool IsUsable(PlayerController player)
    {
        return validCrafting;
    }
}
