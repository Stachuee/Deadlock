using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    PlayerController playerController;

    [Range(0, .3f)][SerializeField] float damping;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 maxCameraTilt;


    private void Start()
    {
        playerController = playerTransform.GetComponent<PlayerController>();
    }
    void Update()
    {
        transform.position = playerTransform.position + offset + new Vector3(playerController.currentAimDirection.x * maxCameraTilt.x, playerController.currentAimDirection.y * maxCameraTilt.y);
    }
}
