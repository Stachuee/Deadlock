using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{

    [SerializeField]
    GameObject throwable;

    PlayerController playerController;


    private void Awake()
    {
        playerController = transform.GetComponent<PlayerController>();
    }

    public void UseEquipment()
    {
        GameObject temp = Instantiate(throwable, transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody2D>().AddForce(playerController.currentAimDirection.normalized * playerController.playerInfo.throwStrength);
    }
}
