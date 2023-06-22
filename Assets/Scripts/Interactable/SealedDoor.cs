using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealedDoor : MonoBehaviour, DangerLevelIncrease
{
    [SerializeField] int unlockAtCureLevel;

    Spawner mySpawner;

    private void Start()
    {
        PacingController.pacingController.AddToNotify(this);
        mySpawner = transform.GetComponentInChildren<Spawner>();
    }

    public void IncreaseLevel(int level)
    {
        if(unlockAtCureLevel == level)
        {
            gameObject.SetActive(false);
        }
    }
}
