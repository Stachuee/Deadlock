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
       // playerController.uiController.myEventSystem.SetSelectedGameObject(inventoryElement);
    }

    public void AddEquipment(GameObject equipmentSlotPrefab)
    {
        Instantiate(equipmentSlotPrefab, equipmentInventory);
    }

    public GameObject GetSelectedSlot()
    {
        return pistolSlot;
    }
}
