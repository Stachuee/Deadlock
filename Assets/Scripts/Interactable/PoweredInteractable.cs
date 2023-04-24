using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoweredInteractable : InteractableBase, PowerInterface
{
    [SerializeField] protected SwitchType useElectricityType;
    [SerializeField] protected bool powered;
    public virtual void PowerOn(bool on)
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
    public override InfoContainer GetInfo()
    {
        return new InfoContainer() { name = displayName, showCharge = true, charged = powered};
    }
}
