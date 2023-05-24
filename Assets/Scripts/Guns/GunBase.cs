using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase : MonoBehaviour, IGun, IControllSubscriberMovment
{
    [SerializeField] protected Transform barrel;

    [SerializeField] SpriteRenderer gunSprite;
    [SerializeField] Transform gunTransform;
    [SerializeField] bool useLaser;
    [SerializeField] LineRenderer laser;
    [SerializeField] protected LayerMask laserLayerMask;


    protected float currentRecoilAngle;
    [SerializeField] protected float recoilAnglePerShot;
    [SerializeField, Range(0f, 1f)] float recoilDamping;
    float recoilVel;

    protected float currentBarrelAngle;
    [SerializeField, Range(0f, 1f)] float barrelDamping;
    float barrelVel;

    protected float rot_z;

    protected Vector2 aimVectorWithRecoil;


    protected Vector2 aimVector;
    
    protected PlayerController owner;

    [SerializeField] protected GameObject inventorySlotPrefab;

    protected bool isShooting ;

    protected virtual void Start()
    {
        owner = transform.GetComponentInParent<PlayerController>();
        owner.AddMovmentSubscriber(this);
    }

    protected virtual void Update()
    {
        
        currentRecoilAngle = Mathf.SmoothDampAngle(currentRecoilAngle, 0, ref recoilVel, recoilDamping);
        currentBarrelAngle = Mathf.SmoothDampAngle(currentBarrelAngle, currentRecoilAngle, ref currentBarrelAngle, barrelDamping);

        aimVectorWithRecoil = Quaternion.AngleAxis(currentBarrelAngle, Vector3.forward) * aimVector;
        if (useLaser)
        {
            laser.SetPosition(0, barrel.position);
            laser.SetPosition(1, Physics2D.Raycast(barrel.position, aimVectorWithRecoil, 100, ~laserLayerMask).point);
        }

        rot_z = Mathf.Atan2(aimVectorWithRecoil.y, aimVectorWithRecoil.x) * Mathf.Rad2Deg;
        if (rot_z > 90 || rot_z < -90) gunSprite.flipY = true;
        else gunSprite.flipY = false;
        gunTransform.rotation = Quaternion.Euler(0f, 0f, rot_z);

    }

    public abstract void Reload();

    public abstract void AddAmmo(AmmoType aT, int amount);

    public abstract void ChangeBulletType(bool input);


    public void Shoot(bool _isShooting)
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

    public abstract string GetAmmoAmount();
    public abstract DamageType GetBulletType();

    public void EnableGun(bool isActive)
    {
        gameObject.SetActive(isActive);
    }



    public GameObject GetInventorySlotPrefab()
    {
        return inventorySlotPrefab;
    }

    public abstract string GetBothAmmoString();

    private void OnEnable()
    {
        if(owner != null)
        owner.AddMovmentSubscriber(this);
        laser.gameObject.SetActive(useLaser);
    }

    private void OnDisable()
    {
        owner.RemoveMovmentSubscriber(this);
    }

    public void ForwardCommandMovment(Vector2 controll)
    {
        aimVector = controll.normalized;
    }

}
