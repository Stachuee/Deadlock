using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class ScientistTurret : PoweredInteractable, ITakeControll, IControllSubscriberMove, IControllSubscriberShoot
{
    readonly float TRAIL_LIFE_TIME = 0.05f;

    [MinMaxSlider(-180, 0)]
    public Vector2Int minMaxTurretAngle;

    [SerializeField] int damagePerBullet;
    bool automatic = true;

    bool controlling = false;

    [SerializeField]
    SpriteRenderer turretSprite;
    [SerializeField]
    Transform gunBarrel;
    [SerializeField] 
    Transform barrelTransform;

    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    float shootDelay;
    [SerializeField]
    float controllingShootDelay;
    float nextShot;

    float turretAngle;
    bool firing;

    PlayerController user;
    float prevRotz = 0;

    List<Transform> targets = new List<Transform>();
    Transform target;


    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] LineRenderer gunTrail;
    float trailDisapearTimer;
    bool trailShown;




    float rot_z;
    Vector2 direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (automatic && collision.tag == "Enemy")
        {
            targets.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(automatic && collision.tag == "Enemy")
        {
            targets.Remove(collision.transform);  
            if(collision.transform == target)
            {
                target = null;
                GetClosest();
            }
        }
    }

    private void Start()
    {
        //rot_z = gunBarrel.rotation.eulerAngles.z;
        //if (rot_z < minMaxTurretAngle.x || rot_z > minMaxTurretAngle.y) rot_z = prevRotz;
        //if (rot_z > 90 || rot_z < -90) turretSprite.flipY = true;
        //gunBarrel.rotation = Quaternion.Euler(0, 0, rot_z);
    }

    public void ForwardCommandAim(Vector2 controll, Vector2 controllSmooth)
    {
        if (!powered) return;
        Vector2 diff = (controllSmooth).normalized;
        rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (rot_z < minMaxTurretAngle.x || rot_z > minMaxTurretAngle.y) rot_z = prevRotz; // works only if minmax is <-180, 180>
        if (rot_z > 90 || rot_z < -90) turretSprite.flipY = true;
        else turretSprite.flipY = false;
        gunBarrel.rotation = Quaternion.Euler(0,0, rot_z);
        turretAngle = rot_z;
        prevRotz = rot_z;
    }

    public void ForwardCommandShoot(bool isShooting)
    {
        firing = isShooting;
    }

    protected override void Awake()
    {
        base.Awake();
        AddAction(TakeControll);
        StartCoroutine("GetClosestCoroutine");


        //if (!automatic) AddAction(TakeControll);
        //else
        //{
        //    StartCoroutine("GetClosestCoroutine");
        //}
    }
    private void Update()
    {
        if (trailShown && trailDisapearTimer <= Time.time)
        {
            gunTrail.transform.gameObject.SetActive(false);
            trailShown = false;
        }

        if(powered)
        {
            if (controlling)
            {
                if (firing && nextShot < Time.time)
                {
                    ManualShoot();
                    nextShot = Time.time + controllingShootDelay;
                }
            }
            else
            {
                if (target != null)
                {
                    Vector2 directionToTarget = (target.position - transform.position).normalized;
                    rot_z = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
                    if (rot_z > 90 || rot_z < -90) turretSprite.flipY = true;
                    else turretSprite.flipY = false;
                    gunBarrel.rotation = Quaternion.RotateTowards(gunBarrel.rotation, Quaternion.Euler(0, 0, rot_z), 180 * Time.deltaTime);

                    if (nextShot < Time.time)
                    {
                        Shoot();
                        nextShot = Time.time + shootDelay;
                    }
                }
            }
        }
        
    }

    IEnumerator GetClosestCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0f,1f));

        while (true)
        {
            if(powered && !controlling) GetClosest();
            yield return new WaitForSeconds(1);
        }
    }


    void GetClosest()
    {
        if (targets.Count > 0)
        {
            Transform closestTarget = target;
            float closestDistance = target != null ? Vector2.Distance(target.position, transform.position) : Mathf.Infinity;

            targets.ForEach(targetToCheck =>
            {
                float currentDistance = Vector2.Distance(targetToCheck.position, transform.position);
                if (closestTarget != targetToCheck && currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestTarget = targetToCheck;
                }
            });
            target = closestTarget;
        }
    }

    private void ManualShoot()
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(barrelTransform.position, new Vector2(Mathf.Cos(rot_z * Mathf.Deg2Rad), Mathf.Sin(rot_z * Mathf.Deg2Rad)), 100, enemyLayer))
        {
            Debug.Log(hit.transform.name);
            gunTrail.SetPosition(0, barrelTransform.position);
            gunTrail.SetPosition(1, hit.point);
            trailDisapearTimer = Time.time + TRAIL_LIFE_TIME;
            gunTrail.transform.gameObject.SetActive(true);
            trailShown = true;

            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<ITakeDamage>().TakeDamage(damagePerBullet, DamageSource.Turret);
            }
        }

    }

    private void Shoot()
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(barrelTransform.position, target.position - gunBarrel.position, 100, enemyLayer))
        {
            gunTrail.SetPosition(0, barrelTransform.position);
            gunTrail.SetPosition(1, hit.point);
            trailDisapearTimer = Time.time + TRAIL_LIFE_TIME;
            gunTrail.transform.gameObject.SetActive(true);
            trailShown = true;

            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<ITakeDamage>().TakeDamage(damagePerBullet, DamageSource.Turret);
            }

        }
    }


    public void TakeControll(PlayerController player, UseType type)
    {
        //if (automatic) return;
        user = player;
        controlling = true;
        player.AddMoveSubscriber(this);
        player.AddShootSubscriber(this);
    }

    public void Leave()
    {
        controlling = false;
        //user.RemoveAimSubscriber(this);
        user.RemoveMoveSubscriber(this);
        user.RemoveShootSubscriber(this);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + new Vector2(Mathf.Cos(minMaxTurretAngle.x * Mathf.Deg2Rad), Mathf.Sin(minMaxTurretAngle.x * Mathf.Deg2Rad)));
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + new Vector2(Mathf.Cos(minMaxTurretAngle.y * Mathf.Deg2Rad), Mathf.Sin(minMaxTurretAngle.y * Mathf.Deg2Rad)));

        Gizmos.DrawLine((Vector2)transform.position + new Vector2(Mathf.Cos(minMaxTurretAngle.x * Mathf.Deg2Rad), Mathf.Sin(minMaxTurretAngle.x * Mathf.Deg2Rad)),
            (Vector2)transform.position + new Vector2(Mathf.Cos((minMaxTurretAngle.x + minMaxTurretAngle.y) / 2 * Mathf.Deg2Rad), Mathf.Sin((minMaxTurretAngle.x + minMaxTurretAngle.y) / 2 * Mathf.Deg2Rad))
            );
        Gizmos.DrawLine((Vector2)transform.position + new Vector2(Mathf.Cos((minMaxTurretAngle.x + minMaxTurretAngle.y) / 2 * Mathf.Deg2Rad), Mathf.Sin((minMaxTurretAngle.x + minMaxTurretAngle.y) / 2 * Mathf.Deg2Rad)),
            (Vector2)transform.position + new Vector2(Mathf.Cos(minMaxTurretAngle.y * Mathf.Deg2Rad), Mathf.Sin(minMaxTurretAngle.y * Mathf.Deg2Rad)));
    
    }

    public bool CanTakeControll()
    {
        return true;
        //return !automatic;
    }

    public void ForwardCommandMove(Vector2 controll, Vector2 controllSmooth)
    {
        if (!powered) return;
        Vector2 diff = (controllSmooth).normalized;
        rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (rot_z < minMaxTurretAngle.x || rot_z > minMaxTurretAngle.y) rot_z = prevRotz; // works only if minmax is <-180, 180>
        if (rot_z > 90 || rot_z < -90) turretSprite.flipY = true;
        else turretSprite.flipY = false;
        gunBarrel.rotation = Quaternion.Euler(0, 0, rot_z);
        turretAngle = rot_z;
        prevRotz = rot_z;
    }
}
