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

    private void Start()
    {
        owner = transform.GetComponentInParent<PlayerController>();
    }

    public abstract void Reload();
    public abstract void Shoot();

}
