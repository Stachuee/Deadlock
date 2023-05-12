using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeControll
{
    public void TakeControll(PlayerController player);
    public bool CanTakeControll();
    public void Leave();
}
