using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : GunBase
{
    [SerializeField] private List<AmmoType> laserTypeList;

    [SerializeField] float fireRate = 0.1f;
    private float shootTimer = 0f; // time elapsed since last shot

    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] LayerMask maskToIgnore;

    [SerializeField] float damage = 5f;
    bool ableToDamage = true;

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] int restoreAmmo;

    [SerializeField] private float currentHitTime = 0f;
    [SerializeField] private float damagePerSecond = 1f;
    private ITakeDamage lastTargetHit = null;
    private float baseDamage;

    AmmoType laserType;
    int currentAmmoTypeId = 0;
    protected override void Start()
    {
        base.Start();
        baseDamage = damage;
    }
    public override void Reload()
    {
        currentAmmo = maxAmmo;
    }

    public override void AddAmmo(AmmoType aT, int amount)
    {
        switch (aT)
        {
            case AmmoType.Bullet:
                currentAmmo += amount;
                break;
            case AmmoType.Repair:
                restoreAmmo += amount;
                break;
            default:
                Debug.LogError($"Wrong AmmoType({aT}) for Laser!");
                break;
        }
    }


    void Update()
    {
        if(laserType == AmmoType.Bullet)
        {
            if (isShooting >= 0.9f && currentAmmo > 0)
            {
                if (Time.time < shootTimer + fireRate)
                {
                    ableToDamage = false;
                    currentAmmo -= 5;
                }
                else ableToDamage = true;
                lineRenderer.enabled = true;

                if (Physics2D.Raycast(transform.position, transform.right))
                {
                    RaycastHit2D hit = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, ~maskToIgnore);
                    DrawRay(firePoint.position, hit.point);
                    if (hit.collider != null && ableToDamage)
                    {
                        ITakeDamage target = hit.transform.GetComponent<ITakeDamage>();

                        if (target != null)
                        {
                            if (target == lastTargetHit)
                            {
                                // Increase the damage based on how long the laser has been hitting the enemy
                                currentHitTime += Time.deltaTime;
                                damage += currentHitTime * damagePerSecond;
                            }
                            else
                            {
                                // Reset the hit time and damage for a new enemy
                                lastTargetHit = target;
                                currentHitTime = 0f;
                                damage = baseDamage;
                            }
                            target.TakeDamage(damage, DamageType.Bullet);
                            target.TakeArmorDamage(DamageType.Bullet, 0.1f);
                            target.TakeArmorDamage(DamageType.Ice, 0.1f);
                            target.TakeArmorDamage(DamageType.Fire, 0.1f);
                            target.TakeArmorDamage(DamageType.Mele, 0.1f);
                        }
                        shootTimer = Time.time;
                    }
                }
                else
                {
                    lastTargetHit = null;
                    currentHitTime = 0f;
                    damage = baseDamage;
                }
            }

            else
            {
                lastTargetHit = null;
                currentHitTime = 0f;
                damage = baseDamage;
                lineRenderer.enabled = false;
            }
        }
        else if (laserType == AmmoType.Repair)
        {
            if (isShooting >= 0.9f && currentAmmo > 0)
            {
                //todo
            }
            }
        
    }

    override public void ChangeBulletType(float input)
    {
        if (input >= 1)
        {
            currentAmmoTypeId = (currentAmmoTypeId + 1) % laserTypeList.Count;
            laserType = laserTypeList[currentAmmoTypeId];
        }
    }

    override public int GetAmmoAmount()
    {
        return currentAmmo;
    }

    override public DamageType GetBulletType()
    {
        return DamageType.Bullet;
    }


    void DrawRay(Vector2 startPosition, Vector2 endPosition)
    {
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

}
