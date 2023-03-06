using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct Door
{
    //public enum ConnectionDirection {Top, Bot, Left, Right }
    public Vector2 doorPosition;
    public string myGUID;
    public string connectedGUID;
    public string roomGUID;
    //public ConnectionDirection connectTo;
}

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
    Door[] doors;
    public Door[] Doors
    {
        set
        {
            doors = value;
        }
        get
        {
            return doors;
        }
    }
    [SerializeField]
    Interactable[] interactable;
    public Interactable[] Interactable
    {
        set
        {
            interactable = value;
        }
        get
        {
            return interactable;
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


    private void Start()
    {
        if (!Application.isPlaying)
        {
            roomGUID = System.Guid.NewGuid().ToString();
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].myGUID = System.Guid.NewGuid().ToString();
                doors[i].connectedGUID = "";
                doors[i].roomGUID = roomGUID;
            }
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
        Gizmos.color = Color.red;
        foreach(Door door in doors)
        {
            Gizmos.DrawWireSphere(door.doorPosition + (Vector2)transform.position, 0.4f);
        }
        Gizmos.color = Color.green;
        foreach (Interactable interactable in interactable)
        {
            Gizmos.DrawWireSphere(interactable.interactablePosition + (Vector2)transform.position, 0.4f);
        }
        Gizmos.color = Color.blue;
        Vector2 position = transform.position;
        Gizmos.DrawLine(new Vector2(position.x + roomSize.x, position.y + roomSize.y), new Vector2(position.x - roomSize.x, position.y + roomSize.y));
        Gizmos.DrawLine(new Vector2(position.x - roomSize.x, position.y + roomSize.y), new Vector2(position.x - roomSize.x, position.y - roomSize.y));
        Gizmos.DrawLine(new Vector2(position.x - roomSize.x, position.y - roomSize.y), new Vector2(position.x + roomSize.x, position.y - roomSize.y));
        Gizmos.DrawLine(new Vector2(position.x + roomSize.x, position.y - roomSize.y), new Vector2(position.x + roomSize.x, position.y + roomSize.y));
    }
}
