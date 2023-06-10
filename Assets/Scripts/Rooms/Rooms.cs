using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct Interactable
{
    public Vector2 interactablePosition;
}


[ExecuteInEditMode]
public class Rooms : MonoBehaviour
{

    public static Rooms starting;
    //[SerializeField]
    //public string roomGUID;
    [SerializeField]
    public Vector2Int roomSize;
    [SerializeField]
    public bool startingRoom;
    [SerializeField]
    public List<RoomEvent> roomEvents;


    public List<IInteractable> remoteAvtivation { get; private set; }
    public List<DoorMarker> doorMarkers { get; private set; }
    public List<StairsScript> stairs { get; private set; }

    MapSegment mySegment;

    public Vector2Int RoomSize
    {
        set
        {
            roomSize = value;
        }
        get
        {
            return roomSize;
        }
    }

    [SerializeField]
    Vector2 position;
    public Vector2 Position
    {
        set
        {
            position = value;
            transform.position = value;
        }
        get
        {
            return position;
        }
    }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            remoteAvtivation = new List<IInteractable>();
            doorMarkers = new List<DoorMarker>();
            stairs = new List<StairsScript>();
            stairs = transform.GetComponentsInChildren<StairsScript>().ToList();
            doorMarkers = transform.GetComponentsInChildren<DoorMarker>().ToList();
            if (startingRoom) starting = this;

            mySegment = GetComponentInParent<MapSegment>();

            mySegment.AddRoom(this);

            List<IInteractable> allInteractables = transform.GetComponentsInChildren<IInteractable>().ToList();


            allInteractables.ForEach(interactable =>
            {
                if(interactable is PoweredInteractable)
                {
                    PoweredInteractable powered = interactable as PoweredInteractable;
                    if (powered.GetSwitchType() == SwitchType.Doors) mySegment.AddDoors(powered);
                    else if (powered.GetSwitchType() == SwitchType.Printers) mySegment.AddPrinters(powered);
                    else if (powered.GetSwitchType() == SwitchType.Lights) mySegment.AddLight(powered);
                    else if (powered.GetSwitchType() == SwitchType.Security) mySegment.AddSecurity(powered);
                }

                if (interactable != null && !interactable.IsRemote())
                {
                    remoteAvtivation.Add(interactable);
                }
            });
            

            ////roomGUID = System.Guid.NewGuid().ToString();
            //foreach (Transform child in transform)
            //{
            //    InteractableBase interactable = child.GetComponent<InteractableBase>();
            //    if (interactable != null && !interactable.HideInComputer())
            //    {
            //        remoteAvtivation.Add(interactable);
            //    }
            //}
        }
    }

    private void Start()
    {
        if (!Application.isPlaying)
        {
            Debug.Log("Adding new room");
            //roomGUID = System.Guid.NewGuid().ToString();
            FacilityController facilityController = FindObjectOfType<FacilityController>();
            if (facilityController != null) facilityController.ReimportRooms();
            else Debug.LogError("No facility controller");
        }
    }

    private void OnDestroy()
    {
        FacilityController facilityController = FindObjectOfType<FacilityController>();
        if (facilityController != null) facilityController.ReimportRooms();
        //else Debug.LogError("No facility controller");
    }

    private void Update()
    {
        position = transform.position;
    }

    public MapSegment GetMySegment()
    {
        return mySegment;
    }

    readonly float TIME_BETWEEN_WARNINGS = 5;
    float lastWarning;
    WarningStrength lastWarningStrength;

    public void SendWarning(WarningStrength strength)
    {
        if(Time.time > lastWarning + TIME_BETWEEN_WARNINGS || (int)strength > (int)lastWarningStrength)
        {
            ComputerUI.DisplayWarningOnAllComputers(this, strength);
            lastWarning = Time.time;
            lastWarningStrength = strength;
        }
    }

    public void AddToInteractable(IInteractable interactable)
    {
        if (interactable is PoweredInteractable)
        {
            PoweredInteractable powered = interactable as PoweredInteractable;
            if (powered.GetSwitchType() == SwitchType.Doors) mySegment.AddDoors(powered);
            else if (powered.GetSwitchType() == SwitchType.Printers) mySegment.AddPrinters(powered);
            else if (powered.GetSwitchType() == SwitchType.Lights) mySegment.AddLight(powered);
            else if (powered.GetSwitchType() == SwitchType.Security) mySegment.AddSecurity(powered);
        }

        if (interactable != null && !interactable.HideInComputer())
        {
            remoteAvtivation.Add(interactable);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 position = transform.position;
        Gizmos.DrawLine(new Vector2(position.x + roomSize.x, position.y + roomSize.y), new Vector2(position.x - roomSize.x, position.y + roomSize.y));
        Gizmos.DrawLine(new Vector2(position.x - roomSize.x, position.y + roomSize.y), new Vector2(position.x - roomSize.x, position.y - roomSize.y));
        Gizmos.DrawLine(new Vector2(position.x - roomSize.x, position.y - roomSize.y), new Vector2(position.x + roomSize.x, position.y - roomSize.y));
        Gizmos.DrawLine(new Vector2(position.x + roomSize.x, position.y - roomSize.y), new Vector2(position.x + roomSize.x, position.y + roomSize.y));
    }
}
