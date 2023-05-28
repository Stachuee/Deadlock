using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : PoweredInteractable
{
    bool opened;
    [SerializeField] GameObject door;
    [SerializeField] GameObject openDoors;

    [SerializeField]
    float maxHp;
    [SerializeField]
    float hp;

    Rooms parrent;

    bool destroyed;

    private void Start()
    {
        parrent = transform.GetComponentInParent<Rooms>();
        AddAction(UseDoor);
    }

    public void UseDoor(PlayerController player)
    {
        if (!powered || destroyed) return;
        opened = !opened;
        door.SetActive(opened);
        openDoors.SetActive(!opened);
    }

    override public void PowerOn(bool on)
    {
        powered = on;
        if(!on || destroyed)
        {
            door.SetActive(false);
            openDoors.SetActive(true);
        }
    }

    public override ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer { hp = hp, maxHp = maxHp, showHp = true, name = displayName };
    }

    public void Repair()
    {
        hp = maxHp;
    }

    public override void Highlight()
    {
        
    }
    public override void UnHighlight()
    {

    }

    public float TakeDamage(float damage)
    {
        float damageAmmount = damage;
        hp -= damageAmmount;
        parrent.SendWarning(WarningStrength.Medium);
        if (hp <= 0)
        {
            parrent.SendWarning(WarningStrength.Strong);
            door.SetActive(false);
            openDoors.SetActive(true);
            destroyed = true;
        }
        return damageAmmount;
    }

    public float Heal(float ammount)
    {
        hp = Mathf.Min(ammount + hp, maxHp);
        return ammount;
    }

}
