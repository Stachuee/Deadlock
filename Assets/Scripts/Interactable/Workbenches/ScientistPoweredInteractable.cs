using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistPoweredInteractable : InteractableBase
{
    [SerializeField] protected bool powered;
    [SerializeField] protected int powerLevel;
    public virtual void PowerOn(int power)
    {
        powerLevel = power;
        powered = power > 0;
    }

    public bool IsPowered()
    {
        return powered;
    }
}
