using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMarker : MonoBehaviour
{
    public enum DoorOrientation {Horizontal, Vertical }

    [SerializeField]
    DoorOrientation doorOrientation;

    [SerializeField]
    float doorSize;

    public float GetDoorSize()
    {
        return doorSize;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (doorOrientation == DoorOrientation.Horizontal)
        {
            Gizmos.DrawLine(transform.position + new Vector3(-doorSize/2, -0.2f), transform.position + new Vector3(doorSize / 2, 0.2f));
            Gizmos.DrawLine(transform.position + new Vector3(-doorSize / 2, 0.2f), transform.position + new Vector3(doorSize / 2, -0.2f));
        }
        else
        {
            Gizmos.DrawLine(transform.position + new Vector3(-0.2f, -doorSize / 2), transform.position + new Vector3(0.2f, doorSize / 2));
            Gizmos.DrawLine(transform.position + new Vector3(0.2f, -doorSize / 2), transform.position + new Vector3(-0.2f, doorSize / 2));
        }
    }
}
