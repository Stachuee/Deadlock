using System.Collections;
using System.Collections.Generic;
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
        roomGUID = System.Guid.NewGuid().ToString();
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
        else Debug.LogError("No facility controller");
    }

    private void Update()
    {
        position = transform.position;
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
