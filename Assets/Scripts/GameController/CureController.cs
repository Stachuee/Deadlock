using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureController : MonoBehaviour
{
    public static CureController instance;

    [System.Serializable]
    struct CureProgressLevel
    {
        public float timeToCompleate;
        public int itemRequired;
        public List<CureMachineSupportType> machinesRequired;
        public List<ItemSO> itemsNeeded;
    }

    [SerializeField] List<CureProgressLevel> cureProgress;

    [SerializeField] int cureProgressLevel;
    [SerializeField] float cureCurrentProgress;

    [SerializeField] bool cureMachinePowered;
    [SerializeField] bool cureMachineSupportFilled;
    [SerializeField] bool cureMachineItemsFilled;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        NextLevel();
    }

    private void Update()
    {
        if(cureMachinePowered && cureMachineSupportFilled && cureMachineItemsFilled)
        {
            cureCurrentProgress += Time.deltaTime;
            if (GetCureCurrentProgress() >= 1) NextLevel();
        }
    }

    private void NextLevel()
    {
        if(cureProgressLevel + 1 >= cureProgress.Count)
        {
            Debug.Log("Win");
        }
        else
        {
            cureProgressLevel++;
            cureCurrentProgress = 0;
            cureMachineSupportFilled = cureProgress[cureProgressLevel].machinesRequired.Count == 0;
            cureMachineItemsFilled = cureProgress[cureProgressLevel].itemsNeeded.Count == 0;
            CureMachine.Instance.SetCurrentUssage(cureProgress[cureProgressLevel].machinesRequired);
            CureMachine.Instance.SetCurrentItemUssage(cureProgress[cureProgressLevel].itemsNeeded);
        }
    }

    public void CureMachineSupportReady(bool filled)
    {
        cureMachineSupportFilled = filled;
    }

    public void CureMachineRunning(bool on)
    {
        cureMachinePowered = on;
    }

    public void CureMachineItemsReady(bool ready)
    {
        cureMachineItemsFilled = ready;
    }


    public float GetCureCurrentProgress()
    {
        return cureCurrentProgress/cureProgress[cureProgressLevel].timeToCompleate;
    }

    public int GetCurrentLevel()
    {
        return cureProgressLevel;
    }
}
