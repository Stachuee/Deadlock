using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerDeposit : InteractableBase
{
    [System.Serializable]
    struct PowerCellStrength
    {
        public ItemSO cell;
        public int strength;
    }


    [SerializeField] List<PowerCellStrength> powerCellsStrength;

    List<ItemSO> acceptedItems;

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

        acceptedItems = new List<ItemSO>();
        powerCellsStrength.ForEach(x => acceptedItems.Add(x.cell));
    }

    public void DepositBattery(PlayerController player)
    {
        if(inDeposit == null)
        {
            ItemSO deposited = player.CheckIfHoldingAnyAndDeposit(acceptedItems);
            if (deposited != null)
            {
                inDeposit = deposited;
                powerCellRenderer.sprite = deposited.GetIconSprite();
                fuseBox.PlugIn(powerCellsStrength.Find(x => x.cell == deposited).strength);
            }
        }
        else
        {
            GameObject temp = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            temp.GetComponentInChildren<Item>().Innit(inDeposit);
            inDeposit = null;
            powerCellRenderer.sprite = null;
            fuseBox.PlugIn(0);
        }
    }

}
