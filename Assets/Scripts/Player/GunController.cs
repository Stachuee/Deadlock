using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private List<GameObject> weapons;
    private int currentWeaponIndex = 0;

    [SerializeField]
    GunBase gun;

    private void Awake()
    {
        playerController = transform.GetComponent<PlayerController>();
        gunSprite = gunTransform.GetComponent<SpriteRenderer>();

        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapons[currentWeaponIndex].SetActive(true);
    }

    private void Update()
    {
        RotateGun();
    }

    public void ChangeWeapon(float scrollInput)
    {
        if (scrollInput >= 1)
        {
            // Scroll up: activate the next weapon in the list
            weapons[currentWeaponIndex].SetActive(false);
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            weapons[currentWeaponIndex].SetActive(true);
            gunTransform = weapons[currentWeaponIndex].GetComponent<Transform>();
            barrel = weapons[currentWeaponIndex].GetComponentInChildren<Transform>();
            gun = weapons[currentWeaponIndex].GetComponent<GunBase>();

        }
        else if (scrollInput <= -1)
        {
            // Scroll down: activate the previous weapon in the list
            weapons[currentWeaponIndex].SetActive(false);
            currentWeaponIndex = (currentWeaponIndex - 1 + weapons.Count) % weapons.Count;
            weapons[currentWeaponIndex].SetActive(true);
            gunTransform = weapons[currentWeaponIndex].GetComponent<Transform>();
            barrel = weapons[currentWeaponIndex].GetComponentInChildren<Transform>();
            gun = weapons[currentWeaponIndex].GetComponent<GunBase>();
        }
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

        if(hit.collider != null)
        {
            laser.SetPosition(0, barrel.position);
            laser.SetPosition(1, hit.point);
        }
    }

    public void ShootGun(float isShooting)
    {
        gun.Shoot(isShooting);
    }
}
