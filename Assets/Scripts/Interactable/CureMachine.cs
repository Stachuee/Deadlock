using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureMachine : ScientistPoweredInteractable
{

    public static CureMachine Instance { get; private set; }
    [SerializeField] List<float> supportsLevels = new List<float>();

    [SerializeField] List<CureMachineSupportType> currentUssage;
    [SerializeField] List<ItemSO> itemsToUse;

    [SerializeField] float supportUsageRate;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) Instance = this;
        else Debug.LogError("Two cure machines");
    }

    PlayerController activePlayer;
    private void Start()
    {
        AddAction(OpenInterface);
        foreach(CureMachineSupportType type in Enum.GetValues(typeof(CureMachineSupportType)))
        {
            supportsLevels.Add(0f);
        }
    }

    private void Update()
    {
        supportsLevels.ForEach(x =>
        {
            float newValue = Mathf.Clamp01(x - supportUsageRate * Time.deltaTime);
            if (newValue <= 0 && x != newValue) CheckSupport();
            x = newValue;
        });
    }

    public void OpenInterface(PlayerController player)
    {
        ItemSO item = null;
        if((item = player.CheckIfHoldingAnyAndDeposit(itemsToUse)) != null)
        {
            itemsToUse.Remove(item);
            if(itemsToUse.Count == 0)
            {
                CureController.instance.CureMachineItemsReady(true);
            }
        }
        else
        {
            activePlayer = player;
            player.LockInAction(CloseInterface);
            activePlayer.uiController.cureMachine.Open(true);
        }
    }

    public void CloseInterface()
    {
        activePlayer.UnlockInAnimation();
        activePlayer.uiController.cureMachine.Open(false);
    }

    public void AddSupport(int type, float ammount)
    {
        if (supportsLevels[type] == 0)
        {
            supportsLevels[type] = Mathf.Clamp01(supportsLevels[type] + ammount);
            CheckSupport();
        }
        else
        {
            supportsLevels[type] = Mathf.Clamp01(supportsLevels[type] + ammount);
        }
    }

    void CheckSupport()
    {
        bool allFilled = true;
        currentUssage.ForEach(toUse =>
        {
            if (supportsLevels[(int)toUse] <= 0) allFilled = false;
        });
        CureController.instance.CureMachineSupportReady(allFilled);
    }

    public void SetCurrentUssage(List<CureMachineSupportType> toUse)
    {
        currentUssage = toUse;
        CheckSupport();
    }

    public void SetCurrentItemUssage(List<ItemSO> toUse)
    {
        itemsToUse = toUse;
    }

    public List<ItemSO> GetRequiredItems()
    {
        return itemsToUse;
    }

    public List<CureMachineSupportType> GetRequiredSupport()
    {
        return currentUssage;
    }

    public float GetSupport(int type)
    {
        return supportsLevels[type];
    }

    public override void PowerOn(int power)
    {
        base.PowerOn(power);
        CureController.instance.CureMachineRunning(powered);
    }
}
