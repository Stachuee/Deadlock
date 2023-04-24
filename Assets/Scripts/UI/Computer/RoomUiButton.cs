using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUiButton : MonoBehaviour
{
    public Rooms room { get; private set; }
    public MapSegment segment { get; private set; }
    ComputerUI computer;
    [SerializeField]
    public Image myImage;

    [SerializeField]
    GameObject IconPrefab;
    [SerializeField]
    Vector2 iconOffset;

    bool Active
    {
        get
        {
            return active;
        }
        set
        {
            if(value)
            {
                myImage.color = Color.white;
            }
            else
            {
                myImage.color = Color.grey;
            }
            active = value;
        }
    }
    bool active;


    public void SetUp(Rooms room, ComputerUI computer)
    {
        Active = room.GetMySegment().IsUnlockerd();
        this.room = room;
        this.segment = room.GetMySegment();
        this.computer = computer;

        int count = 0;

        room.remoteAvtivation.ForEach(x =>
        {
            GameObject temp = Instantiate(IconPrefab, transform);
            RectTransform tempTransform = temp.GetComponent<RectTransform>();
            tempTransform.anchoredPosition = iconOffset + new Vector2((RoomIcon.ICON_SIZE + RoomIcon.ICON_OFFSET) * count * -1, 0 );
            temp.GetComponent<Image>().sprite = x.GetComputerIcon();
            count++;
        });
    }

    public void Pressed()
    {
        if(Active) computer.ChangeCamera(room);
    }

    public void Unlock(bool unlocked)
    {
        Active = unlocked;
    }


    public void UpdateEvent(bool highlight)
    {
        if(highlight)
        {
            myImage.color = Color.red;
        }
        else
        {
            myImage.color = Color.white;
        }
    }
}
