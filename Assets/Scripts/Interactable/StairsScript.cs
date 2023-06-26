using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsScript : PoweredInteractable, ITakeDamageInteractable
{

    [SerializeField]
    StairsScript connectedDoors;
    [SerializeField]
    NavNode node;

    [SerializeField] AudioSource doorsClosingSFX;
    [SerializeField] AudioSource doorsDamageSFX;


    bool Closed
    {
        get
        {
            return closed;
        }
        set
        {
            if(closed != value)
            {
                closed = value;
                if(closed)
                {
                    doorsClosingSFX.Play();
                    staris.sprite = closedDoor;
                    node.obstaclesWeigths += doorNavStrength;
                }
                else
                {
                    doorsClosingSFX.Play();
                    staris.sprite = openDoor;
                    node.obstaclesWeigths -= doorNavStrength;
                }
            }
        }
    }
    bool closed;

    bool wantToBeClosed;

    [SerializeField] Sprite openDoor;
    [SerializeField] Sprite closedDoor;

    [SerializeField] Sprite fixedDoor;
    [SerializeField] Sprite brokenDoor;


    [SerializeField] SpriteRenderer panel;
    SpriteRenderer staris;

    [SerializeField]
    float doorNavStrength;

    [SerializeField]
    float maxHp;
    [SerializeField]
    float hp;

    
    bool destroyed;


    protected override void Awake()
    {
        base.Awake();
        staris = transform.GetComponent<SpriteRenderer>();
        AddAction(UseStairs);
        node  = transform.GetComponentInChildren<NavNode>();
    }

    void UseStairs(PlayerController player, UseType type)
    {
        if(type == UseType.Hand && !closed)
        {
            player.transform.position = connectedDoors.transform.position;
        }
        else if(powered && type == UseType.Computer)
        {
            CloseDoors();
            connectedDoors.SetDoorState(wantToBeClosed);
            RefreshState();
            NavController.instance.UpdateWeigths();
        }
    }
    public void CloseDoors()
    {
        wantToBeClosed = !wantToBeClosed;
    }

    public void SetDoorState(bool state)
    {
        wantToBeClosed = state;
        RefreshState();
    }

    void RefreshState()
    {
        if ((wantToBeClosed || connectedDoors.wantToBeClosed) && (powered || connectedDoors.powered) && !destroyed)
        {
            Closed = wantToBeClosed;
        }
        else
        {
            Closed = false;
        }

        if(destroyed)
        {
            panel.sprite = brokenDoor;
        }
        else
        {
            panel.sprite = fixedDoor;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if(connectedDoors != null) Gizmos.DrawLine(transform.position, connectedDoors.transform.position);
    }

    public StairsScript GetConnected()
    {
        return connectedDoors;
    }

    public override void PowerOn(bool on, string sectorName)
    {
        base.PowerOn(on, sectorName);
        RefreshState();
        connectedDoors.RefreshState();
    }

    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        float damageAmmount = damage;
        hp -= damageAmmount;
        doorsDamageSFX.Play();
        //parrent.SendWarning(WarningStrength.Medium);
        if (hp <= 0)
        {
            //parrent.SendWarning(WarningStrength.Strong);
            destroyed = true;
            connectedDoors.SynchHp(hp);
            RefreshState();
        }
        return damageAmmount;
    }

    public void SynchHp(float hp)
    {
        this.hp = hp;
        if (hp <= 0)
        {
            //parrent.SendWarning(WarningStrength.Strong);
            destroyed = true;
        }
        else
        {
            destroyed = false;
        }
        RefreshState();
    }


    public void Repair()
    {
        Heal(maxHp);
    }

    public float Heal(float ammount)
    {
        hp = Mathf.Min(ammount + hp, maxHp);
        if (hp > 0)
        {
            destroyed = false;
            connectedDoors.SynchHp(hp);
            RefreshState();
        }
        return ammount;
    }

    public void ApplyStatus(Status toApply)
    {
        throw new System.NotImplementedException();
    }

    public void TakeArmorDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public bool IsImmune()
    {
        return !Closed;
    }

    public bool IsArmored()
    {
        return false;
    }
    public override ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer() { hp = hp, maxHp = maxHp, showHp = true, name = displayName, showCharge = true, charged = powered };
    }

    public bool AlwaysAttack()
    {
        return false;
    }
}
