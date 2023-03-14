using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Computer : InteractableBase
{
    PlayerController activePlayer;

    protected override void Awake()
    {
        base.Awake();
        AddAction(OpenComputer);
    }


    void OpenComputer(PlayerController player)
    {
        activePlayer = player;
        player.uiController.computer.OpenComputer();
        player.LockInAction(Back);
    }

    void Back()
    {
        if (activePlayer.uiController.computer.Back())
        {
            activePlayer.UnlockInAnimation();
        }
    }
}
