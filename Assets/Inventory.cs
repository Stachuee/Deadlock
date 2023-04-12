using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform gunsInventory;
    [SerializeField] Transform equipmentInventory;

    PlayerController playerController;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void AddGun(GameObject gunSlotPrefab)
    {
        GameObject inventoryElement = Instantiate(gunSlotPrefab, gunsInventory);
       // playerController.uiController.myEventSystem.SetSelectedGameObject(inventoryElement);
    }

    public void AddEquipment(GameObject equipmentSlotPrefab)
    {
        Instantiate(equipmentSlotPrefab, equipmentInventory);
    }
}
