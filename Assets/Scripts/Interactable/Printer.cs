using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : PoweredInteractable
{
    [SerializeField] bool readyToCollect;

    [SerializeField] float baseProduction;
    [SerializeField]float productionRemain;

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
        if(powered && !readyToCollect)
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
        if(readyToCollect)
        {
            Instantiate(prefabToPrint, transform.position, Quaternion.identity).GetComponent<Item>().Innit(toPrint);
            readyToCollect = false;
            productionRemain = baseProduction;
        }
    }

    public override ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer { showProgress = true, progress = (1 - productionRemain/baseProduction), showCharge = true, charged =powered, name = displayName };
    }
}
