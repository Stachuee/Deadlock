using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBreaker : InteractableBase
{
    MapSegment segment;

    bool on;

    private void Start()
    {
        segment = GetComponentInParent<MapSegment>();
        AddAction(Activate);
    }


    public void Activate(PlayerController player, UseType type)
    {
        on = true;
        segment.UnlockSegment(on);
    }

    public void Deactivate()
    {
        on = false;
        segment.UnlockSegment(on);
    }

}
