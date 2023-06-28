using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSpitter : ActiveEnemy
{
    [SerializeField] GameObject spitProjectal;

    [SerializeField] float shootCooldown;
    float nextShoot;
    [SerializeField] float projectalSpeed;

    [SerializeField] Transform mouth;
    [SerializeField] LayerMask lineOfSightMask;


    protected override void Update()
    {
        base.Update();
        if(state == EnemyState.Hunting && nextShoot <= Time.time)
        {
            nextShoot = Time.time + shootCooldown;
            Vector2 direction = ((PlayerController.solider.transform.position + Vector3.up * 1) - mouth.position).normalized;
            RaycastHit2D lineOfSight = Physics2D.Raycast(mouth.position, direction, Mathf.Infinity, ~lineOfSightMask);
            if (lineOfSight)
            {
                GameObject temp = Instantiate(spitProjectal, mouth.position, Quaternion.identity);
                temp.GetComponent<Rigidbody2D>().AddForce(direction * projectalSpeed);
                temp.transform.right = direction;
            }
        }
    }
}
