using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoundryScript : Workbench
{
    [SerializeField]
    Vector2 itemDropOffset;
    public override void Craft(PlayerController player)
    {
        List<ItemSO> itemsInDepostis = new List<ItemSO>();

        for(int i = 0; i < itemDeposits.Length; i++)
        {
            ItemSO deposit = itemDeposits[i].GetStoredIngredient();
            if(deposit != null) itemsInDepostis.Add(deposit);
        }

        CraftingRecipesSO itemToCraft = recipesAvalible.Find(recipie =>
        {
            List<ItemSO> itemsRequired = recipie.GetIngredientsItem();
            if (itemsRequired.Count == itemsInDepostis.Count)
            {
                for(int i = 0; i < itemsRequired.Count;i++)
                {
                    if (itemsRequired[i] != itemsInDepostis[i]) return false;
                }
                return true;
            }
            return false;
        });

        if(itemToCraft != null)
        {
            for (int i = 0; i < itemDeposits.Length; i++)
            {
                itemDeposits[i].RemoveIngredient(true);
            }

            GameObject temp = Instantiate(itemPrefab, (Vector2)transform.position + itemDropOffset, Quaternion.identity);
            temp.GetComponentInChildren<Item>().Innit(itemToCraft.GetCraftedItem());
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + itemDropOffset, 0.5f);
    }
}
