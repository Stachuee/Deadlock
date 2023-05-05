using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stimulator : MedicineBase
{
    [SerializeField] float effectDuration = 5f;
    float tmpSpeed;


    public override void AddEffect(PlayerController pC)
    {
        pC.Stimulate(effectDuration);
    }

    
}
