using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsScript : InteractableBase
{

    [SerializeField]
    StairsScript connectedDoors;

    protected override void Awake()
    {
        base.Awake();
        AddAction(UseStairs);
    }

    void UseStairs(PlayerController player)
    {
        player.transform.position = connectedDoors.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if(connectedDoors != null) Gizmos.DrawLine(transform.position, connectedDoors.transform.position);
    }
}
