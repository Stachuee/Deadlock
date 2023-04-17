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
    [SerializeField]
    public string roomGUID;
    [SerializeField]
    public Vector2Int roomSize;
    [SerializeField]
    public bool startingRoom;
    [SerializeField]
    public List<RoomEvent> roomEvents;

    [SerializeField]
    public List<InteractableBase> remoteAvtivation { get; private set; }
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
        remoteAvtivation = new List<InteractableBase>();
        doorMarkers = new List<DoorMarker>();
        stairs = new List<StairsScript>();
        stairs = transform.GetComponentsInChildren<StairsScript>().ToList();
        doorMarkers = transform.GetComponentsInChildren<DoorMarker>().ToList();

        roomGUID = System.Guid.NewGuid().ToString();
        foreach(Transform child in transform)
        {
            InteractableBase interactable = child.GetComponent<InteractableBase>();
            if(interactable != null && interactable.IsRemote())
            {
                remoteAvtivation.Add(interactable);
            }
        }
    }

    private void Start()
    {
        if (!Application.isPlaying)
        {
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


    public void SetMySegment(MapSegment segment)
    {
        mySegment = segment;
    }

    public MapSegment GetMySegment()
    {
        return mySegment;
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
