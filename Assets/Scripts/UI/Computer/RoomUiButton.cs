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

        myImage.sprite = room.mySprite;

        room.remoteAvtivation.ForEach(x =>
        {
            if(!x.HideInComputer())
            {
                Debug.Log(x.GetTransform().name);
                GameObject temp = Instantiate(x.GetComputerIcon(), transform);
                RectTransform tempTransform = temp.GetComponent<RectTransform>();
                tempTransform.anchoredPosition = (x.GetPosition() - room.Position) * ComputerUI.scale;
            }
        });
    }

    public void Pressed()
    {
        if (Active)
        {
            computer.ChangeCamera(room);
            UpdateEvent(false, WarningStrength.Strong);
        }
    }

    public void Unlock(bool unlocked)
    {
        Active = unlocked;
    }

    public void UpdateEvent(bool highlight, WarningStrength strength)
    {
        if(highlight)
        {
            switch (strength)
            {
                case WarningStrength.Weak:
                    myImage.color = Color.yellow;
                    break;
                    case WarningStrength.Medium:
                    myImage.color = new Color(255, 165, 0);
                    break;
                case WarningStrength.Strong:
                    myImage.color = Color.red;
                    break;
            }
            
        }
        else
        {
            myImage.color = Color.white;
        }
    }
}
