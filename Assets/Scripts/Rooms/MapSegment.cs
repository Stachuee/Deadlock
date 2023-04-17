using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapSegment : MonoBehaviour
{
    [SerializeField]
    public Vector2Int size;
    [SerializeField]
    public string sectorName;
    [SerializeField]
    public Color segmentColor;
    [SerializeField]
    List<Rooms> roomsInSegment;

    [SerializeField] bool unlocked;
    [SerializeField] List<Spawner> spawnersInSegment;

    [SerializeField] bool alwaysPowered;


    [SerializeField] List<GameObject> doors;
    [SerializeField] bool doorsPowered;
    [SerializeField] List<GameObject> security;
    [SerializeField] bool securityPowered;
    [SerializeField] List<GameObject> printers;
    [SerializeField] bool printersPowered;
    [SerializeField] List<GameObject> lights;
    [SerializeField] bool lightsPowered;

    private void Awake()
    {
        GetComponentsInChildren<Rooms>().ToList().ForEach(x =>
        {
            roomsInSegment.Add(x);
            x.SetMySegment(this);
        });
    }

    private void Start()
    {
        if(doors != null) doors.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(doorsPowered));
        if (printers != null) printers.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(printersPowered));
        if (security != null) security.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(securityPowered));
        if (lights != null) lights.ForEach(x => x.GetComponent<PowerInterface>().PowerOn(lightsPowered));
    }

    public void TurnOnOff(SwitchType switchType, bool on)
    {
        if (alwaysPowered) return;
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
                return doorsPowered || alwaysPowered;
            case SwitchType.Printers:
                return printersPowered || alwaysPowered;
            case SwitchType.Security:
                return securityPowered || alwaysPowered;
            case SwitchType.Lights:
                return lightsPowered || alwaysPowered;
        }
        return false;
    }

    public bool CreateFuse()
    {
        return !alwaysPowered;
    }

    public bool IsUnlockerd()
    {
        return unlocked;
    }

    public void UnlockSegment()
    {
        unlocked = true;
        SegmentController.segmentController.UnlockSegment(this);
        spawnersInSegment.ForEach(x => x.ActivateSpanwer());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = segmentColor;
        Vector2 position = transform.position;

        #if UNITY_EDITOR
        UnityEditor.Handles.Label(new Vector3(position.x - size.x, position.y + size.y), sectorName);
        #endif
        Gizmos.DrawLine(new Vector2(position.x + size.x, position.y + size.y), new Vector2(position.x - size.x, position.y + size.y));
        Gizmos.DrawLine(new Vector2(position.x - size.x, position.y + size.y), new Vector2(position.x - size.x, position.y - size.y));
        Gizmos.DrawLine(new Vector2(position.x - size.x, position.y - size.y), new Vector2(position.x + size.x, position.y - size.y));
        Gizmos.DrawLine(new Vector2(position.x + size.x, position.y - size.y), new Vector2(position.x + size.x, position.y + size.y));
    }
}
