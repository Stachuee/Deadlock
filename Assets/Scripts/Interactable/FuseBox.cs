using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : InteractableBase
{

    PlayerController activePlayer;

    [SerializeField] SwitchType type;

    Dictionary<string, bool> segmentPowered = new Dictionary<string, bool>();

    [SerializeField] int powerStrength;
    [SerializeField] int currentPowerConsumption;

    protected override void Awake()
    {
        base.Awake();
        AddAction(OpenBox);
    }

    private void Start()
    {
        SegmentController.segmentController.mapSegments.ForEach(x => segmentPowered.Add(x.sectorName, x.GetPowerStatus(type)));
    }


    void OpenBox(PlayerController player, UseType type)
    {
        activePlayer = player;
        player.uiController.fuseBox.OpenBox(this.type, this);
        player.LockInAction(CloseBox);
    }

    void CloseBox()
    {
        activePlayer.uiController.fuseBox.CloseBox();
        activePlayer.UnlockInAnimation();
    }

    public void UpdateFuse(string segment, bool value)
    {
        if(segmentPowered[segment] != value)
        {
            if (value) currentPowerConsumption++;
            else currentPowerConsumption--;
        }
        segmentPowered[segment] = value;
        SegmentController.segmentController.mapSegments.ForEach(x => x.TurnOnOff(type, GetFuseStatus(x.sectorName)));
    }

    public bool GetFuseStatus(string segment)
    {
        return powerStrength >= currentPowerConsumption && segmentPowered[segment];
    }

    public void PlugIn(int power)
    {
        powerStrength = power;
        if (currentPowerConsumption > powerStrength)
        {
            SegmentController.segmentController.mapSegments.ForEach(seg =>
            {
                seg.TurnOnOff(type, false);
            });
        }
        else
        {
            SegmentController.segmentController.mapSegments.ForEach(seg =>
            {
                seg.TurnOnOff(type, GetFuseStatus(seg.sectorName));
                //Debug.Log(seg.name + " " + GetFuseStatus(seg.sectorName));
            });
        }
    }
    public float GetPowerConsumption()
    {
        return powerStrength > 0 ? (float) currentPowerConsumption / (float) powerStrength : 0;
    }
}
