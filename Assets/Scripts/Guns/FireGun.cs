using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : GunBase
{
    [SerializeField] float fireRate = 0.1f;

    [SerializeField] ParticleSystem fireVFX;
    [SerializeField] ParticleSystem iceVFX;

    bool startedShooting;

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int currentIceAmmo;

    int currentMode;


    GunController gunController;


    protected override void Start()
    {
        gunController = FindObjectOfType<GunController>();
        base.Start();

    }

    public override void Reload()
    {

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
    }

    override protected void Update()
    {
        base.Update();
        //firePS.transform.rotation = transform.rotation;
        //firePS.transform.position = barrel.position;

        if (isShooting && currentAmmo > 0)
        {
            if(!startedShooting)
            {
                if(currentMode == 0)
                {
                    fireVFX.Play();
                    startedShooting = true;
                }
                else if(currentMode == 1)
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
            fireVFX.Stop();
            iceVFX.Stop();
            startedShooting = false;
            currentMode = (currentMode + 1) % 2;
        }
    }

    override public string GetAmmoAmount()
    {
        if (currentMode == 0) return currentAmmo.ToString();
        else if (currentMode == 1) return currentIceAmmo.ToString();
        else return "";
    }

    override public DamageType GetBulletType()
    {
        if (currentMode == 0) return DamageType.Fire;
        else return DamageType.Ice;
    }

    public override string GetBothAmmoString()
    {
        return currentAmmo + " " + currentIceAmmo;
    }
}