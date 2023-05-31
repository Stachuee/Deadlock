using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public static SpawnerController instance;

    readonly float FIRST_SPAWN_DELAY = 0;

    List<Spawner> spawns = new List<Spawner>();


    [SerializeField]
    List<Wave> waves = new List<Wave>();


    Dictionary<int, ITakeDamage> damageMap = new Dictionary<int, ITakeDamage>();


    WaveSO currentWave;

    [System.Serializable]
    struct Wave
    {
        public List<WaveSO> waves;
    }

    [SerializeField]
    bool toggleSpawns;

    float nextWave;
    bool active;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddSpawner(Spawner spawnerToAdd)
    {
        spawns.Add(spawnerToAdd);
    }


    private void Update()
    {
        if (!toggleSpawns || !active) return;

        if(Time.time >= nextWave)
        {
            TriggerWave();
            nextWave = Time.time + currentWave.GetNextWaveDelay();
        }
    }

    public void StartSpawning()
    {
        active = true;
        nextWave = Time.time + FIRST_SPAWN_DELAY;
    }

    public void TriggerWave()
    {
        
        Wave currentWavePool = waves[ProgressStageController.instance.GetCurrentLevel()];
        currentWave = currentWavePool.waves[UnityEngine.Random.Range(0, currentWavePool.waves.Count)];

        List<Spawner> activeSpanwers = spawns.FindAll(x => x.isActive && !x.spawning);

        if(activeSpanwers.Count > 0)
        {
            activeSpanwers[UnityEngine.Random.Range(0, activeSpanwers.Count)].AddToSpawn(currentWave.GetEnemySpawn());
        }
        else
        {
            Debug.Log("Zero active spanwers");
        }
    }

    int maxId = 1024;
    int nextId;

    public void AddEnemyToMap(ITakeDamage toAdd, Transform transformToAdd)
    {
        nextId++;
        if(nextId > maxId) nextId = 0;

        transformToAdd.name = nextId.ToString();
        damageMap.Add(nextId, toAdd);
    }

    public void RemoveFromMap(Transform toRemove)
    {
        try
        {
            damageMap.Remove(int.Parse(toRemove.name));
        }
        catch (Exception e)
        {
            Debug.LogError("Wrong unit name");
        }
    }

    public ITakeDamage GetITakeDamageFormMap(string key)
    {
        return damageMap[int.Parse(key)];
    }
}
