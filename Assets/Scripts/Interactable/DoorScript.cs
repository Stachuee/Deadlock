using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : PoweredInteractable
{
    bool opened;
    [SerializeField] GameObject door;


    public void Use()
    {
        if (!powered) return;
        opened = !opened;
        door.SetActive(opened);
    }

    override public void PowerOn(bool on)
    {
        powered = on;
        if(!on)
        {
            door.SetActive(false);
        }
    }
}
