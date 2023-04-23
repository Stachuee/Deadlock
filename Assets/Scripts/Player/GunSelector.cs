using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelector : MonoBehaviour
{
    GunController gunController;
    Inventory inventory;
    [SerializeField] int gunIndex;
    void Start()
    {
        gunController = FindObjectOfType<GunController>();
        inventory = FindObjectOfType<Inventory>();
    }

    public void SelectGun()
    {
        gunController.ChangeWeapon(gunIndex);
        inventory.EnableInventory(false);
    }

}
