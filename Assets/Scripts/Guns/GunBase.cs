using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase : MonoBehaviour, IGun
{
    [SerializeField] protected Transform barrel;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int currentAmmo;
    protected PlayerController owner;

    protected float isShooting = 0;

    private void Start()
    {
        owner = transform.GetComponentInParent<PlayerController>();
    }

    public abstract void Reload();
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

    public void EnableGun(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void ChangeBulletType(GameObject _bulletPrefab)
    {
        bulletPrefab = _bulletPrefab;
    }


}
