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


    List<PowerInterface> doors;
    [SerializeField] bool doorsPowered;
    List<PowerInterface> security;
    [SerializeField] bool securityPowered;
    List<PowerInterface> printers;
    [SerializeField] bool printersPowered;
    List<PowerInterface> lights;
    [SerializeField] bool lightsPowered;

    private void Awake()
    {

        //GetComponentsInChildren<Rooms>().ToList().ForEach(x =>
        //{
        //    roomsInSegment.Add(x);
        //    x.SetMySegment(this);
        //});
    }

    private void Start()
    {
        if(doors != null) doors.ForEach(x => x.PowerOn(doorsPowered));
        if (printers != null) printers.ForEach(x => x.PowerOn(printersPowered));
        if (security != null) security.ForEach(x => x.PowerOn(securityPowered));
        if (lights != null) lights.ForEach(x => x.PowerOn(lightsPowered));
    }

    public void TurnOnOff(SwitchType switchType, bool on)
    {
        if (alwaysPowered) return;
        switch (switchType)
        {
            case SwitchType.Doors:
                doors.ForEach(x => x.PowerOn(on));
                doorsPowered = on;
                break;
            case SwitchType.Printers:
                printers.ForEach(x => x.PowerOn(on));
                printersPowered = on;
                break;
            case SwitchType.Security:
                security.ForEach(x => x.PowerOn(on));
                securityPowered = on;
                break;
            case SwitchType.Lights:
                lights.ForEach(x => x.PowerOn(on));
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

    public void AddLight(PowerInterface light)
    {
        if(lights == null) lights = new List<PowerInterface>();
        this.lights.Add(light);
    }

    public void AddLight(List<PowerInterface> light)
    {
        if (lights == null) lights = new List<PowerInterface>();
        this.lights.AddRange(light);
    }

    public void AddDoors(PowerInterface doors)
    {
        if (this.doors == null) this.doors = new List<PowerInterface>();
        this.doors.Add(doors);
    }

    public void AddDoors(List<PowerInterface> doors)
    {
        if (this.doors == null) this.doors = new List<PowerInterface>();
        this.doors.AddRange(doors);
    }

    public void AddSecurity(PowerInterface security)
    {
        if (this.security == null) this.security = new List<PowerInterface>();
        this.security.Add(security);
    }
    public void AddSecurity(List<PowerInterface> security)
    {
        if (this.security == null) this.security = new List<PowerInterface>();
        this.security.AddRange(security);
    }

    public void AddPrinters(PowerInterface printers)
    {
        if (this.printers == null) this.printers = new List<PowerInterface>();
        this.printers.Add(printers);
    }

    public void AddPrinters(List<PowerInterface> printers)
    {
        if (this.printers == null) this.printers = new List<PowerInterface>();
        this.printers.AddRange(printers);
    }

    public void AddRoom(Rooms tooAdd)
    {
        roomsInSegment.Add(tooAdd);
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
