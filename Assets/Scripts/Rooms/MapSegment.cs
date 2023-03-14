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
    public Color segmentColor;


    [SerializeField] List<GameObject> doors;
    [SerializeField] bool doorsPowered;
    [SerializeField] List<GameObject> security;
    [SerializeField] bool securityPowered;
    [SerializeField] List<GameObject> printers;
    [SerializeField] bool printersPowered;
    [SerializeField] List<GameObject> lights;
    [SerializeField] bool lightsPowered;


    private void Start()
    {
        doors.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(doorsPowered));
        printers.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(printersPowered));
        security.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(securityPowered));
        lights.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(lightsPowered));
    }

    public void TurnOnOff(SwitchType switchType, bool on)
    {
        switch (switchType)
        {
            case SwitchType.Doors:
                doors.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(on));
                doorsPowered = on;
                break;
            case SwitchType.Printers:
                printers.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(on));
                printersPowered = on;
                break;
            case SwitchType.Security:
                security.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(on));
                securityPowered = on;
                break;
            case SwitchType.Lights:
                lights.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(on));
                lightsPowered = on;
                break;
        }

    }

    public bool GetPowerStatus(SwitchType switchType)
    {
        switch (switchType)
        {
            case SwitchType.Doors:
                return doorsPowered;
            case SwitchType.Printers:
                return printersPowered;
            case SwitchType.Security:
                return securityPowered;
            case SwitchType.Lights:
                return lightsPowered;
        }
        return false;
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
