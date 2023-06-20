using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour, ICureLevelIncrease
{

    public static SpawnerController instance;

    public List<EnemySO> activeEnemies = new List<EnemySO>();
    public List<EnemySO> activeEnemiesSide = new List<EnemySO>();

    List<Spawner> spawners = new List<Spawner>();
   //List<Spawner> dormantSpawners = new List<Spawner>();
    Dictionary<Transform, ITakeDamage> damageMap = new Dictionary<Transform, ITakeDamage>();

    #region oldSpawner
    //readonly float FIRST_SPAWN_DELAY = 0;
    //readonly int WAVE_STRENGTH_MAX_COUNT = 5;

    //List<Spawner> spawns = new List<Spawner>();


    //[SerializeField]
    //List<Wave> waves = new List<Wave>();
    //[SerializeField]
    //List<float> targetDifficulty;
    //[SerializeField] float avrgOfLastWaves;

    //Dictionary<int, ITakeDamage> damageMap = new Dictionary<int, ITakeDamage>();

    //float lastWavesStrength;
    //Queue<float> wavesStrength = new Queue<float>();

    //WaveSO currentWave;

    //[System.Serializable]
    //struct Wave
    //{
    //    public List<WaveSO> waves;
    //}

    //[SerializeField]
    //bool toggleSpawns;

    //float nextWave;
    //bool active;
    #endregion

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ProgressStageController.instance.AddToNotify(this);
    }


    //public WaveSO SpawnWave()
    //{
    //    List<WaveSO> possible = wavesActive.FindAll(wave => wave.GetWaveStrength() <= PacingController.pacingController.GetFreePacing());
    //    if (possible.Count > 0)
    //    {
    //        WaveSO choosen = possible[UnityEngine.Random.Range(0, possible.Count)];
    //        PacingController.pacingController.IncreasePacing(choosen.GetWaveStrength());
    //        return choosen;
    //    }
    //    return null;
    //}

    //public WaveSO SpawnSideWave()
    //{
    //    List<WaveSO> possible = subWavesActive.FindAll(wave => wave.GetWaveStrength() <= PacingController.pacingController.GetFreePacing() / SIDE_SPAWNERS_PACING_REDUCTION);
    //    if (possible.Count > 0)
    //    {
    //        WaveSO choosen = possible[UnityEngine.Random.Range(0, possible.Count)];
    //        PacingController.pacingController.IncreasePacing(choosen.GetWaveStrength() * SIDE_SPAWNERS_PACING_REDUCTION);
    //        return choosen;
    //    }
    //    return null;
    //}

    public EnemySO GetEnemy(bool main)
    {
        if(main) return activeEnemies[UnityEngine.Random.Range(0, activeEnemies.Count)];
        else return activeEnemiesSide[UnityEngine.Random.Range(0, activeEnemiesSide.Count)];
    }

    public void AddSpawner(Spawner toAdd)
    {
        spawners.Add(toAdd);
    }

    public void IncreaseLevel(int level)
    {
        activeEnemies.AddRange(GameController.currentDangerLevel.GetNewEnemies());
        //wavesActive.AddRange(GameController.currentDangerLevel.GetNewWaves());
        //subWavesActive.AddRange(GameController.currentDangerLevel.GetNewSubWaves());
    }

    public void AwakeSpawner()
    {
        Spawner spawner = spawners.Find(x => !x.isActive);
        if (spawner != null)
        {
            spawner.ActivateSpanwer();

        }
    }

        #region oldSpawner

        //public void AddSpawner(Spawner spawnerToAdd)
        //{
        //    spawns.Add(spawnerToAdd);
        //}


        ////private void Update()
        ////{
        ////    if (!toggleSpawns || !active) return;

        ////    if(Time.time >= nextWave)
        ////    {
        ////        TriggerWave();
        ////        nextWave = Time.time + currentWave.GetNextWaveDelay();
        ////    }
        ////}

        //public void StartSpawning()
        //{
        //    active = true;
        //    nextWave = Time.time + FIRST_SPAWN_DELAY;
        //}

        //public void TriggerWave()
        //{
        //    int currentLevel = ProgressStageController.instance.GetCurrentLevel();
        //    if (wavesStrength.Count > 0)
        //    {
        //        avrgOfLastWaves = lastWavesStrength / wavesStrength.Count;
        //    }
        //    else
        //    {
        //        avrgOfLastWaves = targetDifficulty[currentLevel];
        //    }

        //    Wave currentWavePool = waves[currentLevel];

        //    if(0.5f * (targetDifficulty[currentLevel]/ avrgOfLastWaves) > UnityEngine.Random.Range(0f, 1f))
        //    {
        //        List<WaveSO> avalible = currentWavePool.waves.FindAll(x => x.GetWaveStrength() > targetDifficulty[currentLevel]);
        //        currentWave = avalible[UnityEngine.Random.Range(0, avalible.Count)];
        //    }
        //    else
        //    {
        //        List<WaveSO> avalible = currentWavePool.waves.FindAll(x => x.GetWaveStrength() <= targetDifficulty[currentLevel]);
        //        currentWave = avalible[UnityEngine.Random.Range(0, avalible.Count)];
        //    }

        //    lastWavesStrength += currentWave.GetWaveStrength();
        //    wavesStrength.Enqueue(currentWave.GetWaveStrength());
        //    if(wavesStrength.Count >= WAVE_STRENGTH_MAX_COUNT)
        //    {
        //        lastWavesStrength -= wavesStrength.Dequeue();
        //    }


        //    List<Spawner> activeSpanwers = spawns.FindAll(x => x.isActive && !x.spawning);

        //    if(activeSpanwers.Count > 0)
        //    {
        //        activeSpanwers[UnityEngine.Random.Range(0, activeSpanwers.Count)].AddToSpawn(currentWave.GetEnemySpawn());
        //    }
        //    else
        //    {
        //        Debug.Log("Zero active spanwers");
        //    }
        //}

        #endregion

    public void AddEnemyToMap(ITakeDamage toAdd, Transform transformToAdd)
    {
        damageMap.Add(transformToAdd, toAdd);
    }

    public void RemoveFromMap(Transform toRemove)
    {
        try
        {
            damageMap.Remove(toRemove);
        }
        catch (Exception e)
        {
            Debug.LogError("Wrong unit name" + e);
        }
    }

    public ITakeDamage GetITakeDamageFormMap(Transform get)
    {
        return damageMap[get];
    }


}
