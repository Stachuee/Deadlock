using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    List<Spawner> spawns = new List<Spawner>();

    [SerializeField]
    List<WaveSO> waves = new List<WaveSO>();

    [SerializeField]
    List<EnemySO> enemies = new List<EnemySO>();

    int wave;
    float nextWave;

    private void Awake()
    {
        spawns = FindObjectsOfType<Spawner>().ToList();
    }

    private void Start()
    {
        wave = 0;
    }

    private void Update()
    {
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
