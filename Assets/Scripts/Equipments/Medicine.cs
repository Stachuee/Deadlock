using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MedicineBase
{
    [SerializeField] float healing;
    [SerializeField] float healingDuration;
    public override void AddEffect(PlayerController pC)
    {
        pC.Heal(healing, healingDuration);
    }



}
