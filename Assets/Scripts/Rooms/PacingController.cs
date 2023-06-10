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
    float targetPacing;

    List<DormantSpawner> dormantSpawners;

    [SerializeField] float pacingCheck;
    float lastPaceCheck;

    bool createNests;
    float minNestSpawnCooldown;
    float newNestChance;

    float lastNestSpawn;

    float nextUpdate;

    bool active;

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
        if (!active) return;

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
        targetPacing = GameController.currentDangerLevel.GetTargetPacing();
        pacingFalloff = GameController.currentDangerLevel.GetPacingFallof();
        minNestSpawnCooldown = GameController.currentDangerLevel.GetMinNestSpawnCooldown();
        createNests = GameController.currentDangerLevel.GetSpwanNewNest();
        newNestChance = GameController.currentDangerLevel.GetNewNestChance();

        pacing = Mathf.Max(pacing, targetPacing);
        lastPaceCheck = Time.time;
        active = true;
    }

    public void TriggerEvent()
    {
        if (pacing < targetPacing + pacingOverTime)
        {
            Debug.Log("normal");
            if (createNests && Time.time - lastNestSpawn > minNestSpawnCooldown && Random.Range(0f, 1f) < newNestChance)
            {
                SpawnerController.instance.AwakeSpawner();
                lastNestSpawn = Time.time;
            }
        }
        else
        {
            Debug.Log("hard");
        }
    }

    public float GetFreePacing()
    {
        return targetPacing + pacingOverTime - pacing;
    }

    public void IncreasePacing(float ammount)
    {
        pacing += ammount;
    }

}
