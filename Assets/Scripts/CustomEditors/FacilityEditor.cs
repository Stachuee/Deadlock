using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(FacilityController))]
public class FacilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FacilityController controller = target as FacilityController;
        if (controller == null || controller.gameObject == null) return;

        if(GUILayout.Button("GetMapTexture"))
        {
            controller.GetMapTexture();
        }
    }
}
