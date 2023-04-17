using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : PoweredInteractable
{
    [SerializeField] bool working;
    [SerializeField] bool readyToCollect;

    [SerializeField] float baseProduction;
    float productionRemain;

    [SerializeField] GameObject prefabToPrint;

    protected override void Awake()
    {
        base.Awake();
        AddAction(Collect);
    }


    override public void PowerOn(bool on)
    {
        working = on;
    }

    private void Update()
    {
        if(working && !readyToCollect)
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
            Instantiate(prefabToPrint, transform.position, Quaternion.identity);
            readyToCollect = false;
            productionRemain = baseProduction;
        }
    }
}
