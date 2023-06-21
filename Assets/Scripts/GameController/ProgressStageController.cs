using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProgressLevel
{
    public float timeToCompleate;
    public List<CureMachineSupportType> machinesRequired;
    public List<ItemSO> itemsNeeded;
}


public class ProgressStageController : MonoBehaviour
{
    public static ProgressStageController instance;

   
    [SerializeField] int progressLevel;
    [SerializeField] float currentProgress;
    [SerializeField] float progressRequired;

    [SerializeField] bool machinePowered;
    [SerializeField] bool machineSupportFilled;
    [SerializeField] bool machineItemsFilled;



    List<ICureLevelIncrease> toNotify = new List<ICureLevelIncrease>();

    public static List<ItemSO> toDrop = new List<ItemSO>();

    public List<ProgressLevel> progressLevels;

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
        //SpawnerController.instance.StartSpawning();
        MapSegment.scientistSegment.ScientistSegmentUnlock();
        DialogueManager.instance.StartQuips();
    }

    private void NextLevel()
    {
        if (GameController.currentDangerLevel != null && GameController.currentDangerLevel.IsLast())
        {
            Debug.Log("Win");
        }
        else
        {
            progressLevel++;
            currentProgress = 0;
            GameController.gameController.IncreaseLevel(progressLevel);
            machineSupportFilled = GameController.currentDangerLevel.GetProgressRequired().machinesRequired.Count == 0;
            machineItemsFilled = GameController.currentDangerLevel.GetProgressRequired().itemsNeeded.Count == 0;
            CureMachine.Instance.SetCurrentUssage(new List<CureMachineSupportType>(GameController.currentDangerLevel.GetProgressRequired().machinesRequired));
            CureMachine.Instance.SetCurrentItemUssage(new List<ItemSO>(GameController.currentDangerLevel.GetProgressRequired().itemsNeeded));
            progressRequired = GameController.currentDangerLevel.GetProgressRequired().timeToCompleate;

            toDrop.AddRange(GameController.currentDangerLevel.GetNewItems());
            Notify();
        }
    }

    public ItemSO DropItem()
    {
        if(toDrop.Count > 0)
        {
            return toDrop[UnityEngine.Random.Range(0, toDrop.Count)];
        }
        return null;
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
        return currentProgress/ progressRequired;
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
