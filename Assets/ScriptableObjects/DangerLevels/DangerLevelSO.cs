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

    [SerializeField] float targetPacingSide;
    [SerializeField] float pacingFalloffPerMinuteSide;




    [SerializeField] float waveDuration;
    [SerializeField] float respiteDuration;
    [SerializeField] List<EnemySO> newEnemies;
    [SerializeField] List<EnemySO> newEnemiesSide;

    //[SerializeField] ProgressLevel requiredToNextLevel;
    //[SerializeField] bool lastLevel;

    [SerializeField] float dangerZoneTimer;
    [SerializeField] float hpModifier;

    [SerializeField] bool spawnNewNests;
    [SerializeField] float minNestSpawnCooldown;
    [SerializeField, Range(0, 1)] float newNestChance;

    [SerializeField] List<ItemSO> newItems;

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

    public float GetTargetPacingSide()
    {
        return targetPacingSide;
    }

    public float GetPacingFallofSide()
    {
        return pacingFalloffPerMinuteSide;
    }

    //public ProgressLevel GetProgressRequired()
    //{
    //    return requiredToNextLevel;
    //}

    public List<EnemySO> GetNewEnemies()
    {
        return newEnemies;
    }


    public List<EnemySO> GetNewEnemiesSide()
    {
        return newEnemiesSide;
    }

    public List<ItemSO> GetNewItems()
    {
        return newItems;
    }

    public float GetDangerZoneTimer()
    {
        return dangerZoneTimer;
    }

    //public bool IsLast()
    //{
    //    return lastLevel;
    //}

    public float GetHpModifier()
    {
        return hpModifier;
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

    public float GetWaveDuration()
    {
        return waveDuration;
    }

    public float GetRespiteDuration()
    {
        return respiteDuration;
    }
}
