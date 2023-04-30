using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : PoweredInteractable, IGetHandInfo
{

    [SerializeField] bool broken;
    [SerializeField] List<ItemSO> toRepair;

    [SerializeField] bool readyToCollect;

    [SerializeField] float baseProduction;
    [SerializeField] float productionRemain;

    [SerializeField] ItemSO toPrint;

    [SerializeField] GameObject prefabToPrint;

    protected override void Awake()
    {
        base.Awake();
        AddAction(Collect);
        productionRemain = baseProduction;
    }


    private void Update()
    {
        if(!broken && powered && !readyToCollect)
        {
            productionRemain -= Time.deltaTime;
            if(productionRemain < 0)
            {
                readyToCollect = true;
            }
        }
    }

    public void Collect(PlayerController player)
    {
        if(!broken)
        {
            if (readyToCollect)
            {
                Instantiate(prefabToPrint, transform.position, Quaternion.identity).GetComponent<Item>().Innit(toPrint);
                readyToCollect = false;
                productionRemain = baseProduction;
            }
        }
        else
        {
            ItemSO input = player.CheckIfHoldingAnyAndDeposit(toRepair[0]);
            if(input != null)
            {
                toRepair.Remove(input);
                if (toRepair.Count == 0) broken = false;
                player.UpdateHighlight();
            }
        }
    }

    public override ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer { showProgress = true, progress = (1 - productionRemain/baseProduction), showCharge = true, charged = powered, name = displayName };
    }

    public HandInfoContainer GetHandInfo()
    {
        if (broken) return new HandInfoContainer {show = true, name = toRepair[0].GetItemName(), sprite = toRepair[0].GetIconSprite() };
        else return new HandInfoContainer { show = false };
    }
}
