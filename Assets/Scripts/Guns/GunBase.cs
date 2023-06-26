using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase : MonoBehaviour, IGun
{
    [SerializeField] protected Transform barrel;

    [SerializeField] SpriteRenderer gunSprite;
    [SerializeField] Transform gunTransform;
    [SerializeField] bool useLaser;
    [SerializeField] LineRenderer laser;
    [SerializeField] protected LayerMask laserLayerMask;

    [SerializeField] protected GameObject barrelFlash;

    [SerializeField] protected AudioSource shotAudio;
    [SerializeField] protected AudioSource reloadSFX;

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


    protected bool reloading;
    [SerializeField] float reloadTime;
    float reloadTimer;


    protected int fireMode;
    int targetFireMode;

    [SerializeField] Transform mirror;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        aimVector = owner.currentAimDirection;

        currentRecoilAngle = Mathf.SmoothDampAngle(currentRecoilAngle, 0, ref recoilVel, recoilDamping);
        currentBarrelAngle = Mathf.SmoothDampAngle(currentBarrelAngle, currentRecoilAngle, ref currentBarrelAngle, barrelDamping);

        aimVectorWithRecoil = Quaternion.AngleAxis(currentBarrelAngle, Vector3.forward) * aimVector;
        if (useLaser)
        {
            laser.SetPosition(0, barrel.position);
            RaycastHit2D hit = Physics2D.Raycast(barrel.position, aimVectorWithRecoil, 100, ~laserLayerMask);
            if (hit) laser.SetPosition(1, hit.point);
            else laser.SetPosition(1, barrel.position);
        }

        barrelFlash.transform.position = barrel.position + (barrel.position - transform.position).normalized * 0.5f;

        rot_z = Mathf.Atan2(aimVectorWithRecoil.y, aimVectorWithRecoil.x) * Mathf.Rad2Deg;
        
        
        gunTransform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        if (rot_z > 90 || rot_z < -90)
        {
            mirror.localRotation = Quaternion.Euler(180, 0, 0);
            gunSprite.flipY = true;
        }
        else
        {
            mirror.localRotation = Quaternion.Euler(0, 0, 0);
            gunSprite.flipY = false;
        }


        if (reloading)
        {
            if (reloadTimer >= reloadTime)
            {
                RefillAmmo();
                StopReload();
            }
            owner.uiController.combatHUDController.UpdateReload(1 - reloadTimer / reloadTime);
            reloadTimer += Time.deltaTime;
        }

    }

    public virtual void RefillAmmo()
    {
        fireMode = targetFireMode;
    }

    public virtual void Reload(bool forceReload = false)
    {
        if (!forceReload && (reloading || IsFullOnAmmo())) return; //
        reloading = true;
        reloadTimer = 0;
        reloadSFX.Play();
    }

    public void StopReload()
    {
        reloading = false;
        owner.uiController.combatHUDController.UpdateReload(0);
    }

    protected abstract bool IsFullOnAmmo();

    public abstract void AddAmmo(AmmoType aT, int amount);

    public virtual void ChangeBulletType(bool input)
    {
        if (input)
        {
            StopReload();
            targetFireMode = (fireMode + 1) % 2;
            Reload(true);
        }
    }

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
    //public abstract DamageType GetBulletType();

    public void EnableGun(bool isActive)
    {
        if (gameObject.activeInHierarchy == isActive) return;
        gameObject.SetActive(isActive);
    }

    public GameObject GetInventorySlotPrefab()
    {
        return inventorySlotPrefab;
    }

    public abstract string GetBothAmmoString();

    public abstract Sprite GetAmmoIcon();
    private void OnEnable()
    {
        if(owner == null)
        {
            owner = transform.GetComponentInParent<PlayerController>();
        }

        laser.gameObject.SetActive(useLaser);
        targetFireMode = fireMode;

        rot_z = Mathf.Atan2(owner.currentAimDirection.y, owner.currentAimDirection.x) * Mathf.Rad2Deg;
        if (rot_z > 90 || rot_z < -90) gunSprite.flipY = true;
        else gunSprite.flipY = false;
        gunTransform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    protected virtual void OnDisable()
    {
        StopReload();
    }

}
