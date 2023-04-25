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
    }

    [SerializeField] List<CureProgressLevel> cureProgress;

    [SerializeField] int cureProgressLevel;
    [SerializeField] float cureCurrentProgress;

    bool cureMachineWorking;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if(cureMachineWorking)
        {
            cureCurrentProgress += Time.deltaTime ;
        }
    }

    public void CureMachineRunning(bool on)
    {
        cureMachineWorking = on;
    }

    public float GetCureCurrentProgress()
    {
        return cureCurrentProgress/cureProgress[cureProgressLevel].timeToCompleate;
    }

}
