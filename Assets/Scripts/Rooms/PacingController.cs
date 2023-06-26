using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PacingController : MonoBehaviour
{
    public static PacingController pacingController;

    public float hpModifier;
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
    [SerializeField] float minNestSpawnCooldown;
    float newNestChance;

    [SerializeField] float lastNestSpawn;

    float nextUpdate;

    bool active;

    public static bool wave;

    public List<ItemSO> toDrop = new List<ItemSO>();

    [SerializeField]
    List<DangerLevelSO> dangerLevels;
    List<DangerLevelIncrease> toNotify = new List<DangerLevelIncrease>();
    [SerializeField] int dangerLevel;
    public static DangerLevelSO currentDangerLevel;
    public float dangerLevelTime;


    private void Awake()
    {
        if (pacingController == null) pacingController = this;
        else Destroy(gameObject);
        dangerLevel = -1;

        dormantSpawners = FindObjectsOfType<DormantSpawner>().ToList();
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

        if(currentDangerLevel != null && dangerLevelTime > currentDangerLevel.GetDangerZoneTimer())
        {
            IncreaseLevel();
        }

        dangerLevelTime += Time.deltaTime;
    }

    public void StartGame()
    {
        IncreaseLevel();
    }

    public void IncreaseLevel()
    {
        EffectManager.instance.ScreenShake(3, EffectManager.ScreenShakeRange.Global, EffectManager.ScreenShakeStrength.Weak, Vector2.zero);
        if(currentDangerLevel != null) dangerLevelTime -= currentDangerLevel.GetDangerZoneTimer();
        dangerLevel++;
        currentDangerLevel = dangerLevels[dangerLevel];//.Find(x => x.GetDangerLevel() == dangerLevel && x.GetDifficulty() == GameController.difficulty);
        targetPacing = currentDangerLevel.GetTargetPacing();
        pacingFalloff = currentDangerLevel.GetPacingFallof();
        minNestSpawnCooldown = currentDangerLevel.GetMinNestSpawnCooldown();
        createNests = currentDangerLevel.GetSpwanNewNest();
        newNestChance = currentDangerLevel.GetNewNestChance();
        hpModifier = currentDangerLevel.GetHpModifier();
        waveDuration = currentDangerLevel.GetWaveDuration();
        respiteCurrentDuration = currentDangerLevel.GetRespiteDuration();

        toDrop.AddRange(currentDangerLevel.GetNewItems());

        pacing = Mathf.Max(pacing, targetPacing);
        lastPaceCheck = Time.time;
        active = true;
        wave = true;
        Notify();
    }

    public void TriggerEvent()
    {
        if (createNests && Time.time - lastNestSpawn > minNestSpawnCooldown && Random.Range(0f, 1f) < newNestChance)
        {
            SpawnerController.instance.AwakeSpawner();
            lastNestSpawn = Time.time;
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

    public ItemSO DropItem()
    {
        if (toDrop.Count > 0)
        {
            return toDrop[UnityEngine.Random.Range(0, toDrop.Count)];
        }
        return null;
    }
    public void AddToNotify(DangerLevelIncrease target)
    {
        toNotify.Add(target);
    }

    public void Notify()
    {
        toNotify.ForEach(x =>
        {
            x.IncreaseLevel(dangerLevel);
        });
    }

}
