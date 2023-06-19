using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 2)]
public class EnemySO : ScriptableObject
{
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField] float weight;

    public GameObject GetPrefab()
    {
        return enemyPrefab;
    }

    public float GetWeigth()
    {
        return weight;
    }

}
