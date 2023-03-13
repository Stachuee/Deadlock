using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField]
    float damage = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ITakeDamage>() != null && collision.CompareTag("Enemy"))
        {
            ITakeDamage enemy = collision.GetComponent<ITakeDamage>();
            Debug.Log("kicked");
            enemy.TakeDamage(damage, DamageType.Bullet);
        }
    }
}
