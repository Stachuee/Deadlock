using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Computer : InteractableBase
{
    PlayerController activePlayer;

    [SerializeField] AudioSource computerOnSFX;


    protected override void Awake()
    {
        base.Awake();
        AddAction(OpenComputer);
    }


    void OpenComputer(PlayerController player, UseType type)
    {
        DialogueManager.instance.Trigger("ComputerOpen");
        activePlayer = player;
        player.uiController.computer.OpenComputer();
        player.LockInAction(Back);
        computerOnSFX.Play();
    }

    void Back()
    {
        if (activePlayer.uiController.computer.Back())
        {
            activePlayer.UnlockInAnimation();
        }
    }

}
