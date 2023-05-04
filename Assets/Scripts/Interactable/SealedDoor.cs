using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealedDoor : MonoBehaviour, ICureLevelIncrease
{
    [SerializeField] int unlockAtCureLevel;

    Spawner mySpawner;

    private void Start()
    {
        CureController.instance.AddToNotify(this);
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
