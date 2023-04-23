using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField]
    float damage = 5f;

    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if (playerController.GetMovementDirection().x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if(playerController.GetMovementDirection().x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ITakeDamage>() != null && collision.CompareTag("Enemy"))
        {
            ITakeDamage enemy = collision.GetComponent<ITakeDamage>();
            enemy.TakeDamage(damage, DamageType.Bullet);
        }
    }
}
