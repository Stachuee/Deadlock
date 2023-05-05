using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ITakeDamage
{
    public bool isActive;

    [SerializeField]
    float spawnDelay;
    float lastSpawn;

    [SerializeField]
    float maxHp;
    float hp;
    [SerializeField]
    bool unlocksNewSegment;
    [SerializeField]
    MapSegment segmentToUnlock;

    [SerializeField]
    GameObject bossToSpawn;
    bool bossSpawned;

    Queue<WaveSO.SubWave> waveToSpawn = new Queue<WaveSO.SubWave>();

    Queue<GameObject> prefabsToSpawn = new Queue<GameObject>();

    private void Start()
    {
        hp = maxHp;
        if (isActive)
        {
            SpawnerController.instance.AddSpawner(this);
        }
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

    public float TakeDamage(float damage, DamageType type)
    {
        float damageAmmount = damage;
        //if (type == DamageType.Fire)
        //{
        //    damage *= 1.5f;
        //}
        hp -= damageAmmount;
        if(hp < 0)
        {
            if(unlocksNewSegment)
            {
                SpawnerController.instance.RemoveSpawner(this);
                segmentToUnlock.UnlockSegment();
                Destroy(gameObject);
            }
        }
        else if(hp/maxHp < 0.25f && !bossSpawned)
        {
            bossSpawned = true;
            Instantiate(bossToSpawn, transform.position, Quaternion.identity);
        }
        return damageAmmount;
    }

    public void TakeArmorDamage(DamageType type, float damage)
    {
        
    }
}
