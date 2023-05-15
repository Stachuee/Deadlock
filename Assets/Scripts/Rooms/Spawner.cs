using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : InteractableBase, ICureLevelIncrease
{
    public bool isActive = false;

    [SerializeField] int activateAt;
    [SerializeField] int deactivateAt;


    [SerializeField]
    float spawnDelay;
    float lastSpawn;

    bool spawning;


    Queue<WaveSO.SubWave> waveToSpawn = new Queue<WaveSO.SubWave>();

    WaveSO.EnemySpawn toSpawn;
    int alreadySpawned;
    int step;



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
        if (isActive && lastSpawn + spawnDelay < Time.time)
        {
            if(waveToSpawn.Count > 0 && (toSpawn.count <= alreadySpawned))
            {
                SetNewWave();
            }
            else if(toSpawn.count > alreadySpawned)
            {
                GameObject temp = Instantiate(toSpawn.enemy.GetPrefab(), transform.position, Quaternion.identity);
                temp.GetComponent<EnemyBase>();
                lastSpawn = Time.time;
                alreadySpawned++;
            }
            else if(spawning)
            {
                SpawnerController.instance.FinishedSpawning(this);
                spawning = false;
            }
        }
    }

    public void ActivateSpanwer()
    {
        isActive = true;
    }

    public void DeactivateSpawner()
    {
        if(spawning) SpawnerController.instance.FinishedSpawning(this);
        isActive = false;
        waveToSpawn = null;
    }

    public void AddToSpawn(WaveSO.SubWave subWave)
    {
        waveToSpawn.Enqueue(subWave);
        spawning = true;
    }

    public void SetNewWave()
    {
        if(step >= waveToSpawn.Peek().enemies.Count)
        {
            waveToSpawn.Dequeue();
            step = 0;
        }
        if (waveToSpawn.Count > 0)
        {
            toSpawn = waveToSpawn.Peek().enemies[step];
            spawnDelay = toSpawn.spawnDelay;
            step++;
        }
    }

    public void IncreaseLevel(int level)
    {
        if(level == activateAt) ActivateSpanwer();
        else if(level == deactivateAt) DeactivateSpawner();
    }

}
