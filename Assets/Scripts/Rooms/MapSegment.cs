using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapSegment : MonoBehaviour
{
    [SerializeField]
    public Vector2Int size;
    [SerializeField]
    public string sectorName;
    [SerializeField]
    Color segmentColor;


    [SerializeField] List<GameObject> doors;
    [SerializeField] List<GameObject> security;
    [SerializeField] List<GameObject> printers;
    [SerializeField] List<GameObject> lighs;

    public void TurnOnOff(SwitchType switchType, bool on)
    {
        switch (switchType)
        {
            case SwitchType.Doors:
                doors.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(on));
                break;
            case SwitchType.Printers:
                printers.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(on));
                break;
            case SwitchType.Security:
                security.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(on));
                break;
            case SwitchType.Lights:
                lighs.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(on));
                break;
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = segmentColor;
        Vector2 position = transform.position;

        Handles.Label(new Vector3(position.x - size.x, position.y + size.y), sectorName);
        Gizmos.DrawLine(new Vector2(position.x + size.x, position.y + size.y), new Vector2(position.x - size.x, position.y + size.y));
        Gizmos.DrawLine(new Vector2(position.x - size.x, position.y + size.y), new Vector2(position.x - size.x, position.y - size.y));
        Gizmos.DrawLine(new Vector2(position.x - size.x, position.y - size.y), new Vector2(position.x + size.x, position.y - size.y));
        Gizmos.DrawLine(new Vector2(position.x + size.x, position.y - size.y), new Vector2(position.x + size.x, position.y + size.y));
    }
}
