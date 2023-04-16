using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public static SpawnerController instance;

    List<Spawner> spawns = new List<Spawner>();

    [SerializeField]
    List<WaveSO> waves = new List<WaveSO>();

    [SerializeField]
    bool toggleSpawns;

    int wave;
    float nextWave;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        wave = 0;
    }

    public void AddSpawner(Spawner spawnerToAdd)
    {
        spawns.Add(spawnerToAdd);
    }

    public void RemoveSpawner(Spawner spawnerToRemove)
    {
        spawns.Remove(spawnerToRemove);
    }

    private void Update()
    {
        if (!toggleSpawns) return;
        if(Time.time >= nextWave)
        {
            TriggerWave();
        }
    }

    public void TriggerWave()
    {
        WaveSO currentWave = waves[Mathf.Min(wave, waves.Count - 1)];

        List<Spawner> activeSpanwers = spawns.FindAll(x => x.isActive);

        for (int i = 0; i < currentWave.GetSubWaves().Count; i++)
        {
            if (activeSpanwers.Count > 0)
            {
                int randomSpawner = Random.Range(0, activeSpanwers.Count);
                activeSpanwers[randomSpawner].AddToSpawn(currentWave.GetSubWaves()[i]);
            }
            else Debug.LogError("Not enough spawners");
        }
        nextWave = Time.time + currentWave.GetNextWaveDelay();
        wave++;
    }
}
