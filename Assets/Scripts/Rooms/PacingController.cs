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

    float waveDuration;
    float waveCurrentDuration;

    float respiteDuration;
    float respiteCurrentDuration;

    List<DormantSpawner> dormantSpawners;

    [SerializeField] float pacingCheck;
    float lastPaceCheck;

    bool createNests;
    float minNestSpawnCooldown;
    float newNestChance;

    float lastNestSpawn;

    float nextUpdate;

    bool active;

    public static bool wave;

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


        if (wave)
        {
            waveCurrentDuration += Time.deltaTime;
            if(waveCurrentDuration >= waveDuration)
            {
                wave = false;
                waveCurrentDuration = 0;
            }
        }
        else
        {
            respiteCurrentDuration += Time.deltaTime;
            if(respiteCurrentDuration >= respiteDuration)
            {
                wave = true;
                respiteCurrentDuration = 0;
            }
        }
    }

    public void IncreaseLevel(int level)
    {
        targetPacing = GameController.currentDangerLevel.GetTargetPacing();
        pacingFalloff = GameController.currentDangerLevel.GetPacingFallof();
        minNestSpawnCooldown = GameController.currentDangerLevel.GetMinNestSpawnCooldown();
        createNests = GameController.currentDangerLevel.GetSpwanNewNest();
        newNestChance = GameController.currentDangerLevel.GetNewNestChance();

        waveDuration = GameController.currentDangerLevel.GetWaveDuration();
        respiteCurrentDuration = GameController.currentDangerLevel.GetRespiteDuration();

        pacing = Mathf.Max(pacing, targetPacing);
        lastPaceCheck = Time.time;
        active = true;
        wave = true;
    }

    public void TriggerEvent()
    {
        if (createNests && Time.time - lastNestSpawn > minNestSpawnCooldown && Random.Range(0f, 1f) < newNestChance)
        {
            SpawnerController.instance.AwakeSpawner();
            lastNestSpawn = Time.time;
        }

        if (pacing < targetPacing + pacingOverTime)
        {
            Debug.Log("normal");
            //SpawnerController.instance.SpawnEnemy();
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
