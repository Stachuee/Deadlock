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
        if (!powered) return;
        CraftingRecipesSO toCraft = FindRecipie();
        
        if(toCraft != null)
        {
            GameObject temp = Instantiate(itemPrefab, (Vector2)transform.position + itemDropOffset, Quaternion.identity);
            temp.GetComponentInChildren<Item>().Innit(toCraft.GetCraftedItem());
            for (int i = 0; i < itemDeposits.Length; i++)
            {
                itemDeposits[i].RemoveIngredient(true);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + itemDropOffset, 0.5f);
    }
}
