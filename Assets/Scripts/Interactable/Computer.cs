using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Computer : InteractableBase
{

    protected override void Awake()
    {
        base.Awake();
        AddAction(OpenComputer);
    }


    void OpenComputer(PlayerController player)
    {

    }

}
