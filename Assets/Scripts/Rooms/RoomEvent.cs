using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomEvent : MonoBehaviour
{
    [SerializeField] [Range(0,1)] protected float pacingRequrement;
    [SerializeField] bool oneTime;
    [SerializeField] WarningStrength warningStrength;
    public Rooms room;

    protected virtual void Awake()
    {
        room = transform.GetComponent<Rooms>();    
    }

    public abstract void ExecuteEvent();

    public void SolveEvent()
    {
        if(!oneTime)
        {
            EventManager.eventManager.SolveEvent(this);
            EventManager.eventManager.AddEvent(this);
        }
    }

    public float GetPacingRequrement()
    {
        return pacingRequrement;
    }

    public WarningStrength GetWarningStrength()
    {
        return warningStrength;
    }

}
