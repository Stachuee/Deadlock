using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : InteractableBase
{

    PlayerController activePlayer;

    [SerializeField] SwitchType type;

    Dictionary<string, bool> segmentPowered = new Dictionary<string, bool>();

    bool powerToBox;

    protected override void Awake()
    {
        base.Awake();
        AddAction(OpenBox);
    }

    private void Start()
    {
        SegmentController.segmentController.mapSegments.ForEach(x => segmentPowered.Add(x.sectorName, x.GetPowerStatus(type)));
    }


    void OpenBox(PlayerController player)
    {
        activePlayer = player;
        player.uiController.fuseBox.OpenBox(type, this);
        player.LockInAction(CloseBox);
    }

    void CloseBox()
    {
        activePlayer.uiController.fuseBox.CloseBox();
        activePlayer.UnlockInAnimation();
    }

    public void UpdateFuse(string segment, bool value)
    {
        segmentPowered[segment] = value;

        MapSegment foundSegment = SegmentController.segmentController.mapSegments.Find(x => x.sectorName == segment);
        foundSegment.TurnOnOff(type, GetFuseStatus(segment));
    }

    public bool GetFuseStatus(string segment)
    {
        return powerToBox && segmentPowered[segment];
    }

    public void PlugIn(bool on)
    {
        powerToBox = on;
        if (!on)
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
}
