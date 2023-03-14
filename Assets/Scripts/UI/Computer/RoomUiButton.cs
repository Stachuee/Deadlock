using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUiButton : MonoBehaviour
{
    public Rooms room { get; private set; }
    ComputerUI computer;
    [SerializeField]
    public Image myImage;


    public void SetUp(Rooms room, ComputerUI computer)
    {
        this.room = room;
        this.computer = computer;
    }

    public void Pressed()
    {
        computer.ChangeCamera(room);
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
