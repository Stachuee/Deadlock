using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Pistol, ARiffle, RPG, Firegun, Laser, Harpoon }
public enum AmmoType { Bullet, Fire, Poison, Ice, Precise, Proximity, Repair, Disintegrating }

public class GunController : MonoBehaviour
{
    [SerializeField]
    Transform gunTransform;
    [SerializeField]
    Transform barrel;

    [SerializeField]
    LineRenderer laser;
    [SerializeField]
    LayerMask laserMask;

    SpriteRenderer gunSprite;

    PlayerController playerController;

    [SerializeField] private List<GunBase> weapons;
    private int currentWeaponIndex = 0;

    [SerializeField] private InventorySelector inventorySelector;

    GunBase gun;

    ParticleSystem effectToDeactivate = null; // if some PS can stay in scene but are unnecessary, make it inactive after changing the gun 

    bool active = true;

    private void Awake()
    {
        playerController = transform.GetComponent<PlayerController>();
        gunSprite = gunTransform.GetComponent<SpriteRenderer>();
        
        
        foreach (GunBase weapon in weapons)
        {
            weapon.EnableGun(false);
        }

        if (playerController.isScientist)
        {
            active = false;
            return;
        }
        currentWeaponIndex = 0;
        weapons[currentWeaponIndex].EnableGun(true);

        gunTransform = weapons[currentWeaponIndex].GetGunTransform();
        barrel = weapons[currentWeaponIndex].GetBarrelTransform();
        gun = weapons[currentWeaponIndex].GetGunScript();
    }

    private void Start()
    {

        inventorySelector.ActivateSlot(WeaponType.Pistol);
        inventorySelector.ActivateSlot(WeaponType.Firegun);
        inventorySelector.ActivateSlot(WeaponType.ARiffle);
        inventorySelector.ActivateSlot(WeaponType.Pistol);
        inventorySelector.ActivateSlot(WeaponType.Laser);
        inventorySelector.ActivateSlot(WeaponType.RPG);
        inventorySelector.ActivateSlot(WeaponType.Harpoon);

    }


    public void AddAmmo(WeaponType weaponType, AmmoType ammoType, int amount)
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                weapons[0].AddAmmo(ammoType, amount);
                break;
            case WeaponType.ARiffle:
                weapons[1].AddAmmo(ammoType, amount);
                break;
            case WeaponType.RPG:
                weapons[2].AddAmmo(ammoType, amount);
                break;
            case WeaponType.Firegun:
                weapons[3].AddAmmo(ammoType, amount);
                break;
            case WeaponType.Laser:
                weapons[4].AddAmmo(ammoType, amount);
                break;
            case WeaponType.Harpoon:
                weapons[5].AddAmmo(ammoType, amount);
                break;
        }
    }


    public string GetAmmoString(WeaponType type)
    {
        return weapons[(int)type].GetBothAmmoString();
    }

    public void Reload()
    {
        if (!active) return;
        gun.Reload();
    }


    //public void ChangeWeapon(int gunIndex)
    //{
    //    weapons[currentWeaponIndex].EnableGun(false);
    //    currentWeaponIndex = gunIndex;

    //    gunTransform = weapons[currentWeaponIndex].GetGunTransform();
    //    barrel = weapons[currentWeaponIndex].GetBarrelTransform();
    //    gun = weapons[currentWeaponIndex].GetGunScript();

    //    weapons[currentWeaponIndex].EnableGun(true);

    //    if (effectToDeactivate != null)
    //    {
    //        effectToDeactivate.enableEmission = false;
    //    }
    //}

    public void ChangeWeapon(WeaponType type)
    {
        if (!active) return;
        if (currentWeaponIndex == (int)type) return;
        weapons[currentWeaponIndex].EnableGun(false);
        currentWeaponIndex = (int)type;
        
        gunTransform = weapons[currentWeaponIndex].GetGunTransform();
        barrel = weapons[currentWeaponIndex].GetBarrelTransform();
        gun = weapons[currentWeaponIndex].GetGunScript();

        weapons[currentWeaponIndex].EnableGun(true);

        //if (effectToDeactivate != null)
        //{
        //    effectToDeactivate.enableEmission = false;
        //}
    }


    public void ChangeBullet(bool input)
    {
        if (!active) return;
        gun.ChangeBulletType(input);

    }

    public void ShootGun(bool isShooting)
    {
        if (!active) return;
        gun.Shoot(isShooting);
    }

    public GunBase GetCurrentGun()
    {
        return gun;
    }
}
