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

    [SerializeField] GameObject wave;
    [SerializeField] ParticleSystem charge;

    bool charging;

    [SerializeField] AnimationCurve sizeChange;
    [SerializeField] float maxSize;
    [SerializeField] float explosionTime;
    float explosionStart;
    bool exploded;

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
        if (exploded)
        {
            if(Time.time < explosionStart + explosionTime)
            {
                wave.transform.localScale = Vector3.one * (sizeChange.Evaluate((explosionStart - Time.time) / explosionTime) * maxSize);
            }
            else
            {
                wave.SetActive(false);
            }
        }

    }

    public void OpenInterface(PlayerController player, UseType type)
    {
        ItemSO item = null;
        if((item = player.CheckIfHoldingAnyAndDeposit(itemsToUse)) != null)
        {
            itemsToUse.Remove(item);
            if(itemsToUse.Count == 0)
            {
                ProgressStageController.instance.MachineItemsReady(true);
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
        ProgressStageController.instance.MachineSupportReady(allFilled);
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
        if(power > 0 && charging)
        {
            charge.Play();
        }
        else
        {
            charge.Stop();
        }
        ProgressStageController.instance.MachineRunning(powered);
    }

    public void End()
    {
        Debug.Log("end");
        wave.SetActive(true);
        exploded = true;
        explosionStart = Time.time;
        charge.Stop();
    }

    public void Charging()
    {
        charging = true;
        if(powered) charge.Play();
    }

}
