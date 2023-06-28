using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    public void ThrowEquipment()
    {
        playerController.SetTrigger(false, "Throwing");
        playerController.equipmentController.FinishThrowing();
    }

    public void FinishStanding()
    {
        playerController.FinishStanding();
    }

}
