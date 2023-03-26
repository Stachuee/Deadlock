using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerUI : MonoBehaviour, IControllSubscriberMovment, IControllSubscriberUse
{
    [SerializeField] Transform content;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject roomPrefabUI;
    [SerializeField] GameObject segmentPrefab;

    [SerializeField] PlayerController playerController;

    [SerializeField] int scale;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform contentPanel;


    List<RoomUiButton> roomsUI = new List<RoomUiButton>();

    bool lookingAtMap;
    Rooms activeRoom;
    GameObject firstButton;

    bool setUp;

    public void Setup()
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

            if (roomToDraw.startingRoom) firstButton = temp.gameObject;

            roomsUI.Add(button);
            
            temp.anchoredPosition = ((Vector2)roomToDraw.transform.position - startingPos) * scale;
            if (size.x < Mathf.Abs(temp.anchoredPosition.x)) size.x = Mathf.CeilToInt(Mathf.Abs(temp.anchoredPosition.x)) + roomToDraw.roomSize.x * scale;
            if (size.y < Mathf.Abs(temp.anchoredPosition.y)) size.y = Mathf.CeilToInt(Mathf.Abs(temp.anchoredPosition.y)) + roomToDraw.roomSize.y * scale;
            temp.sizeDelta = new Vector2(roomToDraw.roomSize.x, roomToDraw.roomSize.y) * scale;
        });

        size *= 2;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(size.x, myRect.sizeDelta.x), Mathf.Max(size.y, myRect.sizeDelta.y));

        setUp = true;
    }

    Vector2 vel = Vector2.zero;

    private void Update()
    {
        if(lookingAtMap && !playerController.keyboard)
        {
            contentPanel.anchoredPosition = Vector2.SmoothDamp(contentPanel.anchoredPosition, (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
        - (Vector2)scrollRect.transform.InverseTransformPoint(playerController.uiController.myEventSystem.currentSelectedGameObject.transform.position), ref vel, 0.05f);
        }
    }


    struct AngleToInteractable
    {
        public IInteractable interactable;
        public float angle;
    }
    List<AngleToInteractable> angleToInteractables;

    public void ChangeCamera(Rooms room)
    {
        playerController.cameraController.ChangeTarget(room.transform);
        activeRoom = room;
        angleToInteractables = new List<AngleToInteractable>();

        room.remoteAvtivation.ForEach(x => {
            angleToInteractables.Add(new AngleToInteractable() { 
                interactable = x,
                angle = Vector2.SignedAngle(Vector2.one, (x.transform.position - room.transform.position))
            });
        });

        playerController.AddMovmentSubscriber(this);
        playerController.AddUseSubscriber(this);
        panel.SetActive(false);
        lookingAtMap = false;
    }

    public void OpenComputer()
    {
        if(!setUp) Setup();
        panel.SetActive(true);
        lookingAtMap = true;
        playerController.uiController.myEventSystem.SetSelectedGameObject(firstButton);

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
            activeRoom = null;
            playerController.RemoveMovmentSubscriber(this);
            playerController.RemoveUseSubscriber(this);
            Highlighted = null;
            return false;
        }
    }


    IInteractable Highlighted { 
        get 
        {
            return highlighted;
        }
        set
        {
            if(highlighted != null)
            {
                highlighted.UnHighlight();
            }
            if(value != null)
            {
                value.Highlight();
            }
            highlighted = value;
        }
    }

    IInteractable highlighted;

    public void ForwardCommandMovment(Vector2 controll)
    {
        if(controll.magnitude > 0)
        {
            IInteractable newHighlighted = null;
            float smallestAngle = 45;
            float myAngle = Vector2.SignedAngle(Vector2.one, controll);

            angleToInteractables.ForEach(x =>
            {
                float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(myAngle, x.angle));
                if (deltaAngle < smallestAngle)
                {
                    smallestAngle = deltaAngle;
                    newHighlighted = x.interactable;
                }
            });

            if(newHighlighted != Highlighted)
            {
                Highlighted = newHighlighted;
            }
        }
    }
    public void ForwardCommandUse()
    {
        if(Highlighted != null)
        {
            Highlighted.Use(playerController);
        }
    }

    public void UpdateEvent(RoomEvent eventToUpdate, bool isNew)
    {
        roomsUI.Find(x => x.room == eventToUpdate.room).UpdateEvent(isNew);
    }

}
