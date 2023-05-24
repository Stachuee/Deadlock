using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : GunBase
{
    [SerializeField] private List<AmmoType> laserTypeList;

    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] float damage = 5f;
    [SerializeField] float armorAndResistShreadPerSecond;
    [SerializeField] float maxDamage;
    [SerializeField] float damageIncreasePerSecond;

    [SerializeField] float maxAmmo;
    [SerializeField] float ammoUssagePerSecond;
    [SerializeField] float currentAmmo;
    [SerializeField] float restoreAmmo;

    [SerializeField, MinMaxSlider(0, 2)] Vector2 laserMinWidth;

    private float baseDamage;

    private Transform lastTargetHit = null;
    ITakeDamage lastHitITakeDamage;



    AmmoType laserType;
    int currentAmmoTypeId = 0;
    protected override void Start()
    {
        base.Start();
        baseDamage = damage;
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
            case AmmoType.Repair:
                restoreAmmo += amount;
                break;
            default:
                Debug.LogError($"Wrong AmmoType({aT}) for Laser!");
                break;
        }
    }


    protected override void Update()
    {
        base.Update();
        if(laserType == AmmoType.Bullet)
        {
            if (isShooting && currentAmmo > 0)
            {
                //if (Time.time < shootTimer + fireRate)
                //{
                //    ableToDamage = false;
                //    currentAmmo -= 5;
                //}
                //else ableToDamage = true;
                lineRenderer.enabled = true;

                RaycastHit2D hit;

                if (hit = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, ~laserLayerMask))
                {
                    DrawRay(firePoint.position, hit.point);
                    if (hit.transform.tag == "Enemy")
                    {
                        if(lastTargetHit != hit.transform)
                        {
                            lastTargetHit = hit.transform;
                            lastHitITakeDamage = hit.transform.GetComponent<ITakeDamage>();
                            damage = baseDamage;
                        }

                        if (lastHitITakeDamage != null)
                        {
                            lastHitITakeDamage.TakeDamage(damage * Time.deltaTime, DamageType.Bullet);
                            lastHitITakeDamage.TakeArmorDamage(DamageType.Bullet, armorAndResistShreadPerSecond * Time.deltaTime);
                            lastHitITakeDamage.TakeArmorDamage(DamageType.Ice, armorAndResistShreadPerSecond * Time.deltaTime);
                            lastHitITakeDamage.TakeArmorDamage(DamageType.Fire, armorAndResistShreadPerSecond * Time.deltaTime);
                            lastHitITakeDamage.TakeArmorDamage(DamageType.Mele, armorAndResistShreadPerSecond * Time.deltaTime);
                            damage = Mathf.Clamp(damage + damageIncreasePerSecond * Time.deltaTime, damage, maxDamage);
                        }
                    }
                    else
                    {
                        damage = baseDamage;
                    }
                }
                else
                {
                    lastTargetHit = null;
                    damage = baseDamage;
                }
                currentAmmo -= ammoUssagePerSecond * Time.deltaTime;
            }

            else
            {
                lastTargetHit = null;
                damage = baseDamage;
                lineRenderer.enabled = false;
            }
        }
        else if (laserType == AmmoType.Repair)
        {
            if (isShooting && currentAmmo > 0)
            {
                //todo
            }
            }
        
    }

    override public void ChangeBulletType(bool input)
    {
        if (input)
        {
            currentAmmoTypeId = (currentAmmoTypeId + 1) % laserTypeList.Count;
            laserType = laserTypeList[currentAmmoTypeId];
        }
    }

    override public string GetAmmoAmount()
    {
        return (Mathf.FloorToInt(currentAmmo)).ToString();
    }

    override public DamageType GetBulletType()
    {
        return DamageType.Bullet;
    }


    void DrawRay(Vector2 startPosition, Vector2 endPosition)
    {
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
        float laserWidth = Mathf.Lerp(laserMinWidth.x, laserMinWidth.y, (damage - baseDamage) / (maxDamage - baseDamage));
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
    }

    public override string GetBothAmmoString()
    {
        return Mathf.FloorToInt(currentAmmo) + " " + Mathf.FloorToInt(restoreAmmo);
    }
}
