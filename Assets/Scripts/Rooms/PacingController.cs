using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PacingController : MonoBehaviour, ICureLevelIncrease
{
    public static PacingController pacingController;

    [SerializeField]
    float pacing;
    [SerializeField] float pacingOverTimeIncrease;
    [SerializeField] float pacingFalloff;
    float pacingOverTime;
    Vector2 targetPacing;

    List<DormantSpawner> dormantSpawners;

    [SerializeField] float pacingCheck;
    float lastPaceCheck;


    float nextUpdate;


    private void Awake()
    {
        if (pacingController == null) pacingController = this;
        else Destroy(gameObject);

        dormantSpawners = FindObjectsOfType<DormantSpawner>().ToList();
    }

    private void Start()
    {
        ProgressStageController.instance.AddToNotify(this);
    }


    private void Update()
    {
        pacing -= (pacingFalloff / 60) * Time.deltaTime;
        pacing = Mathf.Max(pacing, 0);
        pacingOverTime += pacingOverTimeIncrease * Time.deltaTime;

        if (lastPaceCheck + pacingCheck < Time.time)
        {
            TriggerEvent();
            lastPaceCheck = Time.time;
        }

    }

    public void IncreaseLevel(int level)
    {
        Debug.Log(GameController.currentDangerLevel.GetTargetPacing());
        targetPacing = GameController.currentDangerLevel.GetTargetPacing();
        pacingFalloff = GameController.currentDangerLevel.GetPacingFallof();
        pacing = Mathf.Max(pacing, targetPacing.x);
        lastPaceCheck = Time.time;
    }

    public void TriggerEvent()
    {
        if (pacing < targetPacing.x + pacingOverTime)
        {
            // too easy
        }
        else if (pacing > targetPacing.y + pacingOverTime)
        {
            //too hard
        }
        else
        {
            //good spot
        }
    }

    public float GetFreePacing()
    {
        return targetPacing.y + pacingOverTime - pacing;
    }

    public void IncreasePacing(float ammount)
    {
        pacing += ammount;
    }

}
