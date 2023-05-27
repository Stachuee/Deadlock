using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : GunBase
{
    readonly int MAX_AMMO = 100;

    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float usagePerSecond;

    [SerializeField] Transform particleHandle;
    [SerializeField] ParticleSystem fireVFX;
    [SerializeField] ParticleSystem iceVFX;

    bool startedShooting;

    [SerializeField] Sprite ammoIcon;
    [SerializeField] float maxAmmo;
    [SerializeField] float currentAmmo;
    [SerializeField] Sprite ammoIceIcon;
    [SerializeField] float maxIceAmmo;
    [SerializeField] float currentIceAmmo;

    int fireMode;
    int targetFireMode;

    GunController gunController;


    protected override void Start()
    {
        gunController = FindObjectOfType<GunController>();
        base.Start();
        currentAmmo = MAX_AMMO;
        currentIceAmmo = MAX_AMMO;
    }

    public override void AddAmmo(AmmoType aT, int amount)
    {
        switch (aT)
        {
            case AmmoType.Bullet:
                currentAmmo += amount;
                break;
            case AmmoType.Ice:
                currentIceAmmo += amount;
                break;
            default:
                Debug.LogError($"Wrong AmmoType({aT}) for Firegun!");
                break;
        }
    }

    private void OnDisable()
    {
        fireVFX.Stop();
        iceVFX.Stop();
        StopReload();
    }

    override protected void Update()
    {
        base.Update();
        //firePS.transform.rotation = transform.rotation;
        //firePS.transform.position = barrel.position;

        if (reloading) return;

        if ((fireMode == 0 && currentAmmo <= 0) || (fireMode == 1 && currentIceAmmo <= 0))
        {
            if (fireMode == 0 && maxAmmo > 0 || fireMode == 1 && maxIceAmmo > 0)
            {
                Reload();
                return;
            }
            else return;
        }

        if (isShooting && ((fireMode == 0 && currentAmmo <= 0 && currentAmmo > 0) || (fireMode == 1 && currentIceAmmo <= 0 && currentIceAmmo > 0)))
        {
            if (fireMode == 0) currentAmmo -= usagePerSecond * Time.deltaTime;
            else currentIceAmmo -= usagePerSecond * Time.deltaTime;

            particleHandle.position = barrel.position;
            particleHandle.rotation = Quaternion.Euler(0, 0, rot_z);

            if (!startedShooting)
            {
                if(fireMode == 0)
                {
                    fireVFX.Play();
                    startedShooting = true;
                }
                else if(fireMode == 1)
                {
                    iceVFX.Play();
                    startedShooting = true;
                }
            }
        }
        else
        {   
            if(startedShooting)
            {
                fireVFX.Stop();
                iceVFX.Stop();
                startedShooting = false;
            }
        }

    }

    override public void ChangeBulletType(bool input)
    {
        if (input )
        {
            StopReload();
            fireVFX.Stop();
            iceVFX.Stop();
            startedShooting = false;
            targetFireMode = (fireMode + 1) % 2;
            Reload(true);
        }
    }

    override public string GetAmmoAmount()
    {
        if (fireMode == 0) return Mathf.CeilToInt(currentAmmo).ToString() + "/" + Mathf.CeilToInt(maxAmmo).ToString();
        else if (fireMode == 1) return Mathf.CeilToInt(currentIceAmmo).ToString() + "/" + Mathf.CeilToInt(maxIceAmmo).ToString();
        else return "";
    }


    public override string GetBothAmmoString()
    {
        return Mathf.CeilToInt(currentAmmo + maxAmmo) + " " + Mathf.CeilToInt(currentIceAmmo + maxIceAmmo);
    }

    public override void RefillAmmo()
    {
        fireMode = targetFireMode;
        if (fireMode == 0)
        {
            maxAmmo += currentAmmo;
            currentAmmo = Mathf.Min(MAX_AMMO, maxAmmo);
            maxAmmo -= currentAmmo;
        }
        else if (fireMode == 1)
        {
            maxIceAmmo += currentIceAmmo;
            currentIceAmmo = Mathf.Min(MAX_AMMO, maxIceAmmo);
            maxIceAmmo -= currentIceAmmo;
        }
    }

    protected override bool IsFullOnAmmo()
    {
        if (fireMode == 0) return currentAmmo == MAX_AMMO;
        else return currentIceAmmo == MAX_AMMO;
    }

    public override Sprite GetAmmoIcon()
    {
        if (fireMode == 0) return ammoIcon;
        else return ammoIceIcon;
    }
}