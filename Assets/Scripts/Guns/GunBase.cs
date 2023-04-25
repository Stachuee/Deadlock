using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase : MonoBehaviour, IGun
{
    [SerializeField] protected Transform barrel;
    [SerializeField] protected GameObject bulletPrefab;
    
    
    protected PlayerController owner;

    [SerializeField] protected GameObject inventorySlotPrefab;

    protected float isShooting = 0;

    protected virtual void Start()
    {
        owner = transform.GetComponentInParent<PlayerController>();
    }

    public abstract void Reload();

    public abstract void AddAmmo(AmmoType aT, int amount);

    public abstract void ChangeBulletType(float input);


    public void Shoot(float _isShooting)
    {
        isShooting = _isShooting;
    }

    public Transform GetGunTransform()
    {
        return transform;
    }

    public GunBase GetGunScript()
    {
        return GetComponent<GunBase>();
    }

    public Transform GetBarrelTransform()
    {
        return GetComponentInChildren<Transform>();
    }

    public abstract int GetAmmoAmount();
    //To Change
    public abstract DamageType GetBulletType();

    public void EnableGun(bool isActive)
    {
        gameObject.SetActive(isActive);
    }



    public GameObject GetInventorySlotPrefab()
    {
        return inventorySlotPrefab;
    }


}
