using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : InteractableBase, ICureLevelIncrease
{

    protected readonly float PACING_LOCK = 5f;
    public bool isActive = false;

    [SerializeField] bool activeFromStart;

    float spawnDelay;
    float lastSpawn;

    public bool spawning;


    List<WaveSO.EnemySpawn> toSpawnList;
    int alreadySpawned;
    int step;


    float spawnCooldown;
    protected float nextWave;

    protected override void Awake()
    {
        base.Awake();
        isActive = false;
    }

    private void Start()
    {
        SpawnerController.instance.AddSpawner(this);
        ProgressStageController.instance.AddToNotify(this);
    }

    private void Update()
    {
        if (spawning && isActive && lastSpawn + spawnDelay < Time.time)
        {
            if (toSpawnList[step].count > alreadySpawned)
            {
                GameObject temp = Instantiate(toSpawnList[step].enemy.GetPrefab(), transform.position, Quaternion.identity);
                SpawnerController.instance.AddEnemyToMap(temp.GetComponent<ITakeDamage>(), temp.transform);
                lastSpawn = Time.time;
                alreadySpawned++;
            }
            else if(step < toSpawnList.Count - 1)
            {
                step++;
                spawnDelay = toSpawnList[step].spawnDelay;
                alreadySpawned = 0;
            }
            else
            {
                spawning = false;
                nextWave = Time.time + spawnCooldown;
            }
        }
        if(nextWave < Time.time)
        {
            GetNewWave();
        }
    }

    public void IncreaseLevel(int level)
    {
        if(!isActive && level >= 0 && activeFromStart)
        { 
            isActive = true;
        }
    }

    public virtual void ActivateSpanwer()
    {
        isActive = true;
    }

    public void DeactivateSpawner()
    {
        isActive = false;
    }

    public void AddToSpawn(List<WaveSO.EnemySpawn> subWave, float cooldownAfterSpawning)
    {
        toSpawnList = subWave;
        step = 0;
        alreadySpawned = 0;
        spawning = true;
        spawnDelay = subWave[step].spawnDelay;
        spawnCooldown = cooldownAfterSpawning;
    }

    public virtual void GetNewWave()
    {
        WaveSO temp = SpawnerController.instance.SpawnWave();
        if (temp != null) AddToSpawn(temp.GetEnemySpawn(), temp.GetNextWaveDelay());
        else nextWave = Time.time + PACING_LOCK;
    }


}
