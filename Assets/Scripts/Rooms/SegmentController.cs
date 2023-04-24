using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentController : MonoBehaviour
{
    public static SegmentController segmentController;

    public List<MapSegment> mapSegments;

    public List<ComputerUI> playersComputers;

    private void Awake()
    {
        if (segmentController == null) segmentController = this;
        else Debug.LogError("Two segmentControllers");
    }

    public void UnlockSegment(MapSegment segment, bool unlocked)
    {
        playersComputers.ForEach(p => p.UnlockSegment(segment, unlocked));
    }

    public void SubscribeToUnlock(ComputerUI computer)
    {
        playersComputers.Add(computer);
    }
}
