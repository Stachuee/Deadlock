using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
