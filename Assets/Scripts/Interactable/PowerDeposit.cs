using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerDeposit : InteractableBase
{

    [SerializeField] List<ItemSO> acceptedItems;
    
    [SerializeField]
    ItemSO inDeposit;
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    SpriteRenderer powerCellRenderer;

    FuseBox fuseBox;

    protected override void Awake()
    {
        base.Awake();
        fuseBox = transform.GetComponentInParent<FuseBox>();
        AddAction(DepositBattery);
    }

    public void DepositBattery(PlayerController player)
    {
        if(inDeposit == null)
        {
            ItemSO deposited = player.CheckIfHoldingAnyAndDeposit(acceptedItems);

            if (deposited != null)
            {
                inDeposit = deposited;
                fuseBox.PlugIn(true);
            }
        }
        else
        {
            GameObject temp = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            temp.GetComponentInChildren<Item>().Innit(inDeposit);
            inDeposit = null;
            powerCellRenderer.sprite = null;
            fuseBox.PlugIn(false);
        }
    }

}
