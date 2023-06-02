using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 3)]
public class WaveSO : ScriptableObject
{
    [System.Serializable]
    public struct EnemySpawn
    {
        public int count;
        public EnemySO enemy;
        public float spawnDelay;
    }

    [SerializeField]
    float nextWaveDelay;

    [SerializeField]
    List<EnemySpawn> subWaves;

    [SerializeField] float difficulty;

    public List<EnemySpawn> GetEnemySpawn()
    {
        return subWaves;
    }
    public float GetNextWaveDelay()
    {
        return nextWaveDelay;
    }
    public float GetWaveStrength()
    {
        return difficulty;
    }

}
