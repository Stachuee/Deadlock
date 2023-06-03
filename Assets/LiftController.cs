using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : InteractableBase
{
    public bool ElevatorOnThisFloor
    {
        get
        {
            return elevatorOnThisFloor;
        }
        set
        {
            if(value)
            {
                myRenderer.color = Color.black;
            }
            else
            {
                myRenderer.color = Color.white;
            }
            elevatorOnThisFloor = value;
        }
    }
    
    private bool elevatorOnThisFloor;
    private bool elevatorOnTheMove;

    [SerializeField]
    float liftCooldown;
    [SerializeField]
    float liftOpenDuration;
    [SerializeField]
    float liftClosing;
    [SerializeField]
    LiftController connectedLift;

    #region temp
    SpriteRenderer myRenderer;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        myRenderer = transform.GetComponent<SpriteRenderer>();
        AddAction(CallLift);
    }

    public void CallLift(PlayerController player, UseType type)
    {
        if(ElevatorOnThisFloor)
        {
            UseLift(player);
        }
        else if(!elevatorOnTheMove)
        {
            StartCoroutine("CallLiftAfterTime");
        }
    }

    IEnumerator CallLiftAfterTime()
    {
        elevatorOnTheMove = true;
        yield return new WaitForSeconds(liftCooldown);
        elevatorOnTheMove = false;
        ElevatorOnThisFloor = true;
        StartCoroutine("CloseLiftAfterTime");
    }

    IEnumerator CloseLiftAfterTime()
    {
        yield return new WaitForSeconds(liftOpenDuration);
        ElevatorOnThisFloor = false;
    }

    IEnumerator CloseLiftWithPlayer(PlayerController player)
    {
        yield return new WaitForSeconds(liftClosing);
        ElevatorOnThisFloor = false;
        player.UnlockInAnimation();
        player.transform.position = connectedLift.transform.position;
    }

    public void UseLift(PlayerController player)
    {
        if(ElevatorOnThisFloor)
        {
            StopCoroutine("CloseLiftAfterTime");
            player.LockInAction(ExitLift);
            StartCoroutine("CloseLiftWithPlayer", player);
        }
    }

    void ExitLift()
    {
        StopCoroutine("CloseLiftWithPlayer");
        StartCoroutine("CloseLiftAfterTime");
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (connectedLift != null) Gizmos.DrawLine(transform.position, connectedLift.transform.position);
    }
}
