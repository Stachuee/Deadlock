using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistTurret : InteractableBase, ITakeControll, IControllSubscriberMovment, IControllSubscriberShoot
{
    [SerializeField]
    SpriteRenderer turretSprite;
    [SerializeField]
    Transform turretBarrel;

    PlayerController user;

    public void ForwardCommandMovment(Vector2 controll)
    {
        Vector2 diff = (controll).normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (rot_z > 90 || rot_z < -90) turretSprite.flipY = true;
        else turretSprite.flipY = false;
        turretBarrel.rotation = Quaternion.Euler(0,0, rot_z);
    }

    public void ForwardCommandShoot(bool isShooting)
    {
        Debug.Log(isShooting);
    }

    protected override void Awake()
    {
        base.Awake();
        AddAction(TakeControll);
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
}
