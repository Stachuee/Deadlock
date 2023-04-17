using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoweredInteractable : InteractableBase, PowerInterface
{
    [SerializeField] protected SwitchType useElectricityType;
    public abstract void PowerOn(bool on);
    public SwitchType GetSwitchType()
    {
        return useElectricityType;
    }
}
