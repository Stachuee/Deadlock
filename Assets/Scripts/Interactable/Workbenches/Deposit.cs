using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deposit : InteractableBase
{
    [SerializeField]
    ItemSO inDeposit;
    [SerializeField]
    GameObject itemPrefab;

    SpriteRenderer myRenderer;

    protected override void Awake()
    {
        base.Awake();
        myRenderer = transform.GetComponent<SpriteRenderer>();
        AddAction(AddIngredient);
    }

    void AddIngredient(PlayerController player, UseType type)
    {
        if (inDeposit == null)
        {
            ItemSO temp = player.DepositIngredient();
            if(temp != null)
            {
                inDeposit = temp;
                myRenderer.sprite = temp.GetIconSprite();
            }
        }
        else
        {
            RemoveIngredient();
        }
    }

    public void RemoveIngredient(bool usedInCrafting = false)
    {
        if(!usedInCrafting)
        {
            GameObject temp = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            temp.GetComponentInChildren<Item>().Innit(inDeposit);
        }
        inDeposit = null;
        myRenderer.sprite = null;
    }

    public ItemSO GetStoredIngredient()
    {
        return inDeposit;
    }

}
