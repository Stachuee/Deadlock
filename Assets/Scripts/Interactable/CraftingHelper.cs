using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingHelper : InteractableBase
{

    PlayerController activePlayer;
    private void Start()
    {
        AddAction(Open);
    }

    public void Open(PlayerController player)
    {
        activePlayer = player;
        player.LockInAction(CloseInterface);
        activePlayer.uiController.craftingHelper.Open(true);
    }

    public void CloseInterface()
    {
        activePlayer.UnlockInAnimation();
        activePlayer.uiController.craftingHelper.Open(false);
    }
}
