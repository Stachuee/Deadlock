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
    RectTransform itemInfoPanelRect;
    ItemInfo itemInfoPanel;

    [SerializeField]
    Vector2 itemInfoOffset;

    [SerializeField]
    List<Panel> panels;
    [SerializeField]
    PlayerController playerController;


    [HideInInspector] public FuseBoxUIScript fuseBox;
    [HideInInspector] public ComputerUI computer;
    [HideInInspector] public CureMachineUI cureMachine;
    [HideInInspector] public CraftingHelperScript craftingHelper;
    [HideInInspector] public CombatHUDController combatHUDController;
    [HideInInspector] public UpgradeGuide upgradeGuide;

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
            if(value != null && value is IGetHandInfo)
            {
                itemInfoPanel.SetupInfo((value as IGetHandInfo).GetHandInfo());
            }
            else
            {
                itemInfoPanel.HideInfo();
            }
            tohighlight = value;
        }
    }

    IInteractable tohighlight;

    private void Awake()
    {
        fuseBox = GetComponentInChildren<FuseBoxUIScript>();
        computer = GetComponentInChildren<ComputerUI>();
        cureMachine = GetComponentInChildren<CureMachineUI>();
        craftingHelper = GetComponentInChildren<CraftingHelperScript>();
        combatHUDController = GetComponentInChildren<CombatHUDController>();
        upgradeGuide = GetComponentInChildren<UpgradeGuide>();
    }


    private void Start()
    {
        panels.ForEach(x =>
        {
            x.panel.transform.position = x.anchor.transform.position;
            x.panel.gameObject.SetActive(x.isOn);

            Destroy(x.anchor.gameObject);
        });
        itemInfoPanelRect = Instantiate(itemInfoPanelPrefab, Vector2.zero, Quaternion.identity, transform).GetComponent<RectTransform>();
        itemInfoPanel = itemInfoPanelRect.GetComponent<ItemInfo>();
        itemInfoPanelRect.gameObject.SetActive(false);
        cam = playerController.cameraController.cam;
    }

    private void Update()
    {
        if(ToHighlight != null)
        {
            itemInfoPanelRect.position = ToHighlight.GetTransform().position + (Vector3)itemInfoOffset;
        }
    }
}
