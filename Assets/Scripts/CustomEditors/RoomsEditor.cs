using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Rooms))]
[CanEditMultipleObjects]
public class RoomsEditor : Editor
{
    readonly float snapingStrength = 1;

    void OnSceneGUI()
    {
        Rooms room = target as Rooms;
        if (room == null || room.gameObject == null) return;

        //Vector2 mouse = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

        FacilityController facilityController = FindObjectOfType<FacilityController>();


        for(int i = 0; i < room.Doors.Length; i++)
        {
            string lastConnection = room.Doors[i].connectedGUID;
            if (lastConnection != "")
            {
                room.Doors[i].connectedGUID = "";
                int index = facilityController.allDoors.FindIndex(x => x.myGUID == lastConnection);
                Door door = facilityController.allDoors[index];
                door.connectedGUID = "";
                facilityController.allDoors[index] = door;
            }

        }


        foreach (Rooms roomToCheck in facilityController.allRooms)
        {
            if (roomToCheck == room) continue;
            if(roomToCheck.RoomSize.x + room.RoomSize.x - Mathf.Abs(roomToCheck.Position.x - room.Position.x) > -snapingStrength && roomToCheck.RoomSize.y + room.RoomSize.y - Mathf.Abs(roomToCheck.Position.y - room.Position.y) > -snapingStrength)
            {
                for (int j = 0; j < room.Doors.Length; j++)
                {
                    for (int i = 0; i < roomToCheck.Doors.Length; i++)
                    {
                        //Handles.DrawLine(roomToCheck.Doors[i].doorPosition + roomToCheck.Position, room.Doors[j].doorPosition + room.Position);
                        if (Vector2.Distance(roomToCheck.Doors[i].doorPosition + roomToCheck.Position, room.Doors[j].doorPosition + room.Position) < 1)
                        {
                            roomToCheck.Doors[i].connectedGUID = room.Doors[j].myGUID;
                            room.Doors[j].connectedGUID = roomToCheck.Doors[i].myGUID;

                            room.Position = roomToCheck.Position + roomToCheck.Doors[i].doorPosition - room.Doors[j].doorPosition;
                            continue;
                        }

                    }
                }
            }
        }
    }
}
