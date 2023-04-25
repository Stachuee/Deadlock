using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureMachine : ScientistPoweredInteractable
{

    public static CureMachine Instance { get; private set; }
    [SerializeField] List<float> supportsLevels = new List<float>();

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
        activePlayer = player;
        player.LockInAction(CloseInterface);
        activePlayer.uiController.cureMachine.Open(true);
    }

    public void CloseInterface()
    {
        activePlayer.UnlockInAnimation();
        activePlayer.uiController.cureMachine.Open(false);
    }

    public void AddSupport(int type, float ammount)
    {
        if(supportsLevels[type] == 0) CheckSupport();
        supportsLevels[type] += ammount;
    }

    void CheckSupport()
    {

    }

    public bool GetSupport(CureMachineSupportType type)
    {
        return supportsLevels[(int)type] >= 0;
    }

    public override void PowerOn(int power)
    {
        base.PowerOn(power);
        CureController.instance.CureMachineRunning(powered);
    }
}
