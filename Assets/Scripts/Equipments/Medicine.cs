using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MedicineBase
{

    public override void AddEffect(PlayerController pC)
    {
        pC.Heal();
    }



}
