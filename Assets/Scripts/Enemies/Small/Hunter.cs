using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : ActiveEnemy
{
    [SerializeField] float jumpCooldown;
    [SerializeField] float jumpStrength;
    float nextJump;
    bool inAir;
    bool onCooldown;

    protected override void EnrageMovment()
    {
        if(rb.velocity.y < 0.1f)
        {
            inAir = false;
            animator.SetTrigger("endFlying");
        }
        else
        {
            inAir = true;
        }

        if(!inAir && !onCooldown)
        {
            onCooldown = true;
            nextJump = Time.time + jumpCooldown;
        }
        else if(onCooldown && nextJump <= Time.time)
        {
            Vector2 jumpVector = (target - (Vector2)transform.position);
            rb.AddForce(new Vector2(jumpVector.x > 0 ? 1 : -1, 1).normalized * jumpStrength);
            onCooldown = false;
        }
    }
}
