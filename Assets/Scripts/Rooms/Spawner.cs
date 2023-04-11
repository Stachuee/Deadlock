using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool isActive;

    [SerializeField]
    float spawnDelay;
    float lastSpawn;

    Queue<WaveSO.SubWave> waveToSpawn = new Queue<WaveSO.SubWave>();

    Queue<GameObject> prefabsToSpawn;

    private void Update()
    {
        if (isActive && lastSpawn + spawnDelay < Time.time)
        {
            if(waveToSpawn.Count > 0 && (prefabsToSpawn == null || prefabsToSpawn.Count == 0))
            {
                SetNewWave();
            }
            else if(prefabsToSpawn.Count != 0)
            {
                GameObject temp = Instantiate(prefabsToSpawn.Dequeue(), transform.position, Quaternion.identity);
                temp.GetComponent<EnemyBase>();
                lastSpawn = Time.time;
            }
        }
    }

    public void AddToSpawn(WaveSO.SubWave subWave)
    {
        waveToSpawn.Enqueue(subWave);
    }

    public void SetNewWave()
    {
        if(waveToSpawn.Count > 0)
        {
            List<WaveSO.EnemySpawn> enemiesToSpawn = waveToSpawn.Dequeue().enemies;
            prefabsToSpawn = new Queue<GameObject>();
            enemiesToSpawn.ForEach(enemy =>
            {
                for (int i = 0; i < enemy.count; i++) prefabsToSpawn.Enqueue(enemy.enemy.GetPrefab());
            });
        }
    }
}
