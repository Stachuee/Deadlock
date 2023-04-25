using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocade : MonoBehaviour, ITakeDamage
{

    [SerializeField]
    float maxHp;
    [SerializeField]
    float hp;
    [SerializeField] string displayName;
    [SerializeField] DoorScript door;

    [SerializeField]
    GameObject bossToSpawn;
    bool bossSpawned;

    void Start()
    {
        hp = maxHp;
    }

    public float TakeDamage(float damage, DamageType type)
    {
        float damageAmmount = damage;
        //if (type == DamageType.Fire)
        //{
        //    damage *= 1.5f;
        //}
        hp -= damageAmmount;
        if (hp < 0)
        {
            door.IsInfected = false;
            Destroy(gameObject);
        }
        else if (hp / maxHp < 0.25f && !bossSpawned)
        {
            bossSpawned = true;
            Instantiate(bossToSpawn, transform.position, Quaternion.identity);
        }
        return damageAmmount;
    }

    public ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer { hp = hp, maxHp = maxHp, showHp = true, name = displayName };
    }
}
