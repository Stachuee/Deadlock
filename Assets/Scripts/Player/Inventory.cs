using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform gunsInventory;
    [SerializeField] Transform equipmentInventory;

    [SerializeField] GameObject pistolSlot;

    PlayerController playerController;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void AddGun(GameObject gunSlotPrefab)
    {
        Instantiate(gunSlotPrefab, gunsInventory);
    }

    public void AddEquipment(GameObject equipmentSlotPrefab)
    {
        Instantiate(equipmentSlotPrefab, equipmentInventory);
    }

    public void EnableInventory(bool isOn)
    {
        gameObject.SetActive(isOn);
    }

    public GameObject GetSelectedSlot()
    {
        return pistolSlot;
    }
}
