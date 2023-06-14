using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityController : MonoBehaviour
{
    public static ElectricityController Instance { get; private set; }
    public static bool Overloaded { get; private set; }

    public static float overcharge;
    public static float maxOvercharge;
    public static int fusesActive;
    public static int maxFusesActive;

    public static Dictionary<string, bool> turretsPowered = new Dictionary<string, bool>();
    public static Dictionary<string, bool> doorsPowered = new Dictionary<string, bool>();
    public static Dictionary<string, bool> printersPowered = new Dictionary<string, bool>();
    public static bool workshopPowered;

    [SerializeField] float overloadDuration;
    float overloadEnd;
    [SerializeField] float _maxOvercharge;
    [SerializeField] float overchargeFalloff;
    [SerializeField] int _maxFusesActive;
    [SerializeField] float[] overchargePerAdditionaFuse = {1, 1.25f, 1.5f, 1.75f, 2f};

    [SerializeField] AudioSource electricityOffSFX;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        maxOvercharge = _maxOvercharge;
        maxFusesActive = _maxFusesActive;
    }

    private void Start()
    {
        SegmentController.segmentController.mapSegments.ForEach(x => {
            turretsPowered.Add(x.sectorName, x.GetPowerStatus(SwitchType.Security));
            doorsPowered.Add(x.sectorName, x.GetPowerStatus(SwitchType.Doors));
            printersPowered.Add(x.sectorName, x.GetPowerStatus(SwitchType.Printers));
        });
    }

    private void Update()
    {
        if(fusesActive > maxFusesActive)
        {
            overcharge += overchargePerAdditionaFuse[Mathf.Min((fusesActive - 1) - maxFusesActive, overchargePerAdditionaFuse.Length - 1)] * Time.deltaTime;
            if(overcharge > maxOvercharge)
            {
                Overload(true);
            }
        }

        if(Overloaded && overloadEnd <= Time.time)
        {
            Overloaded = false;
            Overload(false);
        }

        overcharge = Mathf.Clamp(overcharge - overchargeFalloff * Time.deltaTime, 0, maxOvercharge);
    }

    void Overload(bool value)
    {
        if(value)
        {
            overloadEnd = Time.time + overloadDuration;
            electricityOffSFX.Play();

            SegmentController.segmentController.mapSegments.ForEach(segment =>
            {
                doorsPowered[segment.sectorName] = false;
                turretsPowered[segment.sectorName] = false;
                printersPowered[segment.sectorName] = false;
                workshopPowered = false;
                fusesActive = 0;
            });

            PlayerController.scientist.uiController.fuseBox.RefreshFuses();
            PlayerController.scientist.uiController.workshopFuse.RefreshFuse();
        }
        Overloaded = value;
        SegmentController.segmentController.mapSegments.ForEach(segment =>
        {
            segment.OverloadSegment(value);
        });
    }


    public static bool GetFuseStatus(SwitchType type, string segment)
    {
        switch (type)
        {
            case SwitchType.Doors:
                return doorsPowered[segment];
            case SwitchType.Security:
                return turretsPowered[segment];
            case SwitchType.Printers:
                return printersPowered[segment];
        }
        return false;
    }

    public static void SetFuseStatus(SwitchType type, string segment, bool value)
    {
        if (Overloaded) return; 
        if (GetFuseStatus(type, segment) != value)
        {
            if (value)
            {
                fusesActive++;
            }
            else
            {
                fusesActive--;
            }
        }

        switch (type)
        {
            case SwitchType.Doors:
                doorsPowered[segment] = value;
                break;
            case SwitchType.Security:
                turretsPowered[segment] = value;
                break;
            case SwitchType.Printers:
                printersPowered[segment] = value;
                break;
        }
    }

    public static void SetWorkshopFuse(bool value)
    {
        if(workshopPowered != value)
        {
            if (value)
            {
                fusesActive++;
            }
            else
            {
                fusesActive--;
            }
            workshopPowered = value;
        }
    }


    public static void UpdateFuse(string segment, SwitchType type, bool value)
    {
        SetFuseStatus(type, segment, value);
        SegmentController.segmentController.mapSegments.ForEach(x => x.TurnOnOff(type, GetFuseStatus(type, x.sectorName)));
    }
}
