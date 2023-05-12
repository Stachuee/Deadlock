using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressStageController : MonoBehaviour
{
    public static ProgressStageController instance;

    [System.Serializable]
    struct ProgressLevel
    {
        public float timeToCompleate;
        public int itemRequired;
        public List<CureMachineSupportType> machinesRequired;
        public List<ItemSO> itemsNeeded;
    }

    [SerializeField] List<ProgressLevel> cureProgress;

    [SerializeField] int progressLevel;
    [SerializeField] float currentProgress;

    [SerializeField] bool machinePowered;
    [SerializeField] bool machineSupportFilled;
    [SerializeField] bool machineItemsFilled;

    List<ICureLevelIncrease> toNotify = new List<ICureLevelIncrease>();

    bool started;


    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if(machinePowered && machineSupportFilled && machineItemsFilled)
        {
            currentProgress += Time.deltaTime;
            if (GetCurrentProgress() >= 1) NextLevel();
        }
    }

    public void StartGame()
    {
        if (started) return;
        EffectManager.instance.ScreenShake(3, EffectManager.ScreenShakeRange.Global, EffectManager.ScreenShakeStrength.Weak, Vector2.zero);
        started = true;
        NextLevel();
        SpawnerController.instance.StartSpawning();
    }

    private void NextLevel()
    {
        if(progressLevel + 1 >= cureProgress.Count)
        {
            Debug.Log("Win");
        }
        else
        {
            progressLevel++;
            currentProgress = 0;
            machineSupportFilled = cureProgress[progressLevel].machinesRequired.Count == 0;
            machineItemsFilled = cureProgress[progressLevel].itemsNeeded.Count == 0;
            CureMachine.Instance.SetCurrentUssage(cureProgress[progressLevel].machinesRequired);
            CureMachine.Instance.SetCurrentItemUssage(cureProgress[progressLevel].itemsNeeded);
            Notify();
        }
    }

    public void MachineSupportReady(bool filled)
    {
        machineSupportFilled = filled;
    }

    public void MachineRunning(bool on)
    {
        machinePowered = on;
    }

    public void MachineItemsReady(bool ready)
    {
        machineItemsFilled = ready;
    }


    public float GetCurrentProgress()
    {
        return currentProgress/cureProgress[progressLevel].timeToCompleate;
    }

    public int GetCurrentLevel()
    {
        return progressLevel;
    }

    public void AddToNotify(ICureLevelIncrease target)
    {
        toNotify.Add(target);
    }

    public void Notify()
    {
        toNotify.ForEach(x =>
        {
            x.IncreaseLevel(progressLevel);
        });
    }
}
