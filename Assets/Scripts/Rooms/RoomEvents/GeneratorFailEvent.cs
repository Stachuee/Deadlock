using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorFailEvent : RoomEvent
{
    public override void ExecuteEvent()
    {
        Debug.Log("Event active");
        StartCoroutine("FixGenerator");
    }

    IEnumerator FixGenerator()
    {
        yield return new WaitForSeconds(3);
        SolveEvent();
    }
}
