using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager eventManager;

    FacilityController facilityController;

    [SerializeField]
    List<RoomEvent> events;

    [SerializeField]
    List<RoomEvent> activeEvents;


    private void Awake()
    {
        if (eventManager == null) eventManager = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        facilityController = FacilityController.facilityController;
        events = new List<RoomEvent>();
        facilityController.allRooms.ForEach(x =>
        {
            if(x.roomEvents.Count > 0)
            {
                x.roomEvents.ForEach(y => events.Add(y));
            }
        });
    }

    public void TriggerEvent(float pacing)
    {
        List<RoomEvent> goodEvents = events.FindAll(x => x.GetPacingRequrement() <= pacing);
        RoomEvent chosenEvent =  goodEvents[Random.Range(0, goodEvents.Count)];

        chosenEvent.ExecuteEvent();
        activeEvents.Add(chosenEvent);
        events.Remove(chosenEvent);

        UpdateEventsOnMap(chosenEvent, true, chosenEvent.GetWarningStrength());
    }

    public void AddEvent(RoomEvent eventToAdd)
    {
        events.Add(eventToAdd);
    }

    public void SolveEvent(RoomEvent eventToSolve)
    {
        activeEvents.Remove(eventToSolve);
        UpdateEventsOnMap(eventToSolve, false, eventToSolve.GetWarningStrength());
    }


    List<ComputerUI> listeners = new List<ComputerUI>();

    public void AddlLstener(ComputerUI listener)
    {
        listeners.Add(listener);
    }

    public void UpdateEventsOnMap(RoomEvent eventToChange, bool isNew, WarningStrength strength)
    {
        listeners.ForEach(x => x.UpdateEvent(eventToChange, isNew, strength));
    }
}

