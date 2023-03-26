using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBoxUIScript : MonoBehaviour
{
    [SerializeField] List<Fuse> fuses;

    [SerializeField] Vector2 offset;
    [SerializeField] float fuseHeight;

    [SerializeField] GameObject fusePrefab;

    [SerializeField] PlayerController playerController;

    [SerializeField] Transform fuseBoxUI;

    FuseBox openFuseBox;

    SwitchType openType;

    bool setUp;

    public void Setup()
    {
        int segmenCount = SegmentController.segmentController.mapSegments.Count;
        float fuzeOffset = fuseHeight / (segmenCount - 1);
        for(int i = 0; i < segmenCount; i++)
        {
            Fuse temp = Instantiate(fusePrefab, (Vector2)transform.position + offset + new Vector2(0, fuseHeight / 2) - new Vector2(0, fuzeOffset * i), Quaternion.identity, fuseBoxUI).GetComponent<Fuse>();
            temp.SetFuse(SegmentController.segmentController.mapSegments[i].sectorName, this);
            fuses.Add(temp);
        }
        setUp = true;
    }

    public void OpenBox(SwitchType openType, FuseBox boxOpen)
    {
        if (!setUp) Setup();
        this.openType = openType;

        openFuseBox = boxOpen;

        fuses.ForEach(x =>
        {
            x.TurnFuse(boxOpen.GetFuseStatus(x.segmentName));
        });
        fuseBoxUI.gameObject.SetActive(true);
        playerController.uiController.myEventSystem.SetSelectedGameObject(fuses[0].gameObject);
    }
    public void CloseBox()
    {
        fuseBoxUI.gameObject.SetActive(false);
    }

    public void SwitchFuse(string segmentName, bool on)
    {
        openFuseBox.UpdateFuse(segmentName, on);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine((Vector2)transform.position + offset + new Vector2(0, fuseHeight / 2), (Vector2)transform.position + offset - new Vector2(0, fuseHeight / 2));
    }
}
