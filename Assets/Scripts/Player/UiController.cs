using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UiController : MonoBehaviour
{
    [System.Serializable]
    struct Panel
    {
        public Transform panel;
        public Transform anchor;
        public bool isOn;
    }

    [SerializeField]
    GameObject itemInfoPanelPrefab;
    RectTransform itemInfoPanel;
    [SerializeField]
    Vector2 itemInfoOffset;

    [SerializeField]
    List<Panel> panels;
    [SerializeField]
    PlayerController playerController;


    [HideInInspector] public FuseBoxUIScript fuseBox;
    [HideInInspector] public ComputerUI computer;
    [HideInInspector] public CureMachineUI cureMachine;

    Camera cam;

    public MultiplayerEventSystem myEventSystem; 
    

    public IInteractable ToHighlight
    {
        get
        {
            return tohighlight;
        }
        set
        {
            if(value != tohighlight)
            {
                if(value != null && value is IGetHandInfo)
                {
                    itemInfoPanel.GetComponent<ItemInfo>().SetupInfo((value as IGetHandInfo).GetHandInfo());
                    itemInfoPanel.gameObject.SetActive(true);
                }
                else
                {
                    itemInfoPanel.gameObject.SetActive(false);
                }
                tohighlight = value;
            }
        }
    }

    IInteractable tohighlight;

    private void Awake()
    {
        fuseBox = GetComponentInChildren<FuseBoxUIScript>();
        computer = GetComponentInChildren<ComputerUI>();
        cureMachine = GetComponentInChildren<CureMachineUI>();
    }


    private void Start()
    {
        panels.ForEach(x =>
        {
            x.panel.transform.position = x.anchor.transform.position;
            x.panel.gameObject.SetActive(x.isOn);

            Destroy(x.anchor.gameObject);
        });
        itemInfoPanel = Instantiate(itemInfoPanelPrefab, Vector2.zero, Quaternion.identity, transform).GetComponent<RectTransform>();
        itemInfoPanel.gameObject.SetActive(false);
        cam = playerController.cameraController.cam;
    }

    private void Update()
    {
        if(ToHighlight != null)
        {
            itemInfoPanel.position = ToHighlight.GetTransform().position + (Vector3)itemInfoOffset;
        }
    }
}
