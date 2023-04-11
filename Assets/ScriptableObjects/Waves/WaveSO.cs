using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 3)]
public class WaveSO : ScriptableObject
{
    [System.Serializable]
    public struct SubWave
    {
        public List<EnemySpawn> enemies;
    }

    [System.Serializable]
    public struct EnemySpawn
    {
        public int count;
        public EnemySO enemy;
    }

    [SerializeField]
    int wave;
    [SerializeField]
    int waveWeigth;
    [SerializeField]
    float nextWaveDelay;

    [SerializeField]
    List<SubWave> subWaves;

    public List<SubWave> GetSubWaves()
    {
        return subWaves;
    }
    public float GetNextWaveDelay()
    {
        return nextWaveDelay;
    }

}
