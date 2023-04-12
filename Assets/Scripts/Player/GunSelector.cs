using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelector : MonoBehaviour
{
    GunController gunController;
    [SerializeField] int gunIndex;
    void Start()
    {
        gunController = FindObjectOfType<GunController>();
    }

    public void SelectGun()
    {
        gunController.ChangeWeapon(gunIndex);
    }
}
