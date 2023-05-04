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


    Queue<WaveSO.SubWave> waveToSpawn = new Queue<WaveSO.SubWave>();

    Queue<GameObject> prefabsToSpawn = new Queue<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        isActive = false;
    }

    private void Start()
    {
        SpawnerController.instance.AddSpawner(this);
        CureController.instance.AddToNotify(this);
    }

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

    public void ActivateSpanwer()
    {
        isActive = true;
    }

    public void DeactivateSpawner()
    {
        isActive = false;
        prefabsToSpawn = null;
        waveToSpawn = null;
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

    public void IncreaseLevel(int level)
    {
        if(level == activateAt) ActivateSpanwer();
        else if(level == deactivateAt) DeactivateSpawner();
    }
}
