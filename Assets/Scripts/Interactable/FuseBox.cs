using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : InteractableBase
{

    PlayerController activePlayer;

    [SerializeField] SwitchType type;

    [SerializeField] bool useCells;


    //[SerializeField] int powerStrength;
    [SerializeField] int currentPowerConsumption;
    protected override void Awake()
    {
        base.Awake();
        AddAction(OpenBox);
    }

    //private void Start()
    //{
    //    if (!useCells)
    //    {
    //        //powerStrength = 2;
    //    }
    //}


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
    
    public SwitchType GetSwitchType()
    {
        return type;
    }



    //public void UpdateFuse(string segment, bool value)
    //{
    //    if (segmentPowered[segment] != value)
    //    {
    //        if (value)
    //        {
    //            currentPowerConsumption++;
    //            ElectricityController.fusesActive++;
    //        }
    //        else
    //        {
    //            currentPowerConsumption--;
    //            ElectricityController.fusesActive--;
    //        }
    //    }
    //    segmentPowered[segment] = value;
    //    SegmentController.segmentController.mapSegments.ForEach(x => x.TurnOnOff(type, GetFuseStatus(x.sectorName)));
    //}

    //public void PlugIn(int power)
    //{
    //    powerStrength = power;
    //    if (currentPowerConsumption > powerStrength)
    //    {
    //        SegmentController.segmentController.mapSegments.ForEach(seg =>
    //        {
    //            seg.TurnOnOff(type, false);
    //        });
    //    }
    //    else
    //    {
    //        SegmentController.segmentController.mapSegments.ForEach(seg =>
    //        {
    //            seg.TurnOnOff(type, GetFuseStatus(seg.sectorName));
    //            //Debug.Log(seg.name + " " + GetFuseStatus(seg.sectorName));
    //        });
    //    }
    //}
    //public float GetPowerConsumption()
    //{
    //    return powerStrength > 0 ? (float) currentPowerConsumption / (float) powerStrength : 0;
    //}
}
