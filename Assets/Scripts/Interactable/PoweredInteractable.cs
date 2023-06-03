using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoweredInteractable : InteractableBase, PowerInterface
{
    [SerializeField] protected SwitchType useElectricityType;
    [SerializeField] protected bool powered;
    public virtual void PowerOn(bool on, string sectorName)
    {
        powered = on;
    }
    public virtual bool IsPowered()
    {
        return powered;
    }
    public SwitchType GetSwitchType()
    {
        return useElectricityType;
    }
    public override ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer() { name = displayName, showCharge = true, charged = powered};
    }
}
