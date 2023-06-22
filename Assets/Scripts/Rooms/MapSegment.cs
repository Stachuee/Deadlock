using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapSegment : MonoBehaviour, DangerLevelIncrease
{
    public static MapSegment scientistSegment;

    [SerializeField]
    public Vector2Int size;
    [SerializeField]
    public string sectorName;
    [SerializeField]
    public Color segmentColor;
    [SerializeField] int activateAt;
    public bool broken;
    bool active;

    [SerializeField]
    List<Rooms> roomsInSegment;
    [SerializeField]
    bool scientistSeg;


    [SerializeField] bool segmentUnlocked;

    [SerializeField] bool createFuse;

    bool locked;

    List<PowerInterface> doors;
    [SerializeField] bool doorsPowered;
    List<PowerInterface> security;
    [SerializeField] bool securityPowered;
    List<PowerInterface> printers;
    [SerializeField] bool printersPowered;
    List<PowerInterface> lights;
    [SerializeField] bool lightsPowered;

    private void Start()
    {
        if (doors != null) doors.ForEach(x => x.PowerOn(doorsPowered, sectorName));
        else doors = new List<PowerInterface>();
        if (printers != null) printers.ForEach(x => x.PowerOn(printersPowered, sectorName));
        else printers = new List<PowerInterface>();
        if (security != null) security.ForEach(x => x.PowerOn(securityPowered, sectorName));
        else security = new List<PowerInterface>();
        if (lights != null) lights.ForEach(x => x.PowerOn(lightsPowered, sectorName));
        else lights = new List<PowerInterface>();

        if (scientistSeg) scientistSegment = this;

        PacingController.pacingController.AddToNotify(this);
    }


    public void TurnOnOff(SwitchType switchType, bool on)
    {
        if (locked) return;
        switch (switchType)
        {
            case SwitchType.Doors:
                doors.ForEach(x => x.PowerOn(on, sectorName));
                doorsPowered = on;
                break;
            case SwitchType.Printers:
                printers.ForEach(x => x.PowerOn(on, sectorName));
                printersPowered = on;
                break;
            case SwitchType.Security:
                security.ForEach(x => x.PowerOn(on, sectorName));
                securityPowered = on;
                break;
        }

    }

    public void OverloadSegment(bool overloaded)
    {
        if (overloaded)
        {
            TurnOnOff(SwitchType.Printers, false);
            TurnOnOff(SwitchType.Doors, false);
            TurnOnOff(SwitchType.Security, false);
            TurnOnOffLights(!overloaded);
            locked = true;
        }
        else
        {
            TurnOnOffLights(!overloaded);
            locked = false;
        }
    }

    public void TurnOnOffLights(bool on)
    {
        lights.ForEach(x => x.PowerOn(on, sectorName));
        lightsPowered = on;
    }

    public bool GetPowerStatus(SwitchType switchType)
    {
        switch (switchType)
        {
            case SwitchType.Doors:
                return doorsPowered || !createFuse;
            case SwitchType.Printers:
                return printersPowered || !createFuse;
            case SwitchType.Security:
                return securityPowered || !createFuse;
        }
        return false;
    }

    public void Breakelectricity()
    {
        broken = true;
        TurnOnOffLights(false);
    }

    public void ScientistSegmentUnlock()
    {
        TurnOnOff(SwitchType.Printers, true);
        TurnOnOff(SwitchType.Security, true);
        TurnOnOff(SwitchType.Doors, true);
    }

    public bool CreateFuse()
    {
        return createFuse;
    }

    public bool IsUnlockerd()
    {
        return segmentUnlocked;
    }

    public void UnlockSegment(bool unlocked)
    {
        segmentUnlocked = unlocked;
        SegmentController.segmentController.UnlockSegment(this, unlocked);
    }

    public void AddLight(PowerInterface light)
    {
        if(lights == null) lights = new List<PowerInterface>();
        this.lights.Add(light);
        //light.PowerOn(lightsPowered || !createFuse, sectorName); // temp fix
    }

    public void AddLight(List<PowerInterface> light)
    {
        if (lights == null) lights = new List<PowerInterface>();
        this.lights.AddRange(light);
        //lights.ForEach(x => x.PowerOn(lightsPowered || !createFuse, sectorName)); // temp fix
    }

    public void AddDoors(PowerInterface doors)
    {
        if (this.doors == null) this.doors = new List<PowerInterface>();
        this.doors.Add(doors);
        doors.PowerOn(doorsPowered || !createFuse, sectorName);
    }

    public void AddDoors(List<PowerInterface> doors)
    {
        if (this.doors == null) this.doors = new List<PowerInterface>();
        this.doors.AddRange(doors);
        doors.ForEach(x => x.PowerOn(doorsPowered || !createFuse, sectorName));
    }

    public void AddSecurity(PowerInterface security)
    {
        if (this.security == null) this.security = new List<PowerInterface>();
        this.security.Add(security);
        security.PowerOn(securityPowered || !createFuse, sectorName);
    }
    public void AddSecurity(List<PowerInterface> security)
    {
        if (this.security == null) this.security = new List<PowerInterface>();
        this.security.AddRange(security);
        security.ForEach(x => x.PowerOn(securityPowered || !createFuse, sectorName));
    }

    public void AddPrinters(PowerInterface printers)
    {
        if (this.printers == null) this.printers = new List<PowerInterface>();
        this.printers.Add(printers);
        printers.PowerOn(printersPowered || !createFuse, sectorName);
    }

    public void AddPrinters(List<PowerInterface> printers)
    {
        if (this.printers == null) this.printers = new List<PowerInterface>();
        this.printers.AddRange(printers);
        printers.ForEach(x => x.PowerOn(printersPowered || !createFuse, sectorName));
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

    public void IncreaseLevel(int level)
    {
        if (level == activateAt)
        {
            UnlockSegment(true);
            TurnOnOffLights(true);
        }
        
    }    
}
