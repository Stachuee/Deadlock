using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : InteractableBase
{

    PlayerController activePlayer;

    [SerializeField] SwitchType type;

    protected override void Awake()
    {
        base.Awake();
        AddAction(OpenBox);
    }


    void OpenBox(PlayerController player)
    {
        activePlayer = player;
        player.uiController.fuseBox.OpenBox(type);
        player.LockInAction(CloseBox);
    }

    void CloseBox()
    {
        activePlayer.uiController.fuseBox.CloseBox();
        activePlayer.UnlockInAnimation();
    }
}
