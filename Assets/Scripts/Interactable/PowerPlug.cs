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

    [SerializeField] Sprite empty;
    [SerializeField] Sprite full;

    [SerializeField]
    List<ScientistPoweredInteractable> toManage;

    bool firstTime = true;

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
                powerCellRenderer.sprite = full;
                toManage.ForEach(powered => powered.PowerOn((deposited as PowerCoreItem).GetPowerLevel()));
                if (firstTime)
                {
                    ProgressStageController.instance.StartGame();
                    firstTime = false;
                }
            }
        }
        else
        {
            GameObject temp = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            temp.GetComponentInChildren<Item>().Innit(inDeposit);
            inDeposit = null;
            powerCellRenderer.sprite = empty;
            toManage.ForEach(powered => powered.PowerOn(0));
        }
    }

    public override bool IsUsable(PlayerController player)
    {
        return inDeposit != null || player.CheckIfHoldingAny<PowerCoreItem>();
    }
}
