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

    [SerializeField]
    GunBase gun;

    private void Awake()
    {
        playerController = transform.GetComponent<PlayerController>();
        gunSprite = gunTransform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        RotateGun();
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

    public void ShootGun()
    {
        gun.Shoot();
    }
}
