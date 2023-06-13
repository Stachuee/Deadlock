using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsBetweenRooms : PoweredInteractable, ITakeDamage
{


    bool Closed
    {
        get
        {
            return close;
        }
        set
        {
            if (close != value)
            {
                if(value)
                {
                    node.obstaclesWeigths += doorNavStrength;
                }
                else
                {
                    node.obstaclesWeigths -= doorNavStrength;
                }
            }
            close = value;
        }
    }

    bool close;
    bool wantedState;

    Animator animator;

    [SerializeField]
    Rooms first;
    [SerializeField]
    Rooms second;

    bool multiSegment;

    [SerializeField]
    NavNode node;

    string firstPower;
    [SerializeField] bool firstPowerOn;
    string secondPower;
    [SerializeField] bool secondPowerOn;

    [SerializeField]
    float doorNavStrength;

    [SerializeField]
    float maxHp;
    [SerializeField]
    float hp;
    bool destroyed;

    private void Start()
    {
        animator = transform.GetComponent<Animator>();
        first.AddToInteractable(this);
        second.AddToInteractable(this);

        firstPower = first.GetMySegment().sectorName;
        secondPower = second.GetMySegment().sectorName;

        AddAction(Close);
    }

    public void SetParrents(Rooms first, Rooms second, NavNode node)
    {
        this.first = first;
        this.second = second;
        this.node = node;
    }


    public void Close(PlayerController player, UseType type)
    {
        if((firstPowerOn || secondPowerOn) && type == UseType.Computer && !destroyed)
        {
            wantedState = !Closed;
            RefreshState();
        }
    }

    public override void PowerOn(bool on, string sectorName)
    {
        base.PowerOn(on, sectorName);

        if(firstPower == sectorName)
        {
            firstPowerOn = on;
        }
        else if(secondPower == sectorName)
        {
            secondPowerOn = on;
        }
        RefreshState();
    }

    void RefreshState()
    {
        if((firstPowerOn || secondPowerOn) && !destroyed)
        {
            Closed = wantedState;
            animator.SetBool("Open", !Closed);
        }
        else
        {
            Closed = false;
            animator.SetBool("Open", true);
        }
    }

    public override ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer() { hp = hp, maxHp = maxHp, showHp = true, name = displayName, showCharge = true, charged = firstPowerOn || secondPowerOn };
    }


    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        float damageAmmount = damage;
        hp -= damageAmmount;
        //parrent.SendWarning(WarningStrength.Medium);
        if (hp <= 0)
        {
            //parrent.SendWarning(WarningStrength.Strong);
            destroyed = true;
            first.SendWarning(WarningStrength.Medium);
            second.SendWarning(WarningStrength.Medium);
            RefreshState();
        }
        return damageAmmount;
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


}
