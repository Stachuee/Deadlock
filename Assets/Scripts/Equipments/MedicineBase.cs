using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MedicineBase : MonoBehaviour
{
    [SerializeField] GameObject inventorySlotPrefab;

    public abstract void AddEffect(PlayerController pC);

    public GameObject GetInventorySlotPrefab()
    {
        return inventorySlotPrefab;
    }
}
