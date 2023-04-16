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
    }

    public void Pressed()
    {
        if(Active) computer.ChangeCamera(room);
    }

    public void Unlock()
    {
        Active = true;
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
