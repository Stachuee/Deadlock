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

    //ParticleSystem effectToDeactivate = null; // if some PS can stay in scene but are unnecessary, make it inactive after changing the gun 

    [SerializeField] AudioSource changeGunSFX;

    bool active = true;

    private void Awake()
    {
        playerController = transform.GetComponent<PlayerController>();
        gunSprite = gunTransform.GetComponent<SpriteRenderer>();
        
        
        foreach (GunBase weapon in weapons)
        {
            weapon.EnableGun(false);
        }




        gunTransform = weapons[currentWeaponIndex].GetGunTransform();
        barrel = weapons[currentWeaponIndex].GetBarrelTransform();
        gun = weapons[currentWeaponIndex].GetGunScript();
    }

//    active = false;
//            foreach (GunBase weapon in weapons)
//            {
//                weapon.EnableGun(false);
//            }
//return;


private void Start()
    {
        
        UnlockWeapon(WeaponType.Pistol);
        //UnlockWeapon(WeaponType.RPG);
        //UnlockWeapon(WeaponType.Laser);
        UnlockWeapon(WeaponType.ARiffle);
        //UnlockWeapon(WeaponType.Firegun);
        //UnlockWeapon(WeaponType.Harpoon);


        if (playerController.isScientist)
        {
            active = false;
        }
        else
        {
            currentWeaponIndex = 0;
            weapons[currentWeaponIndex].EnableGun(true);
        }

    }

    public void AddAmmo(WeaponType weaponType, AmmoType ammoType, int amount)
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                weapons[0].AddAmmo(ammoType, Mathf.FloorToInt(amount * (1 + playerController.playerInfo.bonusAmmo)));
                break;
            case WeaponType.ARiffle:
                weapons[1].AddAmmo(ammoType, Mathf.FloorToInt(amount * (1 + playerController.playerInfo.bonusAmmo)));
                break;
            case WeaponType.RPG:
                weapons[2].AddAmmo(ammoType, Mathf.FloorToInt(amount * (1 + playerController.playerInfo.bonusAmmo)));
                break;
            case WeaponType.Firegun:
                weapons[3].AddAmmo(ammoType, Mathf.FloorToInt(amount * (1 + playerController.playerInfo.bonusAmmo)));
                break;
            case WeaponType.Laser:
                weapons[4].AddAmmo(ammoType, Mathf.FloorToInt(amount * (1 + playerController.playerInfo.bonusAmmo)));
                break;
            case WeaponType.Harpoon:
                weapons[5].AddAmmo(ammoType, Mathf.FloorToInt(amount * (1 + playerController.playerInfo.bonusAmmo)));
                break;
        }
    }

    public void UnlockWeapon(WeaponType weaponType)
    {
        inventorySelector.ActivateSlot(weaponType);
        if(GameController.scientist != null) GameController.scientist.uiController.upgradeGuide.UnlockGun(weaponType);
    }

    public GunBase GetGun(WeaponType type)
    {
        return weapons[(int)type];
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

        changeGunSFX.Play();

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
    public void PlayerDead(bool state)
    {
        weapons[currentWeaponIndex].EnableGun(!state);
    }

    public GunBase GetCurrentGun()
    {
        return gun;
    }
}
