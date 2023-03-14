using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerUI : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject roomPrefabUI;
    [SerializeField] GameObject segmentPrefab;

    [SerializeField] PlayerController playerController;

    [SerializeField] int scale;

    List<RoomUiButton> roomsUI = new List<RoomUiButton>();

    bool lookingAtMap;

    private void Start()
    {
        EventManager.eventManager.AddlLstener(this);

        RectTransform myRect = transform.GetComponent<RectTransform>();

        Rooms starting = FacilityController.facilityController.allRooms.Find(x => x.startingRoom);

        List<Rooms> allRooms = new List<Rooms>(FacilityController.facilityController.allRooms);

        Vector2 startingPos = starting.transform.position;

        Vector2Int size = new Vector2Int(0,0); // x, y


        SegmentController.segmentController.mapSegments.ForEach(segment =>
        {
            RectTransform temp = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>();

            temp.anchoredPosition = ((Vector2)segment.transform.position - startingPos) * scale;
            temp.sizeDelta = new Vector2(segment.size.x, segment.size.y) * scale * 2;

            temp.GetComponent<Image>().color = segment.segmentColor;
        });

        allRooms.ForEach(roomToDraw =>
        {
            RectTransform temp = Instantiate(roomPrefabUI, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>();

            RoomUiButton button = temp.GetComponent<RoomUiButton>();
            button.SetUp(roomToDraw, this);

            roomsUI.Add(button);
            
            temp.anchoredPosition = ((Vector2)roomToDraw.transform.position - startingPos) * scale;
            if (size.x < Mathf.Abs(temp.anchoredPosition.x)) size.x = Mathf.CeilToInt(Mathf.Abs(temp.anchoredPosition.x)) + roomToDraw.roomSize.x * scale;
            if (size.y < Mathf.Abs(temp.anchoredPosition.y)) size.y = Mathf.CeilToInt(Mathf.Abs(temp.anchoredPosition.y)) + roomToDraw.roomSize.y * scale;
            temp.sizeDelta = new Vector2(roomToDraw.roomSize.x, roomToDraw.roomSize.y) * scale;
        });

        size *= 2;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(size.x, myRect.sizeDelta.x), Mathf.Max(size.y, myRect.sizeDelta.y));
    }


    public void ChangeCamera(Rooms room)
    {
        playerController.cameraController.ChangeTarget(room.transform);
        panel.SetActive(false);
        lookingAtMap = false;
    }

    public void OpenComputer()
    {
        panel.SetActive(true);
        lookingAtMap = true;
    }

    public bool Back()
    {
        playerController.cameraController.ResetTarget();
        if (lookingAtMap)
        {
            panel.SetActive(false);
            return true;
        }
        else
        {
            panel.SetActive(true);
            lookingAtMap = true;
            return false;
        }
    }

    public void UpdateEvent(RoomEvent eventToUpdate, bool isNew)
    {
        roomsUI.Find(x => x.room == eventToUpdate.room).UpdateEvent(isNew);
        //Debug.Log(eventToUpdate.roomGUID);
        //roomsUI.ForEach(x => Debug.Log(x.roomGUID));
        //Debug.Log(roomsUI.Find(x => x.roomGUID == eventToUpdate.roomGUID));
        //roomsUI.Find(x => x.roomGUID == eventToUpdate.roomGUID).UpdateEvent(isNew);
    }
}
