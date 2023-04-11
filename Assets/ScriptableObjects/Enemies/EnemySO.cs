using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 2)]
public class EnemySO : ScriptableObject
{
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    int weigth;
    [SerializeField]
    int minWave;

    public GameObject GetPrefab()
    {
        return enemyPrefab;
    }
}
