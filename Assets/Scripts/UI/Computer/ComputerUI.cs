using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WarningStrength {Weak, Medium, Strong };
public enum Marker {Normal, Science, Elite, Spawner }
public class ComputerUI : MonoBehaviour, IControllSubscriberMove, IControllSubscriberUse
{
    [SerializeField] Transform content;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject roomPrefabUI;
    [SerializeField] GameObject segmentPrefab;
    [SerializeField] GameObject doorPrefab;
    [SerializeField] GameObject stairsPrefab;
    [SerializeField] GameObject IconPrefab;

    [SerializeField] PlayerController playerController;

    [SerializeField] public static int scale = 30;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform contentPanel;

    [SerializeField] GameObject highlightCursorPrefab;
    GameObject highlightCursor;

    List<RoomUiButton> roomsUI = new List<RoomUiButton>();

    bool lookingAtMap;
    bool controlling;
    ITakeControll controllingObject;
    Rooms activeRoom;
    GameObject firstButton;

    public static Vector2 startingDrawPos;

    bool isScientist;
    bool setUp;

    public static ComputerUI scientistComputer;

    [SerializeField] AudioSource alarmSFX;


    private void Start()
    {
        if(playerController.isScientist)
        {
            scientistComputer = this;
            StartCoroutine("BootCompurer");
        }
    }

    IEnumerator BootCompurer()
    {
        yield return new WaitForSeconds(0.1f);
        if (!setUp) Setup();
    }

    public void Setup()
    {
        EventManager.eventManager.AddlLstener(this);

        RectTransform myRect = transform.GetComponent<RectTransform>();

        Rooms starting = FacilityController.facilityController.allRooms.Find(x => x.startingRoom);

        List<Rooms> allRooms = new List<Rooms>(FacilityController.facilityController.allRooms);

        startingDrawPos = starting.transform.position;

        Vector2Int size = new Vector2Int(0,0); // x, y


        SegmentController.segmentController.mapSegments.ForEach(segment =>
        {
            RectTransform temp = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>();

            temp.anchoredPosition = ((Vector2)segment.transform.position - startingDrawPos) * scale;
            temp.sizeDelta = new Vector2(segment.size.x, segment.size.y) * scale * 2;

            temp.GetComponent<Image>().color = segment.segmentColor;
        });

        allRooms.ForEach(room =>
        {
            room.doorMarkers.ForEach(x =>
            {
                if (x.transform.position.x > room.transform.position.x || x.transform.position.y > room.transform.position.y)
                {
                    RectTransform temp = Instantiate(doorPrefab, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>();
                    temp.anchoredPosition = ((Vector2)x.transform.position - startingDrawPos) * scale;
                    temp.sizeDelta = new Vector2(room.roomSize.x, x.GetDoorSize()) * scale * new Vector2(0.2f, 1);
                }
            });
        });

        allRooms.ForEach(roomToDraw =>
        {
            RectTransform temp = Instantiate(roomPrefabUI, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>();

            RoomUiButton button = temp.GetComponent<RoomUiButton>();
            button.SetUp(roomToDraw, this);

            if (roomToDraw.startingRoom) firstButton = temp.gameObject;

            roomsUI.Add(button);

            temp.anchoredPosition = ((Vector2)roomToDraw.transform.position - startingDrawPos) * scale;
            if (size.x < Mathf.Abs(temp.anchoredPosition.x)) size.x = Mathf.CeilToInt(Mathf.Abs(temp.anchoredPosition.x)) + roomToDraw.roomSize.x * scale;
            if (size.y < Mathf.Abs(temp.anchoredPosition.y)) size.y = Mathf.CeilToInt(Mathf.Abs(temp.anchoredPosition.y)) + roomToDraw.roomSize.y * scale;
            temp.sizeDelta = new Vector2(roomToDraw.roomSize.x, roomToDraw.roomSize.y) * scale * 1.9f;
        });

        allRooms.ForEach(room =>
        {
            room.stairs.ForEach(stairs =>
            {
                StairsScript connected = stairs.GetConnected();
                if (stairs.GetPosition().y > connected.GetPosition().y)
                {
                    RectTransform temp = Instantiate(stairsPrefab, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>();
                    temp.anchoredPosition = (stairs.GetPosition() - startingDrawPos) * scale;
                    temp.sizeDelta = new Vector2(1, 1) * scale;
                    temp = Instantiate(stairsPrefab, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>();
                    temp.anchoredPosition = (connected.GetPosition() - startingDrawPos) * scale;
                    temp.sizeDelta = new Vector2(1, 1) * scale;

                    float height = stairs.GetPosition().y - connected.GetPosition().y;

                    temp = Instantiate(stairsPrefab, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>(); // change this to connected
                    temp.rotation = Quaternion.Euler(0, 0, 0);
                    temp.sizeDelta = new Vector2(0.5f, height / 2) * scale;
                    temp.anchoredPosition = (stairs.GetPosition() - startingDrawPos) * scale;

                    temp = Instantiate(stairsPrefab, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>(); // change this to connected
                    temp.rotation = Quaternion.Euler(0, 0, -180);
                    temp.sizeDelta = new Vector2(0.5f, height / 2) * scale;
                    temp.anchoredPosition = (connected.GetPosition() - startingDrawPos) * scale;

                    temp = Instantiate(stairsPrefab, Vector3.zero, Quaternion.identity, content).GetComponent<RectTransform>(); // change this to connected
                    temp.rotation = Quaternion.Euler(0, 0, (stairs.GetPosition().x > connected.GetPosition().x ? -90 : 90));
                    temp.sizeDelta = new Vector2(0.5f, Mathf.Abs(stairs.GetPosition().x - connected.GetPosition().x)) * scale;
                    if((stairs.GetPosition().x > connected.GetPosition().x))
                    {
                        temp.anchoredPosition = (connected.GetPosition() - startingDrawPos +
                        new Vector2(0, stairs.GetPosition().y - connected.GetPosition().y) / 2) * scale;
                    }
                    else
                    {
                        temp.anchoredPosition = (stairs.GetPosition() - startingDrawPos -
                        new Vector2(0, stairs.GetPosition().y - connected.GetPosition().y) / 2) * scale;
                    }



                    //Vector2 dir = connected.GetPosition() - stairs.GetPosition();
                    //temp.anchoredPosition = (stairs.GetPosition()  - startingDrawPos) * scale;
                    //temp.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(stairs.GetPosition(), connected.GetPosition()));
                    //temp.sizeDelta = new Vector2(0.5f, dir.magnitude) * scale;
                }
            });
        });
        

        size *= 2;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(size.x, myRect.sizeDelta.x), Mathf.Max(size.y, myRect.sizeDelta.y));

        setUp = true;
    }

    Vector2 vel = Vector2.zero;

    [SerializeField]
    RectTransform marker;
    [SerializeField]
    PlayerController player;
    [SerializeField] GameObject playerMarkerPrefab;

    [SerializeField] GameObject normalMarker;
    [SerializeField] GameObject scienceMarker;
    [SerializeField] GameObject eliteMarker;
    [SerializeField] GameObject spawnerMarker;


    private void Update()
    {
        if(lookingAtMap)
        {

            if(player != null)
            {
                if(marker != null)
                {
                    marker.anchoredPosition = ((Vector2)player.transform.position - startingDrawPos) * scale;
                }
                else
                {
                    Debug.Log(marker);
                    marker = Instantiate(playerMarkerPrefab, Vector2.zero, Quaternion.identity, contentPanel).GetComponent<RectTransform>();
                    marker.anchoredPosition = ((Vector2)player.transform.position - startingDrawPos) * scale;
                    Debug.Log(marker);
                }
            }
            else
            {
                player = GameController.solider;
            }
            if (!playerController.keyboard)
            {
                contentPanel.anchoredPosition = Vector2.SmoothDamp(contentPanel.anchoredPosition, (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
        - (Vector2)scrollRect.transform.InverseTransformPoint(playerController.uiController.myEventSystem.currentSelectedGameObject.transform.position), ref vel, 0.05f);
            }
        }

        if (Highlighted != null)
        {
            highlightCursor.transform.position = Highlighted.GetTransform().position;
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
                angle = Vector2.SignedAngle(Vector2.one, (x.GetTransform().position - room.transform.position))
            });
        });

        playerController.uiController.cameraHUD.Show(true, room.GetMySegment());

        playerController.AddMoveSubscriber(this);
        playerController.AddUseSubscriber(this);
        panel.SetActive(false);
        lookingAtMap = false;
    }

    public void UnlockSegment(MapSegment segment, bool unlocked)
    {
        roomsUI.FindAll(x => x.segment == segment).ForEach(button => button.Unlock(unlocked));
    }

    public void OpenComputer()
    {
        if(!setUp) Setup();
        panel.SetActive(true);
        lookingAtMap = true;
        playerController.uiController.myEventSystem.SetSelectedGameObject(null);
        playerController.cameraController.useMove = true;
        StartCoroutine("SetSelected");
    }

    IEnumerator SetSelected()
    {
        yield return new WaitForEndOfFrame();
        playerController.uiController.myEventSystem.SetSelectedGameObject(firstButton);
    }

    public bool Back()
    {
        if(controlling)
        {
            playerController.cameraController.ChangeTarget(activeRoom.transform);
            controllingObject.Leave();
            controlling = false;
            return false;
        }
        else if (lookingAtMap)
        {
            panel.SetActive(false);
            playerController.cameraController.ResetTarget();
            playerController.cameraController.useMove = false;
            return true;
        }
        else
        {
            panel.SetActive(true);
            lookingAtMap = true;
            activeRoom = null;
            playerController.RemoveMoveSubscriber(this);
            playerController.RemoveUseSubscriber(this);
            Highlighted = null;
            playerController.uiController.cameraHUD.Show(false);
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
                if (highlightCursor == null) highlightCursor = Instantiate(highlightCursorPrefab, value.GetTransform().position, Quaternion.identity, transform);
                else
                {
                    highlightCursor.SetActive(true);
                    highlightCursor.transform.position = value.GetTransform().position;
                }
                highlightCursor.GetComponent<HighlightController>().SetupHighlight(value);
            }
            else if(highlightCursor != null)
            {
                highlightCursor.SetActive(false);
            }
            highlighted = value;
        }
    }

    IInteractable highlighted;

    public void ForwardCommandMove(Vector2 controll, Vector2 controllSmooth)
    {
        if (controlling) return;
        if (controll.magnitude > 0)
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

            if (newHighlighted != Highlighted)
            {
                Highlighted = newHighlighted;
            }
        }
    }

    public void ForwardCommandUse()
    {
        if(Highlighted != null && !controlling)
        {
            Highlighted.Use(playerController, UseType.Computer);
            if (Highlighted is ITakeControll)
            {
                controllingObject = Highlighted as ITakeControll;
                if(controllingObject.CanTakeControll())
                {
                    controlling = true;
                    playerController.cameraController.ChangeTarget(Highlighted.GetTransform());
                    Highlighted = null;
                }
            }
        }
    }

    public void UpdateEvent(RoomEvent eventToUpdate, bool isNew, WarningStrength strength)
    {
        if (!isScientist) return;
        if (!setUp) Setup();
        roomsUI.Find(x => x.room == eventToUpdate.room).UpdateEvent(isNew, strength);
    }

    public void DisplayWarning(Rooms room, WarningStrength strength)
    {
        if (!isScientist) return;
        if (!setUp) Setup();
        alarmSFX.Play();
        roomsUI.Find(x => x.room == room).UpdateEvent(true, strength);
    }

    public static void DisplayWarningOnAllComputers(Rooms room, WarningStrength strength)
    {
        if (scientistComputer != null) scientistComputer.DisplayWarning(room, strength);
    }

    public RectTransform CreateMarker(Marker marker)
    {
        switch (marker)
        {
            case Marker.Normal:
                return Instantiate(normalMarker, Vector2.zero, Quaternion.identity, contentPanel).GetComponent<RectTransform>();
            case Marker.Science:
                return Instantiate(scienceMarker, Vector2.zero, Quaternion.identity, contentPanel).GetComponent<RectTransform>();
            case Marker.Elite:
                return Instantiate(eliteMarker, Vector2.zero, Quaternion.identity, contentPanel).GetComponent<RectTransform>();
            case Marker.Spawner:
                return Instantiate(scienceMarker, Vector2.zero, Quaternion.identity, contentPanel).GetComponent<RectTransform>();
        }
        return null;
    }

    public void UpdateMarker(Vector2 position, RectTransform marker)
    {
        marker.anchoredPosition = (position - startingDrawPos) * scale;
    }

    public void DeleteMarker(RectTransform marker)
    {
        Destroy(marker.gameObject);
    }


}
