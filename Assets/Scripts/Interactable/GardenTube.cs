using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenTube : PoweredInteractable
{
    [SerializeField]
    float growthTimer;
    [SerializeField]
    float growthTimerRemain;
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    ItemSO createdObject;

    private void Start()
    {
        AddAction(Collect);
        growthTimerRemain = growthTimer;
    }

    private void Update()
    {
        if (powered)
        {
            growthTimerRemain -= Time.deltaTime;
        }
    }

    public void Collect(PlayerController player, UseType type)
    {
        if (growthTimerRemain < 0)
        {
            growthTimerRemain = growthTimer;
            Instantiate(itemPrefab, transform.position, Quaternion.identity).GetComponentInChildren<Item>().Innit(createdObject);
        }
    }
    override public ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer { name = displayName, showCharge = true, charged = powered, showProgress = true, progress = (1- growthTimerRemain/growthTimer) };
    }
}
