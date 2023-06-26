using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : ActiveEnemy
{
    [SerializeField] float jumpCooldown;
    [SerializeField] float passiveJumpCooldown;
    [SerializeField] float jumpStrength;
    [SerializeField] float passiveJumpStrength;
    float nextJump;
    bool inAir;
    bool onCooldown;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(inAir && collision.transform.tag == "Floor")
        {
            inAir = false;
            animator.SetBool("Jumping", false);
        }
    }

    protected override void EnrageMovment()
    {

        if(!inAir && !onCooldown)
        {
            onCooldown = true;
            nextJump = Time.time + jumpCooldown;
        }
        else if(onCooldown && nextJump <= Time.time)
        {
            Vector2 jumpVector = (target - (Vector2)transform.position);
            animator.SetBool("Jumping", true);
            rb.AddForce(new Vector2(jumpVector.x > 0 ? 1 : -1, 1).normalized * jumpStrength);
            onCooldown = false;
            inAir = true;
        }
    }

    protected override void PassiveMovment()
    {
        Vector2 direction = new Vector2((currentTargetNode.transform.position - transform.position).x, 0);

        if (!inAir && !onCooldown)
        {
            onCooldown = true;
            nextJump = Time.time + passiveJumpCooldown;
        }
        else if (onCooldown && nextJump <= Time.time)
        {
            Vector2 jumpVector = (currentTargetNode.transform.position - transform.position);
            animator.SetBool("Jumping", true);
            rb.AddForce(new Vector2(jumpVector.x > 0 ? 1 : -1, 1).normalized * passiveJumpStrength);
            onCooldown = false;
            inAir = true;
        }

        if (direction.x > currentTargetNode.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (Mathf.Abs(transform.position.x - currentTargetNode.transform.position.x) < 0.2f)
        {

            if (currentTargetNode.navNodeType == NavNode.NavNodeType.Horizontal)
            {
                FindNextNodeTarget();
            }
            else if (currentTargetNode.navNodeType == NavNode.NavNodeType.Stairs)
            {
                FindNextNodeTarget();
                if (!exitedStairs && currentTargetNode.navNodeType == NavNode.NavNodeType.Stairs)
                {
                    transform.position = currentTargetNode.transform.position;
                    exitedStairs = true;
                }
                else
                {
                    exitedStairs = false;
                }
            }
        }
    }
}
