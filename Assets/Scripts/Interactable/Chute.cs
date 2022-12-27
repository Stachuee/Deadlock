using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chute : InteractableBase
{
    [SerializeField]
    bool outputChute;

    public static Chute outChute;

    [SerializeField]
    GameObject itemPrefab;

    protected override void Awake()
    {
        base.Awake();
        if (outputChute)
        {
            if(outChute == null)
            {
                outChute = this;
            }
            else
            {
                Debug.LogError("Multiple out chutes");
            }
        }
        else
        {
            AddAction(DumpItems);
        }
    }


    void DumpItems(PlayerController player)
    {
        for (ItemSO temp = player.DepositIngredient(); temp != null; temp = player.DepositIngredient())
        {
            outChute.DropItems(temp);
        }
    }
    
    void DropItems(ItemSO item)
    {
        GameObject temp = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        temp.GetComponentInChildren<Item>().Innit(item);
    }

}
