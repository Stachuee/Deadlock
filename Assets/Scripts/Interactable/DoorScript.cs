using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : PoweredInteractable, ITakeDamage
{
    bool opened;
    [SerializeField] GameObject door;
    [SerializeField] GameObject infectedDoor;
    [SerializeField] bool startInfected;
    [SerializeField] Blocade infected;

    [SerializeField]
    float maxHp;
    [SerializeField]
    float hp;

    public bool IsInfected 
    {
        get
        {
            return isInfected;
        }
        set
        {
            if(isInfected != value)
            {
                if(value)
                {
                    infectedDoor.SetActive(true);
                    door.SetActive(false);
                }
                else
                {
                    infectedDoor.SetActive(false);
                    if(powered && opened) door.SetActive(true);
                }
            }
            isInfected = value;
        }

    }

    bool isInfected;

    private void Start()
    {
        IsInfected = startInfected;
        if (!startInfected) Destroy(infected.gameObject);
        AddAction(UseDoor);
    }

    public void Use()
    {
        if (!powered || IsInfected) return;
        opened = !opened;
        door.SetActive(opened);
    }

    public void UseDoor(PlayerController player)
    {
        if (!powered || IsInfected) return;
        opened = !opened;
        door.SetActive(opened);
    }

    override public void PowerOn(bool on)
    {
        powered = on;
        if(!on)
        {
            door.SetActive(false);
        }
    }

    public float TakeDamage(float damage, DamageType type)
    {
        float damageAmmount = damage;
        hp -= damageAmmount;
        return damageAmmount;
    }


    public override ComputerInfoContainer GetInfo()
    {
        if (IsInfected) return infected.GetInfo();
        else return new ComputerInfoContainer { hp = hp, maxHp = maxHp, showHp = true, name = displayName };
    }

    public override void Highlight()
    {
        
    }
    public override void UnHighlight()
    {

    }
}
