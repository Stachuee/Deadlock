using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoundryScript : Workbench
{
    [SerializeField]
    Vector2 itemDropOffset;

    [SerializeField] GameObject craftingBar;
    [SerializeField] Transform mask;
    [SerializeField] Vector2 startBarPos;
    [SerializeField] Vector2 endBarPos;

    CraftingRecipesSO toCraft;
    float craftProgress;
    bool crafting;

    [SerializeField] AudioSource craftedSFX;

    private void Update()
    {
        if(crafting && craftProgress > 1)
        {
            GameObject temp = Instantiate(itemPrefab, (Vector2)transform.position + itemDropOffset, Quaternion.identity);
            temp.GetComponentInChildren<Item>().Innit(toCraft.GetCraftedItem());
            craftingBar.transform.localPosition = startBarPos;
            crafting = false;
        }
        else if(crafting)
        {
            craftProgress += ((1 * (float)powerLevel / 2) / (toCraft.GetBaseCraftTime())) * Time.deltaTime;

            craftingBar.transform.localPosition = Vector2.Lerp(startBarPos, endBarPos, craftProgress);
        }
    }

    public override void Craft(PlayerController player, UseType type)
    {
        if (!powered || crafting) return;
        CraftingRecipesSO toCraft = FindRecipie();
        
        if(toCraft != null)
        {
            for (int i = 0; i < itemDeposits.Length; i++)
            {
                itemDeposits[i].RemoveIngredient(true);
            }
            Debug.Log(toCraft);
            this.toCraft = toCraft;
            craftProgress = 0;
            crafting = true;
        }
        craftedSFX.Play();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + itemDropOffset, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine((Vector2)mask.position + startBarPos, (Vector2)mask.position + endBarPos);
    }
}
