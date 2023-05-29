using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : GunBase
{
    readonly float MAX_AMMO = 100;

    [SerializeField] protected LayerMask laserHealingLayerMask;


    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] float damage = 5f;
    [SerializeField] float healing = 5f;
    [SerializeField] float armorAndResistShreadPerSecond;
    [SerializeField] float maxDamage;
    [SerializeField] float damageIncreasePerSecond;


    [SerializeField] float ammoUssagePerSecond;

    [SerializeField] Sprite ammoIcon;
    [SerializeField] float maxAmmo;
    [SerializeField] float currentAmmo;
    [SerializeField] Sprite ammoRestoreIcon;
    [SerializeField] float maxRestoreAmmo;
    [SerializeField] float currentRestoreAmmo;

    [SerializeField, MinMaxSlider(0, 2)] Vector2 laserMinWidth;

    [SerializeField] GameObject laserStart;
    ParticleSystem laserStartParticle;
    [SerializeField] GameObject laserEnd;
    ParticleSystem laserEndParticle;

    bool LaserActive
    {
        set
        {
            if(laserActive != value)
            {
                laserActive = value;
                lineRenderer.enabled = value;
                if(value)
                {
                    laserStartParticle.Play();
                    laserEndParticle.Play();
                }
                else
                {
                    laserStartParticle.Stop();
                    laserEndParticle.Stop();
                }
            }
        }
        get
        {
            return laserActive;
        }
    }
    bool laserActive;

    private float baseDamage;

    private Transform lastTargetHit = null;
    ITakeDamage lastHitITakeDamage;

    int fireMode;
    int targetFireMode;

    protected override void Start()
    {
        base.Start();
        baseDamage = damage;
        currentAmmo = MAX_AMMO;
        currentRestoreAmmo = MAX_AMMO;

        laserStartParticle = laserStart.GetComponentInChildren<ParticleSystem>();
        laserEndParticle = laserEnd.GetComponentInChildren<ParticleSystem>();

    }

    public override void AddAmmo(AmmoType aT, int amount)
    {
        switch (aT)
        {
            case AmmoType.Bullet:
                currentAmmo += amount;
                break;
            case AmmoType.Repair:
                currentRestoreAmmo += amount;
                break;
            default:
                Debug.LogError($"Wrong AmmoType({aT}) for Laser!");
                break;
        }
    }


    protected override void Update()
    {
        base.Update();

        if (reloading)
        {
            LaserActive = false;
            return;
        }

        if ((fireMode == 0 && currentAmmo <= 0) || (fireMode == 1 && currentRestoreAmmo <= 0))
        {
            if (fireMode == 0 && maxAmmo > 0 || fireMode == 1 && maxRestoreAmmo > 0)
            {
                Reload();
                return;
            }
            else return;
        }

        if (fireMode == 0)
        {
            if (isShooting && currentAmmo > 0)
            {
                LaserActive = true;

                RaycastHit2D hit;

                if (hit = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, ~laserLayerMask))
                {
                    DrawRay(firePoint.position, hit.point);
                    
                    laserStart.transform.position = firePoint.position;
                    laserStart.transform.rotation = Quaternion.Euler(0, 0, rot_z);
                    laserEnd.transform.position = hit.point;
                    laserEnd.transform.rotation = Quaternion.Euler(0, 0, rot_z - 180);

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
                            lastHitITakeDamage.TakeDamage(damage * Time.deltaTime);
                            lastHitITakeDamage.TakeArmorDamage(armorAndResistShreadPerSecond * Time.deltaTime);
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
                LaserActive = false;
            }
        }
        else if (fireMode == 1)
        {
            if (isShooting && currentRestoreAmmo > 0)
            {
                LaserActive = true;

                RaycastHit2D hit;

                if (hit = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, ~laserHealingLayerMask))
                {
                    DrawRay(firePoint.position, hit.point);

                    laserStart.transform.position = firePoint.position;
                    laserStart.transform.rotation = Quaternion.Euler(0, 0, rot_z);
                    laserEnd.transform.position = hit.point;
                    laserEnd.transform.rotation = Quaternion.Euler(0, 0, rot_z - 180);

                    if (hit.transform.tag == "Interactable")
                    {
                        if (lastTargetHit != hit.transform)
                        {
                            lastTargetHit = hit.transform;
                            lastHitITakeDamage = hit.transform.GetComponent<ITakeDamage>();
                        }

                        if (lastHitITakeDamage != null)
                        {
                            lastHitITakeDamage.Heal(healing * Time.deltaTime);
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
                currentRestoreAmmo -= ammoUssagePerSecond * Time.deltaTime;
            }

            else
            {
                lastTargetHit = null;
                damage = baseDamage;
                LaserActive = false;
            }
        }
    }

    override public void ChangeBulletType(bool input)
    {
        if (input)
        {
            StopReload();
            fireMode = (fireMode + 1) % 2;
            Reload(true);
        }
    }

    override public string GetAmmoAmount()
    {
        if (fireMode == 0) return Mathf.CeilToInt(currentAmmo).ToString() + "/" + Mathf.CeilToInt(maxAmmo).ToString();
        else if (fireMode == 1) return Mathf.CeilToInt(currentRestoreAmmo).ToString() + "/" + Mathf.CeilToInt(maxRestoreAmmo).ToString();
        else return "";
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
        return Mathf.CeilToInt(currentAmmo + maxAmmo) + " " + Mathf.CeilToInt(currentRestoreAmmo + maxRestoreAmmo);
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
            maxRestoreAmmo += currentRestoreAmmo;
            currentRestoreAmmo = Mathf.Min(MAX_AMMO, maxRestoreAmmo);
            maxRestoreAmmo -= currentRestoreAmmo;
        }
    }

    private void OnDisable()
    {
        StopReload();
    }

    private void OnEnable()
    {
        targetFireMode = fireMode;
    }

    protected override bool IsFullOnAmmo()
    {
        if (fireMode == 0) return currentAmmo == MAX_AMMO;
        else return currentRestoreAmmo == MAX_AMMO;
    }

    public override Sprite GetAmmoIcon()
    {
        if (fireMode == 0) return ammoIcon;
        else return ammoRestoreIcon;
    }
}
