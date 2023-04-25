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

    [SerializeField] bool cureMachinePowered;
    [SerializeField] bool cureMachineSupportFilled;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CureMachine.Instance.SetCurrentUssage(cureProgress[0].machinesRequired);
    }

    private void Update()
    {
        if(cureMachinePowered && cureMachineSupportFilled)
        {
            cureCurrentProgress += Time.deltaTime ;
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

    public float GetCureCurrentProgress()
    {
        return cureCurrentProgress/cureProgress[cureProgressLevel].timeToCompleate;
    }

}
