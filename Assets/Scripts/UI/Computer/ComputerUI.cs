using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerUI : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] GameObject roomPrefabUI;

    [SerializeField] float scale;

    private void Start()
    {
        List<Rooms> closed = new List<Rooms>();
        List<Rooms> open = new List<Rooms>();
        Rooms starting = new List<Rooms>(FindObjectsOfType<Rooms>()).Find(x => x.startingRoom);

        Vector2 startingPos = starting.transform.position;

        open.Add(starting);

        while(open.Count > 0)
        {
            Rooms roomToDraw = open[0];
            open.Remove(roomToDraw);
            closed.Add(roomToDraw);

            RectTransform temp = Instantiate(roomPrefabUI, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>();
            temp.anchoredPosition = ((Vector2)roomToDraw.transform.position - startingPos) * scale;
            temp.localScale = new Vector3(roomToDraw.roomSize.x, roomToDraw.roomSize.y, 1) * scale;

            Debug.Log(roomToDraw.Doors.Length + " " + roomToDraw.Doors[0].roomGUID);

            for(int i = 0; i < roomToDraw.Doors.Length; i++)
            {
                string connectetGuid = roomToDraw.Doors[i].connectedGUID;
                string roomGuid = FacilityController.facilityController.allDoors.Find(x => x.myGUID == connectetGuid).roomGUID;
                if(!(closed.Find(x => x.roomGUID == roomGuid) || open.Find(x => x.roomGUID == roomGuid)))
                {
                    open.Add(FacilityController.facilityController.allRooms.Find(x => x.roomGUID == roomGuid));
                }
            }
        }
    }

}
