using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class ScientistTurret : InteractableBase, ITakeControll, IControllSubscriberMovment, IControllSubscriberShoot
{
    [MinMaxSlider(-180, 0)]
    public Vector2Int minMaxTurretAngle;

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
    float nextShot;

    float turretAngle;
    bool firing;

    PlayerController user;
    float prevRotz = 0;

    public void ForwardCommandMovment(Vector2 controll)
    {
        Vector2 diff = (controll).normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
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
    }
    private void Update()
    {
        if (firing && nextShot < Time.time)
        {
            Instantiate(bulletPrefab, barrelTransform.position, Quaternion.Euler(0, 0, turretAngle));
            nextShot = Time.time + shootDelay;
        }
    }

    public void TakeControll(PlayerController player)
    {
        user = player;
        player.AddMovmentSubscriber(this);
        player.AddShootSubscriber(this);
    }

    public void Leave()
    {
        user.RemoveMovmentSubscriber(this);
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
}
