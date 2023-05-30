using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeSO : ItemSO
{

    [SerializeField] bool forScientist;

    public override void Drop(PlayerController player, Item item)
    {
    
    }

    public override bool PickUp(PlayerController player, Item item, out bool destroy)
    {
        if (forScientist == player.isScientist)
        {
            if(!player.HasUpgrade(id))
            {
                PickUpUpgrade(player);
                player.GetUpgrade(id);
            }
            else
            {
                //play dialogue
            }
            destroy = true;
            return false;
        }
        else
        {
            destroy = true;
            return true;
        }

    }

    public abstract void PickUpUpgrade(PlayerController player);
}
