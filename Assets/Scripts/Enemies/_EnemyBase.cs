using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _EnemyBase : MonoBehaviour, ITakeDamage
{
    [SerializeField]
    protected float maxHp;
    protected float hp;
    [SerializeField, MinMaxSlider(0, 10)]
    protected Vector2 randomSpeed;
    protected float speed;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float attackSpeed;
    protected float lastAttack;

    [SerializeField]
    DamageTypeResistance resistances;

    bool poisoned = false;
    bool onFire = false;
    bool freezed = false;

        protected virtual void Start()
    {
        hp = maxHp;
        speed = Random.Range(randomSpeed.x, randomSpeed.y);
    }

    public float TakeDamage(float damage, DamageType type)
    {
        float damageTaken = damage;
        switch (type)
        {
            case DamageType.Bullet:
                damageTaken = (1 - resistances.GetResistance(type)) * damage;
                hp -= damageTaken;
                break;
            case DamageType.Poison:
                if (poisoned) break;
                damageTaken = (1 - resistances.GetResistance(type)) * damage;
                StartCoroutine(PoisonDamage(3, damageTaken));
                break;
            case DamageType.Fire:
                if (onFire) break;
                damageTaken = (1 - resistances.GetResistance(type)) * damage;
                StartCoroutine(FireDamage(3, damageTaken));
                break;
            case DamageType.Ice:
                if (freezed) break;
                damageTaken = (1 - resistances.GetResistance(type)) * 0.5f;
                StartCoroutine(Freeze(3, damageTaken));
                break;
            case DamageType.Mele:
                damageTaken = (1 - resistances.GetResistance(type)) * damage;
                hp -= damageTaken;
                break;
            case DamageType.Disintegrating:
                float dResistance = resistances.GetResistance(DamageType.Bullet);
                if (dResistance > 0.5f)
                {
                    damageTaken = damage * (1 - (dResistance - 0.5f));
                }
                else
                {
                    damageTaken = damage;
                }
                hp -= damageTaken;
                break;
            default:
                Debug.LogError($"Invalid DamageType: {type} for Enemy");
                break;
        }
        

        if (hp <= 0) Dead();
        return damageTaken;
    }

    public void TakeArmorDamage(DamageType type, float damage)
    {
        resistances.SetResistance(type, resistances.GetResistance(type) - damage);
    }

    public virtual void Dead()
    {
        Destroy(gameObject);
    }

    private IEnumerator PoisonDamage(int poisonStrenght, float damage)
    {
        poisoned = true;

        while (poisonStrenght > 0)
        {
            yield return new WaitForSeconds(1f);
            hp -= damage;
            poisonStrenght--;
        }
        poisoned = false;
    }

    private IEnumerator FireDamage(int fireStrenght, float damage)
    {
        onFire = true;

        while (fireStrenght > 0)
        {
            yield return new WaitForSeconds(1f);
            hp -= damage;
            fireStrenght--;
        }
        onFire = false;
    }

    private IEnumerator Freeze(int iceStrenght, float damage)
    {
        freezed = true;
        float tmpSpeed = speed;
        speed *= damage;
        yield return new WaitForSeconds(iceStrenght);
        freezed = false;
        speed = tmpSpeed;
    }
}
