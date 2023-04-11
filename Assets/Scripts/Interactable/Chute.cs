using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chute : InteractableBase
{
    [SerializeField]
    Chute connectedChute;

    [SerializeField]
    GameObject itemPrefab;

    protected override void Awake()
    {
        base.Awake();
        AddAction(DumpItems);
    }


    void DumpItems(PlayerController player)
    {
        for (ItemSO temp = player.DepositIngredient(); temp != null; temp = player.DepositIngredient())
        {
            connectedChute.DropItems(temp);
        }
    }
    
    void DropItems(ItemSO item)
    {
        GameObject temp = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        temp.GetComponentInChildren<Item>().Innit(item);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(connectedChute != null)Gizmos.DrawLine(transform.position, connectedChute.transform.position);
    }
}
