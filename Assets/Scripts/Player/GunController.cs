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

    [SerializeField] private Inventory inventory;

    [SerializeField] private InventorySelector inventorySelector;

    GunBase gun;

    ParticleSystem effectToDeactivate = null; // if some PS can stay in scene but are unnecessary, make it inactive after changing the gun 



    private void Awake()
    {
        playerController = transform.GetComponent<PlayerController>();
        gunSprite = gunTransform.GetComponent<SpriteRenderer>();

        foreach (GunBase weapon in weapons)
        {
            weapon.EnableGun(false);
        }
        currentWeaponIndex = 0;
        weapons[currentWeaponIndex].EnableGun(true);

        gunTransform = weapons[currentWeaponIndex].GetGunTransform();
        barrel = weapons[currentWeaponIndex].GetBarrelTransform();
        gun = weapons[currentWeaponIndex].GetGunScript();
    }

    private void Start()
    {
        /*inventory.AddGun(weapons[1].GetInventorySlotPrefab());
        inventory.AddGun(weapons[2].GetInventorySlotPrefab());
        inventory.AddGun(weapons[3].GetInventorySlotPrefab());
        inventory.AddGun(weapons[4].GetInventorySlotPrefab());
        inventory.AddGun(weapons[5].GetInventorySlotPrefab());*/
        inventorySelector.AddSlot(SlotType.Weapon, 1);
        inventorySelector.AddSlot(SlotType.Weapon, 2);
        inventorySelector.AddSlot(SlotType.Weapon, 3);
        inventorySelector.AddSlot(SlotType.Weapon, 4);
        inventorySelector.AddSlot(SlotType.Weapon, 5);


    }

    private void Update()
    {
        RotateGun();
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


    public void Reload()
    {
        gun.Reload();
    }


    public void ChangeWeapon(int gunIndex)
    {
        weapons[currentWeaponIndex].EnableGun(false);
        currentWeaponIndex = gunIndex;

        gunTransform = weapons[currentWeaponIndex].GetGunTransform();
        barrel = weapons[currentWeaponIndex].GetBarrelTransform();
        gun = weapons[currentWeaponIndex].GetGunScript();

        weapons[currentWeaponIndex].EnableGun(true);

        if (effectToDeactivate != null)
        {
            effectToDeactivate.enableEmission = false;
        }
    }


    public void ChangeBullet(float input)
    {
        gun.ChangeBulletType(input);

    }

    void RotateGun()
    {
        //Debug.Log(playerController.currentAimDirection.normalized);
        Vector2 diff = (playerController.currentAimDirection).normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (rot_z > 90 || rot_z < -90) gunSprite.flipY = true;
        else gunSprite.flipY = false;

        gunTransform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        RaycastHit2D hit = Physics2D.Raycast(barrel.position, diff, Mathf.Infinity, ~laserMask);

        if (hit.collider != null)
        {
            laser.SetPosition(0, barrel.position);
            laser.SetPosition(1, hit.point);
        }
    }

    public void ShootGun(float isShooting)
    {
        gun.Shoot(isShooting);
    }

    public void SetSelectedSlot()
    {
        playerController.uiController.myEventSystem.SetSelectedGameObject(inventory.GetSelectedSlot());
    }

    public void SetEffectToDeactivate(ParticleSystem ps)
    {
        effectToDeactivate = ps;
    }

    public GunBase GetCurrentGun()
    {
        return gun;
    }
}
