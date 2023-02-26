using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBoxUIScript : MonoBehaviour
{
    [SerializeField] List<Button> fuses;

    [SerializeField] Vector2 offset;
    [SerializeField] float fuseHeight;

    [SerializeField] GameObject fusePrefab;



    SwitchType openType;

    private void Start()
    {
        int segmenCount = SegmentController.segmentController.mapSegments.Count;
        float fuzeOffset = fuseHeight / (segmenCount - 1);
        for(int i = 0; i < segmenCount; i++)
        {
            Fuse temp = Instantiate(fusePrefab, (Vector2)transform.position + offset + new Vector2(0, fuseHeight / 2) - new Vector2(0, fuzeOffset * i), Quaternion.identity, transform).GetComponent<Fuse>();
            temp.SetFuse(SegmentController.segmentController.mapSegments[i].sectorName, this);
        }
    }

    public void OpenBox(SwitchType openType)
    {
        this.openType = openType;
        gameObject.SetActive(true);
    }
    public void CloseBox()
    {
        gameObject.SetActive(false);
    }

    public void SwitchFuse(string segmentName, bool on)
    {
        MapSegment segment = SegmentController.segmentController.mapSegments.Find(x => x.sectorName == segmentName);
        segment.TurnOnOff(openType, on);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine((Vector2)transform.position + offset + new Vector2(0, fuseHeight / 2), (Vector2)transform.position + offset - new Vector2(0, fuseHeight / 2));
    }
}
