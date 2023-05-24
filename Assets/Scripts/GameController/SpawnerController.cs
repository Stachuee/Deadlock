using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public static SpawnerController instance;

    readonly float FIRST_SPAWN_DELAY = 0;

    List<Spawner> spawns = new List<Spawner>();

    List<Spawner> spawningSpawners = new List<Spawner>();

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
    bool cooldown;
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

        if (spawningSpawners.Count == 0 && !cooldown)
        {
            nextWave = Time.time + currentWave.GetNextWaveDelay();
            cooldown = true;
        }

        if(cooldown && Time.time >= nextWave)
        {
            TriggerWave();
            cooldown = false;
        }
    }

    public void StartSpawning()
    {
        cooldown = true;
        active = true;
        nextWave = Time.time + FIRST_SPAWN_DELAY;
    }

    public void TriggerWave()
    {
        
        Wave currentWavePool = waves[ProgressStageController.instance.GetCurrentLevel()];
        currentWave = currentWavePool.waves[Random.Range(0, currentWavePool.waves.Count)];

        List<Spawner> activeSpanwers = spawns.FindAll(x => x.isActive);

        for (int i = 0; i < currentWave.GetSubWaves().Count; i++)
        {
            if (activeSpanwers.Count > 0)
            {
                int randomSpawner = Random.Range(0, activeSpanwers.Count);
                activeSpanwers[randomSpawner].AddToSpawn(currentWave.GetSubWaves()[i]);
                spawningSpawners.Add(activeSpanwers[randomSpawner]);
            }
            else Debug.LogError("Not enough spawners");
        }
    }

    public void FinishedSpawning(Spawner spawning)
    {
        spawningSpawners.Remove(spawning);
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
        damageMap.Remove(int.Parse(toRemove.name));
    }

    public ITakeDamage GetITakeDamageFormMap(string key)
    {
        return damageMap[int.Parse(key)];
    }
}
