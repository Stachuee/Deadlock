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
    public float speedupUponCompleate;
    public bool isLast;
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

    public List<ProgressLevel> progressLevels;

    bool started;
    bool last;
    bool charging;


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
            if(GetCurrentLevel() >= 0.75f && last && !charging)
            {
                charging = true;
                CureMachine.Instance.Charging();
            }
        }
    }

    public void StartGame()
    {
        if (started) return;
        started = true;
        PacingController.pacingController.StartGame();
        NextLevel();
        //SpawnerController.instance.StartSpawning();
        MapSegment.scientistSegment.ScientistSegmentUnlock();
        DialogueManager.instance.StartQuips();
    }

    private void NextLevel()
    {
        if (progressLevel >= 0 && progressLevels[progressLevel].isLast)
        {
            CureMachine.Instance.End();
        }
        else
        {
            progressLevel++;
            PacingController.pacingController.dangerLevelTime += progressLevels[progressLevel].speedupUponCompleate;

            currentProgress = 0;
            machineSupportFilled = progressLevels[progressLevel].machinesRequired.Count == 0;
            machineItemsFilled = progressLevels[progressLevel].itemsNeeded.Count == 0;
            CureMachine.Instance.SetCurrentUssage(new List<CureMachineSupportType>(progressLevels[progressLevel].machinesRequired));
            CureMachine.Instance.SetCurrentItemUssage(new List<ItemSO>(progressLevels[progressLevel].itemsNeeded));
            progressRequired = progressLevels[progressLevel].timeToCompleate;
            last = progressLevels[progressLevel].isLast;
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
        return currentProgress/ progressRequired;
    }

    public int GetCurrentLevel()
    {
        return progressLevel;
    }


}
