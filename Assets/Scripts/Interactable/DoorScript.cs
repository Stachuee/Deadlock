using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : PoweredInteractable
{
    bool opened;
    [SerializeField] GameObject door;
    [SerializeField] GameObject infectedDoor;

    [SerializeField]
    float maxHp;
    [SerializeField]
    float hp;


    private void Start()
    {
        AddAction(UseDoor);
    }

    public void Use()
    {
        if (!powered) return;
        opened = !opened;
        door.SetActive(opened);
    }

    public void UseDoor(PlayerController player)
    {
        if (!powered) return;
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
        return new ComputerInfoContainer { hp = hp, maxHp = maxHp, showHp = true, name = displayName };
    }

    public override void Highlight()
    {
        
    }
    public override void UnHighlight()
    {

    }
}
