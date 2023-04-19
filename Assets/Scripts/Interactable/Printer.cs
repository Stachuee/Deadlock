using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : PoweredInteractable
{
    [SerializeField] bool readyToCollect;

    [SerializeField] float baseProduction;
    float productionRemain;

    [SerializeField] GameObject prefabToPrint;

    protected override void Awake()
    {
        base.Awake();
        AddAction(Collect);
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
            Instantiate(prefabToPrint, transform.position, Quaternion.identity);
            readyToCollect = false;
            productionRemain = baseProduction;
        }
    }
}
