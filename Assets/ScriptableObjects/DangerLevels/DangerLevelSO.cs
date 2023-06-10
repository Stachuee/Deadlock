using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty {Easy, Normal};

[CreateAssetMenu(fileName = "DangerLevel", menuName = "ScriptableObjects/DangerLevel", order = 0)]
public class DangerLevelSO : ScriptableObject
{
    [SerializeField] Difficulty difficulty;
    [SerializeField] int dangerLevel;
    [SerializeField] float targetPacing;
    [SerializeField] float pacingFalloffPerMinute;
    [SerializeField] List<WaveSO> newWaves;
    [SerializeField] List<WaveSO> newSubWaves;
    [SerializeField] ProgressLevel requiredToNextLevel;
    [SerializeField] bool lastLevel;

    [SerializeField] bool spawnNewNests;
    [SerializeField] float minNestSpawnCooldown;
    [SerializeField, Range(0, 1)] float newNestChance;

    public int GetDangerLevel()
    {
        return dangerLevel;
    }

    public Difficulty GetDifficulty()
    {
        return difficulty;
    }

    public float GetTargetPacing()
    {
        return targetPacing;
    }

    public float GetPacingFallof()
    {
        return pacingFalloffPerMinute;
    }

    public ProgressLevel GetProgressRequired()
    {
        return requiredToNextLevel;
    }

    public List<WaveSO> GetNewWaves()
    {
        return newWaves;
    }

    public List<WaveSO> GetNewSubWaves()
    {
        return newSubWaves;
    }

    public bool IsLast()
    {
        return lastLevel;
    }

    public bool GetSpwanNewNest()
    {
        return spawnNewNests;
    }
    public float GetMinNestSpawnCooldown()
    {
        return minNestSpawnCooldown;
    }
    public float GetNewNestChance()
    {
        return newNestChance;
    }
}
