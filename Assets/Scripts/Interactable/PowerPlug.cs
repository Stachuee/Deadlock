using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlug : InteractableBase
{
    public static PowerPlug Instance;

    List<ItemSO> acceptedItems;

    [SerializeField]
    ItemSO inDeposit;
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    SpriteRenderer powerCellRenderer;

    [SerializeField] Sprite empty;
    [SerializeField] Sprite full;

    [SerializeField]
    List<ScientistPoweredInteractable> toManage;

    bool firstTime = true;

    PlayerController activePlayer;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        AddAction(OpenPanel);
        acceptedItems = new List<ItemSO>();
    }

    public void OpenPanel(PlayerController player, UseType use)
    {
        activePlayer = player;
        player.LockInAction(CloseBox);
        activePlayer.uiController.workshopFuse.Open();
    }

    public void TurnOn(bool state)
    {
        ElectricityController.SetWorkshopFuse(state);
        toManage.ForEach(powered => powered.PowerOn(state ? 1 : 0));
        if(firstTime)
        {
            firstTime = false;
            ProgressStageController.instance.StartGame();
        }
    }

    public void Refresh()
    {
        toManage.ForEach(powered => powered.PowerOn(ElectricityController.workshopPowered ? 1 : 0));

    }


    void CloseBox()
    {
        activePlayer.uiController.workshopFuse.Close();
        activePlayer.UnlockInAnimation();
    }

    //public void PlugBattery(PlayerController player, UseType type)
    //{
    //    if (inDeposit == null)
    //    {
    //        ItemSO deposited = player.CheckIfHoldingAnyAndDeposit<PowerCoreItem>();
    //        if (deposited != null)
    //        {
    //            inDeposit = deposited;
    //            powerCellRenderer.sprite = full;
    //            toManage.ForEach(powered => powered.PowerOn((deposited as PowerCoreItem).GetPowerLevel()));
    //            if (firstTime)
    //            {
    //                ProgressStageController.instance.StartGame();
    //                firstTime = false;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        GameObject temp = Instantiate(itemPrefab, transform.position, Quaternion.identity);
    //        temp.GetComponentInChildren<Item>().Innit(inDeposit);
    //        inDeposit = null;
    //        powerCellRenderer.sprite = empty;
    //        toManage.ForEach(powered => powered.PowerOn(0));
    //    }
    //}

    public override bool IsUsable(PlayerController player)
    {
        return inDeposit != null || player.CheckIfHoldingAny<PowerCoreItem>();
    }
}
