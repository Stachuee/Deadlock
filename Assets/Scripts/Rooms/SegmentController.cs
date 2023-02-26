using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentController : MonoBehaviour
{
    public static SegmentController segmentController;

    public List<MapSegment> mapSegments;

    private void Awake()
    {
        if (segmentController == null) segmentController = this;
        else Debug.LogError("Two segmentControllers");
    }
}
