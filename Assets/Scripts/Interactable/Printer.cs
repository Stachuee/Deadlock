using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : PoweredInteractable, IGetHandInfo
{

    [SerializeField] bool broken;
    [SerializeField] ItemSO toRepair;

    [SerializeField] bool readyToCollect;

    [SerializeField] float baseProduction;
    [SerializeField] float productionRemain;

    [SerializeField] ItemSO toPrint;

    [SerializeField] GameObject prefabToPrint;

    [SerializeField] GameObject craftingBar;
    [SerializeField] Transform mask;
    [SerializeField] Vector2 startBarPos;
    [SerializeField] Vector2 endBarPos;
    
    
    
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
            craftingBar.transform.localPosition = Vector2.Lerp(startBarPos, endBarPos, 1 - (productionRemain / baseProduction));
        }
    }

    public void Collect(PlayerController player, UseType type)
    {
        if(!broken)
        {
            if (readyToCollect)
            {
                Instantiate(prefabToPrint, transform.position + new Vector3(Random.Range(-0.1f, 0.1f), 0 , 0), Quaternion.identity).GetComponentInChildren<Item>().Innit(toPrint);
                readyToCollect = false;
                productionRemain = baseProduction;
            }
        }
        else
        {
            ItemSO input = player.CheckIfHoldingAnyAndDeposit(toRepair);
            if(input != null)
            {
                broken = false;
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
        if (broken) return new HandInfoContainer {show = true, name = toRepair.GetItemName(), sprite = toRepair.GetIconSprite() };
        else return new HandInfoContainer { show = false };
    }

    public override bool IsUsable(PlayerController player)
    {
        return readyToCollect || (broken && player.CheckIfHoldingAny(toRepair));
    }
}
