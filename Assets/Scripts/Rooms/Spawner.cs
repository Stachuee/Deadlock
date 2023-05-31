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

    public bool spawning;

    List<WaveSO.EnemySpawn> toSpawnList;
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
                alreadySpawned = 0;
            }
            else
            {
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
        isActive = false;
    }

    public void AddToSpawn(List<WaveSO.EnemySpawn> subWave)
    {
        Debug.Log(subWave.Count);
        toSpawnList = subWave;
        step = 0;
        spawning = true;
    }



    public void IncreaseLevel(int level)
    {
        if(level == activateAt) ActivateSpanwer();
        else if(level == deactivateAt) DeactivateSpawner();
    }

}
