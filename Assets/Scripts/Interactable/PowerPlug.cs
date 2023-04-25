using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlug : InteractableBase
{
    List<ItemSO> acceptedItems;

    [SerializeField]
    ItemSO inDeposit;
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    SpriteRenderer powerCellRenderer;

    [SerializeField]
    List<ScientistPoweredInteractable> toManage;

    void Start()
    {
        AddAction(PlugBattery);
        acceptedItems = new List<ItemSO>();
    }

    public void PlugBattery(PlayerController player)
    {
        if (inDeposit == null)
        {
            ItemSO deposited = player.CheckIfHoldingAnyAndDeposit<PowerCoreItem>();
            if (deposited != null)
            {
                inDeposit = deposited;
                powerCellRenderer.sprite = deposited.GetIconSprite();
                toManage.ForEach(powered => powered.PowerOn((deposited as PowerCoreItem).GetPowerLevel()));
            }
        }
        else
        {
            GameObject temp = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            temp.GetComponentInChildren<Item>().Innit(inDeposit);
            inDeposit = null;
            powerCellRenderer.sprite = null;
            toManage.ForEach(powered => powered.PowerOn(0));
        }
    }
}
