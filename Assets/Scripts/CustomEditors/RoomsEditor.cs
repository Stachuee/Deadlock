using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Rooms))]
[CanEditMultipleObjects]
public class RoomsEditor : Editor
{


    void OnSceneGUI()
    {
        Rooms room = target as Rooms;
        if (room == null || room.gameObject == null) return;

        //Vector2 mouse = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

        FacilityController facilityController = FindObjectOfType<FacilityController>();


    }
}
